using TMPro;
using UnityEngine;

namespace SCS
{
    /**
     * 画面左に流れていくUI要素
     */
    public class FlowComment : MonoBehaviour
    {
        [SerializeField] float _speed;
        [SerializeField] TextMeshProUGUI _text;

        RectTransform _rectTrans;

        void Awake()
        {
            _rectTrans = GetComponent<RectTransform>();
        }

        void Update()
        {
            _rectTrans.Translate(Vector3.left * (_speed * Time.deltaTime));
            if (_rectTrans.position.x < -_rectTrans.sizeDelta.x / 2)
            {
                Destroy(gameObject);
            }
        }

        public void Init(string text)
        {
            _text.text = text;

            var pos = _rectTrans.position;
            // 画面の右外側から出現する
            // この時点では横幅が更新されていないのでsizeDeltaは使えない
            pos.x = Screen.width + _text.preferredWidth / 2;
            // 上下方向に見切れない様にする
            var topLimit = _rectTrans.sizeDelta.y / 2;
            var bottomLimit = Screen.height - _rectTrans.sizeDelta.y / 2;
            pos.y = Random.Range(topLimit, bottomLimit);
            _rectTrans.position = pos;
        }
    }
}
