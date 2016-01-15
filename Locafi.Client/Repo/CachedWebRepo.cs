using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Config;
using Locafi.Client.Contract.Http;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Contract.Repo.Cache;
using Locafi.Client.Model;
using Locafi.Client.Model.Dto;
using Locafi.Client.Repo.Cache;

namespace Locafi.Client.Repo
{
    public abstract class CachedWebRepo : WebRepo
    {
        protected CachedWebRepo(IHttpTransferer transferer, IAuthorisedHttpTransferConfigService authorisedConfigService, ISerialiserService serialiser, string service)
            : base(transferer, authorisedConfigService, serialiser, service)
        {
        }

        protected CachedWebRepo(IHttpTransferer transferer, IHttpTransferConfigService configService, ISerialiserService serialiser, string service)
            : base(transferer, configService, serialiser, service)
        {
        }

        public async Task<ICachedResponse<T>> Post<T, TData>(TData data, string extra = "", ICache<TData> cache = null) where T : class, new() where TData : ICacheable, new()
        {
            try
            {
                // we need to check the types of exceptions thrown when offline and handle gracefully
                var result = await base.Post<T>(data, extra);
                return new WebRepoCacheResult<T>(result, true, false);
            }
            catch (Exception ex)
            {
                if (cache == null)
                {
                    return new WebRepoCacheResult<T>(null, false, false, ex);
                }
                else
                {
                    var cacheEntity = new CachedEntity<TData>(data.Id, data, extra);
                    cache.Push(cacheEntity);
                    return new WebRepoCacheResult<T>(null, false, true);
                }
            }
        }

        public async Task<IList<ICachedResponse<T>>> PostCache<T, TData>(ICache<TData> cache, int? amount = null) where T : class, new() where TData : ICacheable, new()
        {
            var list = new List<ICachedResponse<T>>();            
            foreach (var cachedEntity in cache.CopyCache(amount))
            {
                var result = await base.Post<T>(cachedEntity.Entity, cachedEntity.Extra);
                if (result == null) break;
                list.Add(new WebRepoCacheResult<T>(result, true, false));
                cache.Remove(cachedEntity.Id);
            }
            return list;
        }

        
    }
}
