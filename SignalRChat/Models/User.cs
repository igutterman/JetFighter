namespace SignalRChat.Models
{
    public class User
    {


        public string _name { get; set; }

        public IList<string> ConnectionIds = new List<string>();

        public User(string name, string connectionId)
        {
            _name = name;

            ConnectionIds.Add(connectionId);
        }
    }
}
