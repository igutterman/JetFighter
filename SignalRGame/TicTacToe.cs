using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRGame.Models;
using SignalRGame.Hubs;

namespace SignalRGame
{
    public class TicTacToe
    {

        public string GameId;
        public char[,] Board { get; set; } = new char[3, 3] { { ' ', ' ', ' ' }, { ' ', ' ', ' ' }, { ' ', ' ', ' ' } };

        public bool isXturn = true;

        public (string, string) playerIDs;

        private readonly string HostDomain = Environment.GetEnvironmentVariable("HOST_DOMAIN");

        private HubConnection _hubConnection;

        public bool test = false;

        public TicTacToe(string gameID)
        {
            GameId = gameID;

            playerIDs = (string.Empty, string.Empty);

        }

        public async Task StartConnectionAsync() =>
            await _hubConnection.StartAsync();


        public async Task SwitchTest()
        {
            test = !test;
            Console.WriteLine("switched test");
        }
            
        

        public void AddPlayer(string playerID)
        {
            if (playerIDs.Item1 == string.Empty)
            {
                playerIDs.Item1 = playerID;
                Console.WriteLine($"Set player 1 to {playerID}");
            }
            else if (playerIDs.Item2 == string.Empty)
            {
                playerIDs.Item2 = playerID;
                Console.WriteLine($"Set player 2 to {playerID}");
            }

            else
            {
                Console.WriteLine("Both player slots filled");
            }
                
            
        }


        public void OnNotificationReceived(Notification notification)
        {
            Console.WriteLine("TTT received notification");
        }

        public bool isValidMove(int player, int row, int col)
        {

            if (player == 1 && !isXturn)
                return false;

            if (player == 2 && isXturn)
                return false;

            if (Board[row, col] != ' ')
                return false;

            return true;

        }

        public void setPlayerid(string id, int player)
        {
            if (player == 1)
                playerIDs.Item1 = id;
            else if (player == 2)
                playerIDs.Item2 = id;
        }



        public int getPlayerNumber(string id)
        {
            if (id == playerIDs.Item1)
                return 1;
            else if (id == playerIDs.Item2)
                return 2;
            else
                return -1;
        }

        public void setCell(int player, int row, int col)
        {
            char c = player == 1 ? 'X' : 'O';

            
            Board[row, col] = c;
            isXturn = !isXturn;
        }

        public char checkWin()
        {
            if (Board[0, 0] == Board[0, 1] && Board[0, 0] == Board[0, 2])
                return Board[0,0];

            if (Board[1, 0] == Board[1, 1] && Board[1, 0] == Board[1, 2])
                return Board[1, 0];

            if (Board[2, 0] == Board[2, 1] && Board[2, 0] == Board[2, 2])
                return Board[2, 0];

            if (Board[0, 0] == Board[1, 0] && Board[0, 0] == Board[2, 0])
                return Board[0, 0];

            if (Board[0, 1] == Board[1, 1] && Board[0, 1] == Board[2, 1])
                return Board[0, 1];

            if (Board[0, 2] == Board[1, 2] && Board[0, 2] == Board[2, 2])
                return Board[0, 2];

            if (Board[0, 0] == Board[1, 1] && Board[0, 0] == Board[2, 2])
                return Board[0, 0];

            if (Board[2, 0] == Board[1, 1] && Board[2, 0] == Board[0, 2])
                return Board[2, 0];

            //if nobody has won
            return 'Q';
        }





        

    }
}
