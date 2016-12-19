using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locafi.Client.Contract.Repo;
using Locafi.Client.Contract.Repo.Cache;

namespace Locafi.Client.Repo.Cache
{
    public class WebRepoCacheResult<TData> : ICachedResponse<TData>
    {
        public WebRepoCacheResult(TData data, bool uploaded, bool cached, Exception error = null)
        {
            Data = data;
            Uploaded = uploaded;
            Cached = cached;
            Error = error;
        }     

        public TData Data { get; }
        public bool Uploaded { get; }
        public bool Cached { get; }
        public Exception Error { get; }
    }
}
