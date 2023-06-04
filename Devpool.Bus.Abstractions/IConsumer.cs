namespace Devpool.Bus.Abstractions;

public interface IConsumer<T> where T: IEvent
{ 
    Task Consume(CancellationToken cancellationToken = default);
    void Close();
}