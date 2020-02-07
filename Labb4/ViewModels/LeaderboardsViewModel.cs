using Labb4.Commands;
using Labb4ClassLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;

namespace Labb4.ViewModels
{
    internal class LeaderboardsViewModel : BaseViewModel
    {
        public SortLeaderboardsCommand SortCommand { get; }

        private List<MatchResult> matches = new List<MatchResult>();
        public List<MatchResult> Matches
        {
            get => matches;
            set
            {
                matches = value;
                OnPropertyChanged(nameof(Matches));
            }
        }

        private int index = 0;
        public int Index
        {
            get => index;
            set
            {
                index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        public List<string> SortTypes { get; } = new List<string>
        {
            "By Names",
            "By Result",
            "By Length",
            "By Date"
        };

        public LeaderboardsViewModel()
        {
            SortCommand = new SortLeaderboardsCommand(this);

            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(Server.IP, Server.PORT);
            socket.Send(new byte[1]);

            byte[] data = new byte[1024];
            socket.Receive(data);
            int size = BitConverter.ToInt32(data, 0);

            for (int i = 0; i < size; i++)
            {
                data = new byte[1024];
                socket.Receive(data);
                var result = MatchResult.GetObject(data);
                Matches.Add(result);
                MessageBox.Show("HELLO");
            }
        }
    }
}
