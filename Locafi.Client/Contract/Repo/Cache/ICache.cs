using System;
using System.Collections.Generic;

namespace Locafi.Client.Contract.Repo.Cache
{
    public interface ICache<T>
    {
        void Push(ICachedEntity<T> entity);
        IList<ICachedEntity<T>> CopyCache(int? maxCopy = null);
        void Remove(string id);
    }
}
