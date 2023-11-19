using UnityEngine;

namespace SCS
{
    public static class UserDataRepository
    {
        public static string LoadToken()
        {
            return PlayerPrefs.GetString("token");
        }

        public static string LoadThreadUrl()
        {
            return PlayerPrefs.GetString("threadUrl");
        }

        public static void SaveToken(string token)
        {
            PlayerPrefs.SetString("token", token);
        }

        public static void SaveThreadUrl(string threadUrl)
        {
            PlayerPrefs.SetString("threadUrl", threadUrl);
        }
    }
}
