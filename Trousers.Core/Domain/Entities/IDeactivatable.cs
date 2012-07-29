namespace Trousers.Core.Domain.Entities
{
    public interface IDeactivatable
    {
        bool IsCurrent { get; }
    }
}