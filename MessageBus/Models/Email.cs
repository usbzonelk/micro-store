using System.ComponentModel.DataAnnotations;

namespace MessageBus.Models
{
    public class Email
    {
        [Required]
        public string Subject
        {
            get;
            set;
        }
        [Required]
        public string Body
        {
            get;
            set;
        }
        [Required]
        public List<string> Recipients
        {
            get;
            set;
        }
    }
}