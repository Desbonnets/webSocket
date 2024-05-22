internal class Message
{
    private string message;
    private string sender;
    private DateTime timestamp;

    public Message(string message, string sender, DateTime timestamp)
    {
        this.message = message;
        this.sender = sender;
        this.timestamp = timestamp;
    }

    public string getMessage()
    {
        return message;
    }
    public string getSender()
    {
        return sender;
    }
    public DateTime getTimestamp()
    {
        return timestamp;
    }
}