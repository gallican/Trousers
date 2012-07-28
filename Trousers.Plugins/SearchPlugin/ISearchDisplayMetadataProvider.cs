namespace Trousers.Plugins.SearchPlugin
{
    public interface ISearchDisplayMetadataProvider
    {
        string[] DisplayFields { get; }
        string[] LongFields { get; }
    }
}