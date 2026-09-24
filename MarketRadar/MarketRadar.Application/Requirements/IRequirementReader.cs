public interface IRequirementReader<TRequest, TResponse>
{
    TResponse Get(TRequest request);
}