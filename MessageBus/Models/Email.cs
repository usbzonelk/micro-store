namespace MessageBus.Models
{
    public class Email
    {
        public string Subject
        {
            get;
            set;
        }
        public string Body
        {
            get;
            set;
        }
        public List<string> Recipients
        {
            get;
            set;
        }
    }
}