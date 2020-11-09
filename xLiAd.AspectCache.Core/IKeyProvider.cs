using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace xLiAd.AspectCache.Core
{
    /// <summary>
    /// 缓存Key的提供者
    /// </summary>
    public interface IKeyProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKeyTemplete"></param>
        /// <param name="parameters"></param>
        /// <param name="classImplementation"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        string ProvideKey(string cacheKeyTemplete, object[] parameters, object classImplementation, MethodInfo method);
    }
}
