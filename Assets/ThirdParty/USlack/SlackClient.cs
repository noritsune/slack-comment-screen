using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace USlack
{
    public class SlackClient
    {
        readonly string _token;

        public SlackClient(string token)
        {
            _token = token;
        }

        public async Task<SlackMessages> ConversationsReplies(string channelId, string ts, string oldest = null)
        {
            var form = new WWWForm();
            form.AddField("channel", channelId);
            form.AddField("ts", ts);
            if (oldest != null) form.AddField("oldest", oldest);

            var json = await Post("conversations.replies", form);
            return JsonUtility.FromJson<SlackMessages>(json);
        }

        Task<string> Post(string method, WWWForm form)
        {
            var url = $"https://slack.com/api/{method}";
            var req = UnityWebRequest.Post(url, form);
            req.SetRequestHeader("Authorization", $"Bearer {_token}");
            var tcs = new TaskCompletionSource<string>();
            req.SendWebRequest().completed += _ =>
            {
                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(req.error);
                    tcs.SetException(new Exception(req.error));
                    return;
                }

                var json = req.downloadHandler.text;
                var res = JsonUtility.FromJson<SlackResponse>(json);
                if (!res.Ok)
                {
                    Debug.LogError(res.Error);
                    tcs.SetException(new Exception(res.Error));
                    return;
                }
                tcs.SetResult(json);
            };
            return tcs.Task;
        }
    }
}
