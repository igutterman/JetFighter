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



        public override async Task OnConnectedAsync()
        {

            //string name = Context.User.Identity.Name;
            //_connections.Add(name, Context.ConnectionId);


            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //string name = Context.User.Identity.Name;

            //_connections.Remove(name, Context.ConnectionId);

            string id = Context.ConnectionId;

            Console.WriteLine($"{id} disconnected");

            Console.WriteLine($"Removed {id} from room: {RemoveFromRoom(id)}");



            await base.OnDisconnectedAsync(exception);

        }

        public string RemoveFromRoom(string connectionID)
        {

            foreach (var kv in rooms)
            {
                if (kv.Value.Contains(connectionID))
                {
                    kv.Value.Remove(connectionID);
                    string roomName = kv.Key;

                    Console.WriteLine($"count: {kv.Value.Count}");
                    foreach (string id in kv.Value)
                    {
                        Console.WriteLine(id);
                    }

                    if (kv.Value.Count == 0)
                    {
                        Console.WriteLine("Room is empty");
                        rooms.Remove(kv.Key, out _);
                        Clients.All.RemoveRoom(roomName);
                    }
                        


                    return roomName;
                }
            }
            return null;

        }




        public async Task SendMessage(string user, string message)
          => await Clients.All.ReceiveMessage(user, message);
        

        public async Task SendMessageToGroup(string user, string message, string group)
          => await Clients.Group(group).ReceiveGroupMessage(user, message, group);


        public async Task CreateRoom(string name)
        {


            if (rooms.ContainsKey(name)) return;
            
            //Creating a room does not add the room creator to it, since connection will be in a new client.
            //rooms.TryAdd(name, new List<string> { Context.ConnectionId });

            rooms.TryAdd(name, new List<string>());
            games.TryAdd(name, new Game());

            await Clients.All.AddRoom(name);
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

        public async Task PassRoomsList()
        {

            await Clients.Caller.ReceiveRoomsList(rooms);

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


        public async Task SendTurn(string group, char c, int row, int col)
        {
            await Clients.Group(group).ReceiveTurn(c, row, col);
        }

        public async Task SendWin(string group, char c)
        {
            await Clients.Group(group).ReceiveWin(c);

        }

        //public async Task SendJetFighter()
        //{
        //    FighterJet jet = new FighterJet(500, 500, 0.5F);

        //    jet.Bullets.Add(new Bullet(501, 501, 0.5F));
        //    jet.Bullets.Add(new Bullet(502, 502, 0.4F));

        //    await Clients.Caller.ReceiveJetFighter(jet);
        //

        public async Task SendGameState(string group)
        {

            GameState state = new GameState
            {
                Jets = games[group].GetJets()
            };


            await Clients.Group(group).ReceiveGameState(state);
        }

        public async Task SendDummyState()
        {
            GameState state = new GameState();
            await Clients.Caller.ReceiveGameState(state.GenerateDummyState());
        }





        //public async Task SendNotification(Notification notification, string group)
        //{

        //}



    }
}
