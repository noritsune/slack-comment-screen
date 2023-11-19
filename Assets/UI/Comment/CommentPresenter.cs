using System;
using TNRD;
using UnityEngine;

namespace SCS
{
    public class CommentPresenter : MonoBehaviour
    {
        [SerializeField] FlowComment _commentPrefab;
        [SerializeField] Transform _commentParent;
        [SerializeField] SerializableInterface<ICommentSource> _commentSource;
        [SerializeField] ErrorWindow _errorWindow;

        void Awake()
        {
            _commentSource.Value.OnNewComment += HandleOnNewComment;
            _commentSource.Value.OnError += ShowErrorWindow;
        }

        void HandleOnNewComment(string text)
        {
            var comment = Instantiate(_commentPrefab, _commentParent);
            comment.Init(text);
        }

        void ShowErrorWindow(Exception e)
        {
            if (_errorWindow.IsShowing) return;

            _errorWindow.Show($"{e.Message}\n\n{e.StackTrace}");
        }
    }
}
