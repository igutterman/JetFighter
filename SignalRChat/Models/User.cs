namespace SignalRChat.Models
{
    public class User
    {

        public string ConnectionId { get; set; }


        public User(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}
