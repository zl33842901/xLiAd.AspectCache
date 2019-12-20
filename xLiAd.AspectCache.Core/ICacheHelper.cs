using System;
using System.Collections.Generic;
using System.Text;

namespace xLiAd.AspectCache.Core
{
    public interface ICacheHelper
    {
        void Set(string key, object value, TimeSpan timeSpan);

        object Get(string key, Type type);

        T Get<T>(string key);

        void DeleteByPattern(string key);

        void Delete(string key);
    }
}
