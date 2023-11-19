using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCS
{
    public class StartWindow : MonoBehaviour
    {
        [SerializeField] GameObject _content;
        [SerializeField] TMP_InputField _tokenInput;
        [SerializeField] TMP_InputField _threadUrlInput;
        [SerializeField] Button _submitButton;

        public Action<string, string, string> OnSubmit;

        void Awake()
        {
            _tokenInput.text = UserDataRepository.LoadToken();
            _threadUrlInput.text = UserDataRepository.LoadThreadUrl();

            _tokenInput.onValueChanged.AddListener(_ => UpdateSubmitBtnInteractiveState());
            _threadUrlInput.onValueChanged.AddListener(_ => UpdateSubmitBtnInteractiveState());
            _submitButton.onClick.AddListener(HandleOnSubmit);

            UpdateSubmitBtnInteractiveState();
        }

        void UpdateSubmitBtnInteractiveState()
        {
            _submitButton.interactable = !string.IsNullOrEmpty(_tokenInput.text)
                                      && !string.IsNullOrEmpty(_threadUrlInput.text);
        }

        void HandleOnSubmit()
        {
            var token = _tokenInput.text;
            // 例: https://hoge.slack.com/archives/C01Q6TAGNBZ/p1700255769657409
            var url = _threadUrlInput.text;
            var channelId = url.Split('/')[4];
            var threadTs = url.Split('/')[5]
                // 1文字目はpなので2文字目から
                .Substring(1)
                // 下6桁は小数点以下の桁なのでその区切りに小数点を入れる
                .Insert(10, ".");

            UserDataRepository.SaveToken(token);
            UserDataRepository.SaveThreadUrl(url);

            OnSubmit?.Invoke(token, channelId, threadTs);
            _content.SetActive(false);
        }
    }
}
