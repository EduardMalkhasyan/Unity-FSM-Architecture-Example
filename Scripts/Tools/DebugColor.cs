using UnityEngine;

namespace BugiGames.Tools
{
    public static class DebugColor
    {
        public static void LogOrange(string text, bool bold = false)
        {
            if (bold)
            {
                Debug.Log($"<color=#CC7F00><b>{text}</b></color>");
            }
            else
            {
                Debug.Log($"<color=#CC7F00>{text}</color>");
            }
        }

        public static void LogGreen(string text, bool bold = false)
        {
            if (bold)
            {
                Debug.Log($"<color=#00CC55><b>{text}</b></color>");
            }
            else
            {
                Debug.Log($"<color=#00CC55>{text}</color>");
            }
        }

        public static void LogRed(string text, bool bold = false)
        {
            if (bold)
            {
                Debug.Log($"<color=#CC1F00><b>{text}</b></color>");
            }
            else
            {
                Debug.Log($"<color=#CC1F00>{text}</color>");
            }
        }

        public static void LogBlue(string text, bool bold = false)
        {
            if (bold)
            {
                Debug.Log($"<color=#0084CC><b>{text}</b></color>");
            }
            else
            {
                Debug.Log($"<color=#0084CC>{text}</color>");
            }
        }

        public static void LogViolet(string text, bool bold = false)
        {
            if (bold)
            {
                Debug.Log($"<color=#B476E5><b>{text}</b></color>");
            }
            else
            {
                Debug.Log($"<color=#B476E5>{text}</color>");
            }
        }
    }
}

