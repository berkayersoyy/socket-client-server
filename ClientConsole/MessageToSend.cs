using System;

namespace ClientConsole
{
    [Serializable]
    public class MessageToSend
    {
        public string Message { get; set; }
        public Guid ClientId { get; set; }
    }
}