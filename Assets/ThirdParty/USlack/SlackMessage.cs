using System;
using UnityEngine;

namespace USlack
{
    [Serializable]
    public class SlackMessage
    {
        [SerializeField] string client_msg_id;
        [SerializeField] string text;

        public string Id => client_msg_id;
        public string Text => text;
    }
}
