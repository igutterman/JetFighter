using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRChat.Models
{
    public sealed class Room : IAsyncDisposable
    {

        public string Name { get; set; }
        //public IList<User> Users { get; set; }

        private bool _disposed;

        private readonly string HostDomain = Environment.GetEnvironmentVariable("HOST_DOMAIN");

        private HubConnection _hubConnection;

        public Room(string name)
        {
            Name = name;
            List<User> Users = new List<User>();


            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri($"{HostDomain}/chatHub"))
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<Notification>(
                "NotificationReceived", OnNotificationReceivedAsync);

        }

        //public void AddUser(User user)
        //{
        //    Users.Add(user); 
        //}


        public async ValueTask DisposeAsync()
        {
            await Task.Yield();
            Console.WriteLine("Cleaned up!");
            _disposed = true;
        }

        private async Task OnNotificationReceivedAsync(Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
