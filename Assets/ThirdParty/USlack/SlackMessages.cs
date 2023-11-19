using System;
using UnityEngine;

namespace USlack
{
    [Serializable]
    public class SlackMessages
    {
        [SerializeField] SlackMessage[] messages;

        public SlackMessage[] Messages => messages;
    }
}
