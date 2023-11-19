using System;
using UnityEngine;

namespace SCS
{
    public class RandomCommentSource : MonoBehaviour, ICommentSource
    {
        [SerializeField] float _intervalSec;

        public Action<string> OnNewComment { get; set; }
        public Action<Exception> OnError { get; set; }

        void Awake()
        {
            InvokeRepeating(nameof(GenerateComment), 0, _intervalSec);
        }

        void GenerateComment()
        {
            OnNewComment?.Invoke("初見です😁😀🤚🐼");
        }
    }
}
