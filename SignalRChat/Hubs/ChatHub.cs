using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;


namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {

        public static readonly ConcurrentDictionary<string, int> rooms = new ConcurrentDictionary<string, int>();

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        public async Task CreateRoom(string name)
        {

            foreach (var kv in rooms)
            {
                Console.WriteLine($"room: {kv.Key}");
            }

            if (rooms.ContainsKey(name)) return;
            rooms.TryAdd(name, 1);
            await Clients.All.SendAsync("AddRoom", name);
            //needs to send new room to rooms list that clients see


        }

        public async Task JoinRoom(string name)
        {
            if (!rooms.ContainsKey(name)) return;
            await Groups.AddToGroupAsync(Context.ConnectionId, name);
            rooms[name]++;

            Console.WriteLine($"{Context.ConnectionId.ToString()} joined room: {name} ");

            
        }



    }
}
