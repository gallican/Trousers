namespace Trousers.Core.Domain.Entities
{
    public interface IVersionable
    {
        int Revision { get; }
    }
}