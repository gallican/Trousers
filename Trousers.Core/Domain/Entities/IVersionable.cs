namespace Trousers.Core.Domain.Entities
{
    public interface IVersionable
    {
        long Revision { get; }
    }
}