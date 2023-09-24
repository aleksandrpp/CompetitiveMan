using UnityEngine;

namespace AK.CompetitiveMan
{
    public static class Utils
    {
        public static float Clamp(this float angle, float min, float max)
        {
            angle %= 360;
            if (angle is >= -360 and <= 360)
            {
                if (angle < -360) angle += 360;
                if (angle > 360) angle -= 360;
            }

            return Mathf.Clamp(angle, min, max);
        }
    }
}