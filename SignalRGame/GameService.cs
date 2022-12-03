using Microsoft.AspNetCore.SignalR;
using SignalRGame.Hubs;
using SignalRGame.Clients;
using SignalRGame.GameLogic;
using System.Collections.Concurrent;
using SignalRGame.Models;

namespace SignalRGame
{
    public class GameService
    {

        private readonly IHubContext<ChatHub, IChatClient> _context;

        //maps group name to game instance
        private readonly ConcurrentDictionary<string, Game> _games;

        public GameService(IHubContext<ChatHub, IChatClient> context)
        {
            _games = new ConcurrentDictionary<string, Game>();
            _context = context;
        }

        public int getPlayerCount(string game)
        {
            return _games[game].players.Count;
        }

        public void removePlayer(string game)
        {
        //    _games[game].
        }

        public bool checkGameExists(string gameName)
        {
            if (_games.ContainsKey(gameName))
                return true;
            return false;
        }

        public void createGame(string gameName)
        {
            _games.TryAdd(gameName, new Game());
        }

        public Dictionary<string, List<string>> getGamesList()
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            
            foreach (var kv in _games)
            {
                dict.Add(kv.Key, kv.Value.players.Keys.ToList());
            }

            return dict;
        }

        public bool AddPlayerToGame(string gameName, string playerID)
        {
            Console.WriteLine($"{playerID} wants to join {gameName}");
            Console.WriteLine($"current players: {_games[gameName].GetPlayerCount()}");

            return _games[gameName].AddPlayer(playerID);
        }

        public async void StartGame(string gameName)
        {

            if (!_games.ContainsKey(gameName))
                return;

            //insert more validation logic if needed


            _games[gameName].OnSendState = OnStateChanged;

            async void OnStateChanged(FighterJet[] jets)
            {
                GameState state = new GameState(jets);
                Console.WriteLine($"GameService: game: {gameName}, jet0: {jets[0].X}, {jets[0].Y}");
                await _context.Clients.Groups(gameName).ReceiveGameState(state);
            }

            _games[gameName].Start();
        }

        

        public async Task SayHi()
        {
            await _context.Clients.All.ReceiveMessage("Pepe", "Hi");
        }






    }
}
