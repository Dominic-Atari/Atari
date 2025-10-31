using System;
public class Publisher
{
    public string PublisherName { get; set; } = "TMCO";

    // Define the event using EventHandler<T>
    public event EventHandler<MessageEventArgs>? MyEvent;
    // Method to raise the event
    protected virtual void StartEvent(MessageEventArgs Message)
    {
        MyEvent?.Invoke(this, Message);
    }
    public void DoSomething(string message, Publisher publisher)
    {
        // Create MessageEventArgs instance
        var args = new MessageEventArgs("Prepare for a meeting: " + message +" "+ publisher.PublisherName);
        StartEvent(args);
    }
}
