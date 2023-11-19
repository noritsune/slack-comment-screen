using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using USlack;

namespace SCS
{
    public class SlackCommentSource : MonoBehaviour, ICommentSource
    {
        [SerializeField] StartWindow _startWindow;

        public Action<string> OnNewComment { get; set; }
        public Action<Exception> OnError { get; set; }

        SlackClient _client;
        ICommentSource _commentSourceImplementation;
        // DateTimeだとタイムゾーンが不明だしUnixTimeへの変換が面倒なのでDateTimeOffsetを使う
        string _listenStartTimeStampStr;
        string _channelId;
        string _threadTs;
        readonly HashSet<string> _oldMsgIds = new ();

        // conversations.repliesAPIはTier1なので1分間に50回しか呼べない
        // エラーが起きないようにちょっと長めにしてみた
        const float INTERVAL_SEC = 1.3f;

        void Awake()
        {
            _startWindow.OnSubmit += StartListening;
        }

        /**
         * @param threadUrl 例: https://hoge.slack.com/archives/C01Q6TAGNBZ/p1700255769657409
         */
        void StartListening(string token, string channelId, string threadTs)
        {
            _client = new SlackClient(token);
            _channelId = channelId;
            _threadTs = threadTs;
            _listenStartTimeStampStr = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            InvokeRepeating(nameof(FetchMessages), 0, INTERVAL_SEC);
        }

        async Task FetchMessages()
        {
            try
            {
                var msgs = await _client.ConversationsReplies(
                    _channelId,
                    _threadTs,
                    oldest: _listenStartTimeStampStr
                );
                var newMsgs = msgs.Messages
                    // 1つ目はスレッドのルートメッセージなので無視
                    .Skip(1)
                    // 前回の実行以降のメッセージを取得するようにしたら取りこぼしと重複取得が発生した
                    // そのため、スレッド内の全メッセージを取ってきてフィルタをかける方式にした
                    .Where(msg => !_oldMsgIds.Contains(msg.Id))
                    .ToList();
                foreach (var newMsg in newMsgs)
                {
                    OnNewComment?.Invoke(newMsg.Text);
                    _oldMsgIds.Add(newMsg.Id);
                }
            }
            catch (Exception e)
            {
                OnError?.Invoke(e);
            }
        }
    }
}
