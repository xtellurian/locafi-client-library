using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Contract.Repo.Cache;

namespace Locafi.Client.Repo.Cache
{
    public class CachedEntity <T> : ICachedEntity<T>
    {
        public CachedEntity(string id, T entity,  string extra)
        {
            Id = id;
            Entity = entity;
            Extra = extra;
        }

        public string Id { get; set; }
        public T Entity { get; set; }
        public string Extra { get; set; }
    }
}
