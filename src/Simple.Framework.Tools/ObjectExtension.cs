using System.Reflection;
using Simple.Framework.Tools;
using Newtonsoft.Json;

namespace System
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 实体转换
        /// </summary>
        /// <typeparam name="T">转换的类型</typeparam>
        /// <param name="obj">Value</param>
        /// <returns></returns>
        public static T Map<T>(this object obj)
        {
            return Mapping.Convert<T>(obj);
        }

        /// <summary>
        /// 提示转换为字典类型
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, object>? ToDictionary(this object obj)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(obj), new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            });
        }
    }
}