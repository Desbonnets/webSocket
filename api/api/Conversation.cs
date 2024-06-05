using Google.Protobuf.WellKnownTypes;
using System.Reflection;

namespace api
{
    public class Conversation(long Id,string Name, DateTime Date)
    {
        public long Id { get; } = Id;
        public string Name { get; set; } = Name;
        public DateTime Date { get; set; } = Date;
        public List<Message> Messages { get; set; } = new List<Message>();

        public string getName()
        {
            return Name;
        }
        public DateTime getDate()
        {
            return Date;
        }

        public void addMessage(Message message)
        {
            this.Messages.Add(message);
        }

        public void removeMessage(Message message)
        {
            this.Messages.Remove(message);
        }

        public List<Message> GetMessages()
        {
            return this.Messages;
        }
    }
}
