using System;

// Custom EventArgs class to hold message data
public class MessageEventArgs : EventArgs
{
    public string Message { get; set; }
    public MessageEventArgs(string message)
    {
        Message = message;
    }
}
