using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace xLiAd.AspectCache.Core
{
    /// <summary>
    /// 默认的 缓存Key的提供者
    /// </summary>
    public class DefaultKeyProvider : IKeyProvider
    {
        /// <summary>
        /// 根据上下文信息生成缓存Key
        /// </summary>
        /// <param name="cacheKeyTemplete"></param>
        /// <param name="parameters"></param>
        /// <param name="classImplementation"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string ProvideKey(string cacheKeyTemplete, object[] parameters, object classImplementation, MethodInfo method)
        {
            string key;
            if (string.IsNullOrEmpty(cacheKeyTemplete))
            {
                key = classImplementation.GetType().FullName + ":" + method.Name + ":";
                string ka = Newtonsoft.Json.JsonConvert.SerializeObject(parameters);
                key += ka;
            }
            else
            {
                key = cacheKeyTemplete.Replace("{class}", classImplementation.GetType().FullName)
                    .Replace("{method}", method.Name);
                var matches = Regex.Matches(key, "{arg([\\w|\\.|_]+)}");
                foreach(Match match in matches)
                {
                    var val = match.Value.Substring(4, match.Length - 5);
                    var valPropertyArray = val.Split('.');
                    var paramIndex = int.TryParse(valPropertyArray[0], out int v) ? v : -1;
                    if (paramIndex < 0)
                        throw new ArgumentException("Unknown arg -- arg" + valPropertyArray[0], "arg" + valPropertyArray[0]);
                    if (paramIndex >= parameters.Length)
                        throw new ArgumentOutOfRangeException("arg" + paramIndex, "arg" + paramIndex + " dosn't exists");
                    var obj = parameters[paramIndex];
                    foreach (var s in valPropertyArray.Skip(1))
                    {
                        var mem = obj.GetType().GetMember(s, System.Reflection.MemberTypes.Field | System.Reflection.MemberTypes.Property, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).FirstOrDefault();
                        if (mem == null)
                            throw new ArgumentException("Unknown arg " + s, s);
                        if (mem.MemberType == System.Reflection.MemberTypes.Field)
                            obj = (mem as FieldInfo).GetValue(obj);
                        else
                            obj = (mem as PropertyInfo).GetValue(obj);
                    }
                    key = key.Replace($"{{arg{val}}}", obj.ToString());
                }
            }
            return key;
        }
    }
}
