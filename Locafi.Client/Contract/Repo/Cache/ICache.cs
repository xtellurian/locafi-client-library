using System.Collections.Generic;

namespace Locafi.Client.Contract.Repo.Cache
{
    public interface ICache<T>
    {
        void Push(ICachedEntity<T> entity);
        IEnumerable<ICachedEntity<T>> CopyCache();
        void Remove(string id);
    }
}
