using UnityEngine;

namespace Assets.Scripts
{
    public static class Utils
    {
        public static void DebugLog(this Vector3 v)
        {
            Debug.Log(string.Format("{0},{1},{2}", v.x, v.y, v.z));
        }
    }

    public static class Random
    {
        public static bool Boolean()
        {
            return UnityEngine.Random.value >= 0.5;
        }
    }

}