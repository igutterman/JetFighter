using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using SignalRGame.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using SignalRGame.Clients;
using SignalRGame.GameLogic;


namespace SignalRGame.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {

        //private readonly static ConcurrentDictionary<string, List<string>> rooms = new ConcurrentDictionary<string, List<string>>();

        //private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        private readonly static ConcurrentDictionary<string, Game> games = new ConcurrentDictionary<string, Game>();

        //private readonly static IHubContext<ChatHub, IChatClient> _context;

        private readonly GameService _gameService;

        public ChatHub(GameService gameService)
        {
            _gameService = gameService;
        }



        public override async Task OnConnectedAsync()
        {


            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            string id = Context.ConnectionId;

            await _gameService.removePlayer(id);


            await base.OnDisconnectedAsync(exception);

        }



        public async Task SendMessage(string user, string message)
          => await Clients.All.ReceiveMessage(user, message);


        public async Task SendMessageToGroup(string user, string message, string group)
          => await Clients.Group(group).ReceiveGroupMessage(user, message, group);


        public async Task CreateGame(string gameName)
        {


            //if (rooms.ContainsKey(name)) return;
            if (_gameService.checkGameExists(gameName))
                return;


            _gameService.createGame(gameName);

            //Creating a room does not add the room creator to it,  ///since connection will be in a new client. <- not anymore
            //rooms.TryAdd(name, new List<string> { Context.ConnectionId });

            //rooms.TryAdd(name, new List<string>());
            //games.TryAdd(name, new Game());

            await Clients.All.AddGame(gameName);
            //needs to send new room to rooms list that clients see

            //await Groups.AddToGroupAsync(Context.ConnectionId, name);


        }



        public async Task PassGamesList()
        {

            await Clients.Caller.ReceiveGamesList(_gameService.getGamesList());

        }



        public async Task AddPlayerToGame(string gameName)
        {

            if (!_gameService.checkGameExists(gameName))
            {
                await Clients.Caller.ReceiveAddToGameResponse("No such game", 0, gameName);
                return;
            }

            int playerNum = _gameService.AddPlayerToGame(gameName, Context.ConnectionId);

            //0 indicates failure to join game
            if (playerNum > 0)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, gameName);
            }

            string message = playerNum > 0 ? $"You joined {gameName}" : $"Failed to join {gameName}, full";


            await Clients.Caller.ReceiveAddToGameResponse(message, playerNum, gameName);


        }


        public void StartGame(string gameName)
        {

            _gameService.StartGame(gameName);


        }

        public void TurnLeft(string gameName)
        {
            _gameService.TurnLeft(Context.ConnectionId, gameName);
        }

        public void TurnRight(string gameName)
        {
            _gameService.TurnRight(Context.ConnectionId, gameName);
        }

        public void Shoot(string gameName)
        {
            _gameService.Shoot(Context.ConnectionId, gameName);
        }

        public void SetJetSpeed(float value)
        {
            _gameService.SetJetSpeed(value);
        }

        public void GetJetSpeed()
        {
            _gameService.GetJetSpeed();
        }


        public async Task PassSettingsValues()
        {
            var o = _gameService.getOptions();

            await Clients.All.ReceiveSettingsValues(o.gameSpeed, o.jetSpeed, o.bulletSpeed, o.bulletLifetime, o.turnSpeed, o.bulletDelay);
        }

        public void ClientSetSettings(string gameSpeed, string jetSpeed, string bulletSpeed, string bulletLifetime, string turnSpeed, string bulletDelay)
        {

            float GameSpeed = float.Parse(gameSpeed);
            float JetSpeed = float.Parse(jetSpeed);
            float BulletSpeed = float.Parse(bulletSpeed);
            int BulletLifetime = int.Parse(bulletLifetime);
            float TurnSpeed = float.Parse(turnSpeed);
            int BulletDelay = Int32.Parse(bulletDelay);

            Console.WriteLine(gameSpeed);

            GameConfigOptions o = new GameConfigOptions();


            o.gameSpeed = GameSpeed;
            o.jetSpeed = JetSpeed;
            o.bulletSpeed = BulletSpeed;
            o.bulletLifetime = BulletLifetime;
            o.turnSpeed = TurnSpeed;
            o.bulletDelay = BulletDelay;

            _gameService.SetOptions(o);

        }
    }
}

