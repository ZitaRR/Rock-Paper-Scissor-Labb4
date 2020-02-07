using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Labb4ClassLibrary
{
    internal class Game
    {
        private Dictionary<int, Client> players;
        private Stopwatch timer = null;
        private int rounds = 0;

        public Game(Client p1, Client p2)
        {
            players = new Dictionary<int, Client>
            {
                [1] = p1,
                [2] = p2
            };

            players[1].GameData.HasFoundGame = true;
            players[1].GameData.OpponentName = players[2].GameData.PlayerName;

            players[2].GameData.HasFoundGame = true;
            players[2].GameData.OpponentName = players[1].GameData.PlayerName;

            Console.WriteLine($"{players[1].GameData.PlayerName} and {players[2].GameData.PlayerName} matched against each other!");

            while (true)
            {
                if (players.Where(p => p.Value.GameData.ShouldDisconnect).Count() > 0)
                {
                    break;
                }

                players[1].GameData.CanPlay = true;
                players[1].SendData();

                players[2].GameData.CanPlay = true;
                players[2].SendData();

                if (timer == null)
                {
                    timer = new Stopwatch();
                    timer.Start();
                }

                byte[] data = new byte[1024];

                players[1].Socket.Receive(data);
                players[1].GameData = GameData.GetObject(data);

                players[2].Socket.Receive(data);
                players[2].GameData = GameData.GetObject(data);

                var result = Winner(players[1], players[2]);
                var outcome = result.Item1 == null ? $"It was a tie" : $"{result.Item1.GameData.PlayerName} won with {result.Item2}";

                Console.WriteLine($"{players[1].GameData.PlayerName} played {players[1].GameData.SelectedMove.ToString()}.\n" +
                    $"{players[2].GameData.PlayerName} played {players[2].GameData.SelectedMove.ToString()}.\n" +
                    $"{outcome}.");

                if (result.Item1 != null)
                {
                    result.Item1.GameData.PlayerScore++;
                    players.Where(p => p.Value != result.Item1).FirstOrDefault().Value.GameData.OpponentScore++;
                    rounds++;

                    if (rounds >= 3)
                    {
                        timer.Stop();

                        players[1].GameData.ShouldDisconnect = true;
                        players[2].GameData.ShouldDisconnect = true;

                        bool player1Win = players[1].GameData.PlayerScore > players[1].GameData.OpponentScore;
                        bool player2Win = players[2].GameData.PlayerScore > players[2].GameData.OpponentScore;

                        int size = Server.Cosmos.Users.Count();

                        players[1].SaveMatchResult(size + 1, player1Win, timer.Elapsed);
                        players[2].SaveMatchResult(size + 2, player2Win, timer.Elapsed);
                    }
                }

                players[1].SendData();
                players[2].SendData();
            }
        }

        private (Client, string) Winner(Client player1, Client player2)
        {
            string move1 = player1.GameData.SelectedMove.ToString();
            string move2 = player2.GameData.SelectedMove.ToString();

            switch (move1)
            {
                case "Stone":
                    if (move2 == "Scissors")
                        return (player1, move1);
                    else if (move2 == "Paper")
                        return (player2, move2);
                    else return (null, "tied");
                case "Scissors":
                    if (move2 == "Paper")
                        return (player1, move1);
                    else if (move2 == "Stone")
                        return (player2, move2);
                    else return (null, "tied");
                case "Paper":
                    if (move2 == "Stone")
                        return (player1, move1);
                    else if (move2 == "Scissors")
                        return (player2, move2);
                    else return (null, "tied");
                default: return (null, "tied");
            }
        }
    }
}
