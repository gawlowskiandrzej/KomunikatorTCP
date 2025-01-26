namespace ClientWPF.Models
{
    internal class Message
    {
        public string UserFrom { get; set; }
        public string UserTo{ get; set; }
        public string Content { get; set; }

        public Message(string userFrom, string userTo, string content)
        {
            UserFrom = userFrom;
            UserTo = userTo;
            Content = content;
        }
    }
}
