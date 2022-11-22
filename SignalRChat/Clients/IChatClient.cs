using SignalRChat.Models;
using System.Collections.Concurrent;

namespace SignalRChat.Clients
{
    public interface IChatClient
    {

        Task ReceiveMessage(string user, string message);

        Task ReceiveGroupMessage(string user, string message, string group);

        Task AddRoom(string roomName);

        Task ReceiveRoomsList(ConcurrentDictionary<string, List<string>> roomsList);


        Task RemoveRoom(string roomName);

        //needed?
        Task OnNotificationReceived(Notification notification);

        Task TableClicked(string roomName, string userID);
    }
}
