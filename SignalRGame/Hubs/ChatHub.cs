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

        private readonly static ConcurrentDictionary<string, List<string>> rooms = new ConcurrentDictionary<string, List<string>>();

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

            //string name = Context.User.Identity.Name;
            //_connections.Add(name, Context.ConnectionId);

            //await _gameService.SayHi();


            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //string name = Context.User.Identity.Name;

            //_connections.Remove(name, Context.ConnectionId);

            string id = Context.ConnectionId;

            await _gameService.removePlayer(id);

            //Console.WriteLine($"{id} disconnected");

            //Console.WriteLine($"Removed {id} from room: {RemoveFromRoom(id)}");



            await base.OnDisconnectedAsync(exception);

        }

        //public string RemoveFromRoom(string connectionID)
        //{

        //    foreach (var kv in rooms)
        //    {
        //        if (kv.Value.Contains(connectionID))
        //        {
        //            kv.Value.Remove(connectionID);
        //            string roomName = kv.Key;

        //            Console.WriteLine($"count: {kv.Value.Count}");
        //            foreach (string id in kv.Value)
        //            {
        //                Console.WriteLine(id);
        //            }

        //            if (kv.Value.Count == 0)
        //            {
        //                Console.WriteLine("Room is empty");
        //                rooms.Remove(kv.Key, out _);
        //                Clients.All.RemoveGame(gameName);
        //            }
                        


        //            return roomName;
        //        }
        //    }
        //    return null;

        //}




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

        public async Task JoinRoom(string name)
        {
            if (!rooms.ContainsKey(name)) return;
            //should alert user that room doesn't exist



            await Groups.AddToGroupAsync(Context.ConnectionId, name);
            rooms[name].Add(Context.ConnectionId);

            Console.WriteLine($"{Context.ConnectionId} joined room: {name} ");

            games[name].AddPlayer(Context.ConnectionId);

            
        }

        public async Task PassGamesList()
        {

            await Clients.Caller.ReceiveGamesList(_gameService.getGamesList());

        }

        //Game data section

        //public async Task ReceiveTurn(string group, int row, int col)
        //{

        //    TicTacToe game = games[group];

            

        //    int playerNumber = game.getPlayerNumber(Context.ConnectionId);
        //    if (playerNumber == -1) return;

        //    if (!game.isValidMove(playerNumber, row, col)) return;

        //    game.setCell(playerNumber, row, col);

        //    char c = playerNumber == 1 ? 'X' : 'O';

        //    await SendTurn(group, c, row, col);

        //    if (game.checkWin() != 'Q')
        //        await SendWin(group, c);

        //}


        //public async Task SendTurn(string group, char c, int row, int col)
        //{
        //    await Clients.Group(group).ReceiveTurn(c, row, col);
        //}

        //public async Task SendWin(string group, char c)
        //{
        //    await Clients.Group(group).ReceiveWin(c);

        //}

        //public async Task SendJetFighter()
        //{
        //    FighterJet jet = new FighterJet(500, 500, 0.5F);

        //    jet.Bullets.Add(new Bullet(501, 501, 0.5F));
        //    jet.Bullets.Add(new Bullet(502, 502, 0.4F));

        //    await Clients.Caller.ReceiveJetFighter(jet);
        //

        //public async Task SendGameState(string group, FighterJet[] jets)
        //{

        //    GameState state = new GameState(jets);

        //    //Console.WriteLine(jets[0].ToString());

        //    await Clients.Group(group).ReceiveGameState(state);
        //}

        //delete after testing and remove no-params gamestate constructor
        public async Task SendDummyState()
        {
            GameState state = new GameState();
            await Clients.Caller.ReceiveGameState(state.GenerateDummyState());
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


            //Game game = games[roomName];
            //return game.AddPlayer(Context.ConnectionId);
        }


        public void StartGame(string gameName)
        {

            _gameService.StartGame(gameName);

            //Game game = games[roomName];

            //game.OnSendState = OnStateChanged;

            //async void OnStateChanged(FighterJet[] jets)
            //{
            //    Console.WriteLine($"jet0: {jets[0].X}, {jets[0].Y}");
            //    await SendGameState(roomName, jets);
            //}

            //game.Start();


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






        //public async Task SendNotification(Notification notification, string group)
        //{

        //}



    }
}
