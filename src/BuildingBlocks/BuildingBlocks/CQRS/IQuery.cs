namespace BuildingBlocks.CQRS;

public interface IQuery<TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}