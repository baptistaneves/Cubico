namespace RealEstate.Domain.Abstractions;

public class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    public IReadOnlyList<IDomainEvent> DomainEvents => throw new NotImplementedException();

    public IDomainEvent[] ClearDomainEvents()
    {
        throw new NotImplementedException();
    }
}