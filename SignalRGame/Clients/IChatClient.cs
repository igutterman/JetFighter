using SignalRGame.Models;
using System.Collections.Concurrent;
using SignalRGame.GameLogic;

namespace SignalRGame.Clients
{
    public interface IChatClient
    {

        Task ReceiveMessage(string user, string message);

        Task ReceiveGroupMessage(string user, string message, string group);

        Task ReceiveAddToGameResponse(string response, bool success, string gameName);

        Task AddGame(string gameName);

        //returns dictionary of game names and connected playerIDs
        Task ReceiveGamesList(Dictionary<string, List<string>> gamesList);

        Task NotifyPlayerLeft(string player);


        Task RemoveGame(string gameName);

        Task ReceiveTurn(char c, int row, int col);

        Task ReceiveWin(char c);

        //Task ReceiveJetFighter(FighterJet jet);

        Task ReceiveGameState(GameState state);

        Task AddPlayerToGame(string roomName);

        Task ReceiveSettingsValues(float gameSpeed, float jetSpeed, float bulletSpeed, float bulletLifetime, float turnSpeed, int bulletDelay);
    }
}
