
public class Subscriber
{
    public void SubscribeOnMyEvent(Publisher publisher)
    {
        publisher.MyEvent += OnMyEvent;
    }

    private void OnMyEvent(object? sender, MessageEventArgs e)
    {
        Console.WriteLine($"message received: {e.Message}");
    }
}