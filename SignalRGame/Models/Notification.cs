namespace SignalRGame.Models
{
    public class Notification
    {
        public string Payload { get; set; }
        public User From { get; set; }

        public Notification(string payload, User from)
        {
            Payload = payload;
            From = from;
        }
    }
}
