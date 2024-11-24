using UnityEngine;

namespace Utils
{
    public static class Formatting
    {
        public static string FormatTime(float seconds)
        {
            int minutes = Mathf.FloorToInt(seconds / 60); // Calculate total minutes
            int remainingSeconds = Mathf.FloorToInt(seconds % 60); // Calculate remaining seconds

            return $"{minutes:00}:{remainingSeconds:00}"; // Format as MM:SS
        }
    }
}