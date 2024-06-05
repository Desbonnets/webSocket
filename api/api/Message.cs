namespace api
{
    public class Message
    {
        public string message { get; set; }
        public string sender { get; set; }
        public DateTime timestamp { get; set; }

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
}
