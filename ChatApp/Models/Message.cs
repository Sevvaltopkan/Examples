using System;

namespace ChatApp.Models
{
    public class Message : BaseEntity
    {
        private string _content;
        private Guid _senderId;
        private Guid _receiverId;

        public Message(Guid senderId, Guid receiverId, string content)
            : base()
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
        }

        public string Content
        {
            get { return _content; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _content = "(boş mesaj)";
                else
                    _content = value.Trim();
            }
        }

        public Guid SenderId
        {
            get { return _senderId; }
            set
            {
                // relationship: kullanıcıya referans
                if (value == Guid.Empty)
                    _senderId = Guid.NewGuid(); // fallback ama normalde boş olmasını istemiyoruz
                else
                    _senderId = value;
            }
        }

        public Guid ReceiverId
        {
            get { return _receiverId; }
            set
            {
                if (value == Guid.Empty)
                    _receiverId = Guid.NewGuid();
                else
                    _receiverId = value;
            }
        }

        public override string ToString()
        {
            return $"[{Id}] {SenderId} -> {ReceiverId}: {Content}";
        }
    }
}
