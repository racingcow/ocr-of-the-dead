using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace ocr_api.Helpers
{
    public static class DynamicHelper
    {
        public const char DELIMITER_OUTER = '&';
        public const char DELIMITER_INNER = '=';

        public static string PropsToKey(dynamic obj, params string[] propNames)
        {
            var dict = (IDictionary<string, object>) obj;
            var pairs = propNames.Select(propName => $"{propName}{DELIMITER_INNER}{dict[propName].ToString()}").ToList();
            return string.Join(DELIMITER_OUTER.ToString(), pairs).Base64Encode();
        }

        public static void KeyToProps(this string key, dynamic obj)
        {
            var dict = (IDictionary<string, object>) obj;
            var pairs = key.Base64Decode().Split(DELIMITER_OUTER);
            foreach (var pair in pairs)
            {
                var kvp = pair.Split(DELIMITER_INNER);
                var name = kvp[0];
                var val = kvp[1];
                if (dict.ContainsKey(name))
                {
                    dict[name] = val;
                }
                else
                {
                    dict.Add(name, val);
                }
            }
        }
    }
}
