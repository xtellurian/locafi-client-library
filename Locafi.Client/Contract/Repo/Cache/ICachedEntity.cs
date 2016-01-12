namespace Locafi.Client.Contract.Repo.Cache
{
    public interface ICachedEntity <T>
    {
        string Id { get; set; }
        T Entity { get; set; }
        string Extra { get; set; }
        
    }
}
