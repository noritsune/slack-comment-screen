using System;
using UnityEngine;

namespace USlack
{
    [Serializable]
    public class SlackResponse
    {
        [SerializeField] bool ok;
        [SerializeField] string error;

        public bool Ok => ok;
        public string Error => error;
    }
}
