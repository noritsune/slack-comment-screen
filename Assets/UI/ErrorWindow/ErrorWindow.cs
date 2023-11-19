using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SCS
{
    public class ErrorWindow : MonoBehaviour
    {
        [SerializeField] GameObject _content;
        [SerializeField] TextMeshProUGUI _msg;
        [SerializeField] Button _closeBtn;

        public bool IsShowing => _content.activeSelf;

        void Awake()
        {
            _closeBtn.onClick.AddListener(Hide);
        }

        public void Show(string msg)
        {
            _content.SetActive(true);
            _msg.text = msg;
        }

        void Hide()
        {
            Application.Quit();
        }
    }
}
