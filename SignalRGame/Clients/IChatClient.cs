using SignalRGame.Models;
using System.Collections.Concurrent;
using SignalRGame.GameLogic;

namespace SignalRGame.Clients
{
    public interface IChatClient
    {

        Task ReceiveMessage(string user, string message);

        Task ReceiveGroupMessage(string user, string message, string group);

        Task AddRoom(string roomName);

        Task ReceiveRoomsList(ConcurrentDictionary<string, List<string>> roomsList);


        Task RemoveRoom(string roomName);

        Task ReceiveTurn(char c, int row, int col);

        Task ReceiveWin(char c);

        //Task ReceiveJetFighter(FighterJet jet);

        Task ReceiveGameState(GameState state);

        Task AddPlayerToGame(string roomName);
    }
}
