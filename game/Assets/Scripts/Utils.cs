using System.Collections.Specialized;
using System.ComponentModel;
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

    //public class Random
    //{
    //    public static bool Boolean()
    //    {
    //        return UnityEngine.Random.value >= 0.5;
    //    }
    //}

    public static class Extensions
    {
        public static NameValueCollection ToNameValueCollection<T>(this T dynamicObject)
        {
            var nameValueCollection = new NameValueCollection();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynamicObject))
            {
                var val = propertyDescriptor.GetValue(dynamicObject);
                if (val == null) continue;

                var value = val.ToString();
                nameValueCollection.Add(propertyDescriptor.Name, value);
            }
            return nameValueCollection;
        }
    }

}