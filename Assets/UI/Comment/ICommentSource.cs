using System;

namespace SCS
{
    public interface ICommentSource
    {
        Action<string> OnNewComment { get; set; }
        Action<Exception> OnError { get; set; }
    }
}
