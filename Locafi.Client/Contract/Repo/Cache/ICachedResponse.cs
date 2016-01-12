using System;

namespace Locafi.Client.Contract.Repo.Cache
{
    public interface ICachedResponse<out TData>
    {
        TData Data { get; }
        bool Uploaded { get; }
        bool Cached { get; }
        Exception Error { get; }
    }
}
