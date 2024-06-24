using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace deneme_cs408project_server
{
    public partial class Server : Form
    {
        private TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();
        private Dictionary<string, int> playerScores = new Dictionary<string, int>();
        private const int MAX_PLAYERS = 4;
        private bool isListening = false;
        private CancellationTokenSource countdownTokenSource;
        private List<Player> activePlayers = new List<Player>();
        private List<Player> passivePlayers = new List<Player>();

        private Queue<TcpClient> waitingQueue = new Queue<TcpClient>();
        private Dictionary<string, string> playerGestures = new Dictionary<string, string>();
        private bool gameInProgress = false;
        private List<Player> currentRoundPlayers = new List<Player>();
        public Server()
        {
            InitializeComponent();
            LoadInitialScores();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LoadInitialScores()
        {
            try
            {
                foreach (var line in File.ReadAllLines("leaderboard.txt"))
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        playerScores[parts[0].Trim()] = int.Parse(parts[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendTextSafe(richTextBox1, "Error loading initial scores: " + ex.Message + "\n");
            }
            UpdateLeaderboard();
        }

        private void InitializeServer(string portNumberString)
        {
            if (!int.TryParse(portNumberString, out int port))
            {
                MessageBox.Show("Invalid port number.");
                return;
            }

            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                isListening = true;

                server.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
                AppendTextSafe(richTextBox1, $"Server started. Listening for players on {IPAddress.Any}:{port}...\n");
            }
            catch (Exception ex)
            {
                AppendTextSafe(richTextBox1, $"Error starting server: {ex.Message}\n");
            }
        }

        private void OnClientConnect(IAsyncResult ar)
        {
            if (!isListening)
            {
                return;
            }
            TcpClient client = server.EndAcceptTcpClient(ar);
            if (clients.Count >= MAX_PLAYERS)
            {
                waitingQueue.Enqueue(client);
                SendMessage(client, "The room is full, you have been added to the waiting queue.");
            }
            else
            {
                clients.Add(client);
                AppendTextSafe(richTextBox1, "Player connected. Total players: " + clients.Count + "\n");
                Task.Run(() => HandleClient(client));
                CheckPlayersReady();
            }
            server.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
        }

        private void BroadcastPlayerList()
        {
            string message = "Players:" + string.Join(",", activePlayers.Select(p => p.Name));
            foreach (var player in activePlayers)
            {
                SendMessage(player.Client, message);
            }
        }

        private void HandleClient(TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = new byte[1024];
            int bytesRead;
            
            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    var data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (data.StartsWith("G:"))
                    {
                        var gesture = data.Substring(2).Trim().ToUpper();
                        var playerName = GetPlayerName(client);
                        if (gesture == "ROCK" || gesture == "PAPER" || gesture == "SCISSORS")
                        {
                            playerGestures[playerName] = gesture;
                            AppendTextSafe(richTextBox1, $"Player {playerName} chose {gesture}.\n");
                        }
                        else
                        {
                            SendMessage(client, "Invalid gesture. Please enter Rock, Paper, or Scissors.");
                        }
                    }
                    // added afterwards
                    else if (data.Equals("LEAVE", StringComparison.OrdinalIgnoreCase))
                    {
                        var playerName = GetPlayerName(client);
                        var player = activePlayers.FirstOrDefault(p => p.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase));
                        
                        if (player != null)
                        {
                            if (player.HasLeftGame)
                            {
                                SendMessage(client, "You already left from the game.");
                            }
                            else
                            {
                                player.HasLeftGame = true;
                                passivePlayers.Add(player);
                                AppendTextSafe(richTextBox1, $"Player {playerName} has left the game.\n");
                                foreach (var cl in clients)
                                {
                                    SendMessage(cl, $"{playerName} has left the game.\n");
                                }
                                activePlayers.Remove(player);  // Remove the player from activePlayers list
                                BroadcastPlayerList();
                                UpdateLeaderboard();

                            }
                        }
                        else
                        {
                            SendMessage(client, "You already left from the game.");
                        }
                    }

                    else if (data.Equals("REJOIN", StringComparison.OrdinalIgnoreCase))
                    {
                        var playerName = GetPlayerName(client);
                        var player = passivePlayers.FirstOrDefault(p => p.Name.Equals(playerName));

                        if (player != null)
                        {
                            if (!player.HasLeftGame)
                            {
                                SendMessage(client, "You are already in the game.");
                            }
                            else
                            {
                                player.HasLeftGame = false;
                                AppendTextSafe(richTextBox1, $"Player {playerName} has rejoined the game.\n");
                                foreach (var cl in clients)
                                {
                                    SendMessage(cl, $"{playerName} has rejoined the game.\n");
                                }
                                activePlayers.Add(player);
                                passivePlayers.Remove(player);
                                BroadcastPlayerList();
                                UpdateLeaderboard();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Player not found in the passivePlayers list.");
                        }
                    }

                    else
                    {
                        if (!activePlayers.Any(p => p.Name.Equals(data, StringComparison.OrdinalIgnoreCase)))
                        {
                            var newPlayer = new Player(data, client);
                            activePlayers.Add(newPlayer);
                            BroadcastPlayerList();
                        }
                        else
                        {
                            SendMessage(client, "A player with this name is already connected.");
                            return;
                        }

                        if (!playerScores.ContainsKey(data))
                        {
                            playerScores[data] = 0;
                            AppendTextSafe(richTextBox1, $"Player {data} connected.\n");
                            foreach (var cl in clients)
                            {
                                SendMessage(cl, $"{data} joined the room.\n");
                            }
                            UpdateLeaderboard();
                        }
                        else
                        {
                            AppendTextSafe(richTextBox1, $"Player {data} reconnected.\n");
                            foreach (var cl in clients)
                            {
                                SendMessage(cl, $"{data} rejoined the room.\n");
                            }
                            UpdateLeaderboard();
                        }

                        
                    }
                }
            }
            catch (Exception ex)
            {
                AppendTextSafe(richTextBox1, $"Error: {ex.Message}\n");
            }
            finally
            {
                OnClientDisconnect(client);
            }
        }

        private async void CheckPlayersReady()
        {
            if (clients.Count == MAX_PLAYERS)
            {
                foreach (var client in clients)
                {
                    SendMessage(client, "Countdown: 5");
                }

                countdownTokenSource?.Cancel();
                countdownTokenSource = new CancellationTokenSource();

                try
                {
                    await StartCountdown(5, countdownTokenSource.Token);
                    foreach (var client in clients)
                    {
                        SendMessage(client, "GameStart");
                    }
                    StartGame();
                }
                catch (OperationCanceledException)
                {
                    if (clients.Count < MAX_PLAYERS)
                    {
                        foreach (var client in clients)
                        {
                            SendMessage(client, "Countdown aborted. Waiting for more players.");
                        }
                    }
                    while (clients.Count < MAX_PLAYERS && waitingQueue.Count > 0)
                    {
                        var nextClient = waitingQueue.Dequeue();
                        clients.Add(nextClient);
                        SendMessage(nextClient, "You have been added to the game.");
                    }
                    CheckPlayersReady();
                }
            }
        }

        private async Task StartCountdown(int seconds, CancellationToken token)
        {
            for (int i = seconds; i > 0; i--)
            {
                token.ThrowIfCancellationRequested();
                foreach (var client in clients)
                {
                    SendMessage(client, $"Countdown: {i}");
                }
                await Task.Delay(1000, token);
            }
        }

        private void StartGame()
        {
            gameInProgress = true;
            playerGestures.Clear();
            currentRoundPlayers = new List<Player>(activePlayers);
            PromptGestures();
        }

        private void PromptGestures()
        {
            foreach (var player in currentRoundPlayers)
            {
                SendMessage(player.Client, "Game started! Please enter your gesture (Rock, Paper, Scissors) within 10 seconds.");
            }
            Task.Run(async () => await CollectGestures());
        }

        private async Task CollectGestures()
        {
            await Task.Delay(10000); // Wait for 10 seconds
            foreach (var player in currentRoundPlayers.ToList())
            {
                if (!playerGestures.ContainsKey(player.Name))
                {
                    playerGestures[player.Name] = "N"; // No gesture
                    SendMessage(player.Client, "You didn't enter a gesture. You lose this round.");
                }
            }
            EvaluateGameRound();
        }

        private void EvaluateGameRound()
        {
            var validGestures = playerGestures.Where(pg => pg.Value != "N").ToDictionary(pg => pg.Key, pg => pg.Value);
            var gestureCounts = validGestures.Values.GroupBy(g => g).ToDictionary(g => g.Key, g => g.Count());

            if (validGestures.Count == 0)
            {
                foreach (var client in clients)
                {
                    SendMessage(client, "Result: All players failed to choose a gesture. Round tied.");
                }
                playerGestures.Clear();
                PromptGestures();
                return;
            }

            if (validGestures.Count == 1)
            {
                DeclareRoundWinner(validGestures.First().Value);
                return;
            }

            if (gestureCounts.Values.Max() == validGestures.Count) // All same gesture among valid choices
            {
                foreach (var client in clients)
                {
                    SendMessage(client, "Result: All gestures are the same. Round tied.");
                }
                playerGestures.Clear();
                PromptGestures();
            }
            else if (gestureCounts.ContainsKey("ROCK") && gestureCounts["ROCK"] == validGestures.Count - 1 && gestureCounts.ContainsKey("PAPER"))
            {
                DeclareRoundWinner("PAPER");
            }
            else if (gestureCounts.ContainsKey("PAPER") && gestureCounts["PAPER"] == validGestures.Count - 1 && gestureCounts.ContainsKey("SCISSORS"))
            {
                DeclareRoundWinner("SCISSORS");
            }
            else if (gestureCounts.ContainsKey("SCISSORS") && gestureCounts["SCISSORS"] == validGestures.Count - 1 && gestureCounts.ContainsKey("ROCK"))
            {
                DeclareRoundWinner("ROCK");
            }
            else
            {
                var winners = DetermineWinners(validGestures);
                if (winners.Count == 1)
                {
                    DeclareRoundWinner(winners.First());
                }
                else
                {
                    var winnerNames = string.Join(", ", winners);
                    foreach (var client in clients)
                    {
                        SendMessage(client, $"Result: Round tied. Players {winnerNames} with winning gestures move to the next round.");
                    }
                    currentRoundPlayers = currentRoundPlayers.Where(p => winners.Contains(p.Name)).ToList();
                    playerGestures.Clear();
                    PromptGestures();
                }
            }
        }

        private List<string> DetermineWinners(Dictionary<string, string> validGestures)
        {
            var gestureCounts = validGestures.GroupBy(pg => pg.Value).ToDictionary(g => g.Key, g => g.Count());
            var rockCount = gestureCounts.ContainsKey("ROCK") ? gestureCounts["ROCK"] : 0;
            var paperCount = gestureCounts.ContainsKey("PAPER") ? gestureCounts["PAPER"] : 0;
            var scissorsCount = gestureCounts.ContainsKey("SCISSORS") ? gestureCounts["SCISSORS"] : 0;

            if (rockCount == 2 && paperCount == 0 && scissorsCount == 0)
            {
                return validGestures.Where(pg => pg.Value == "ROCK").Select(pg => pg.Key).ToList(); // Rocks move on
            }
            if (rockCount == 2 && paperCount == 1 && scissorsCount == 1)
            {
                return validGestures.Where(pg => pg.Value == "ROCK").Select(pg => pg.Key).ToList(); // Rocks move on
            }
            if (paperCount == 2 && rockCount == 0 && scissorsCount == 0)
            {
                return validGestures.Where(pg => pg.Value == "PAPER").Select(pg => pg.Key).ToList(); // Papers move on
            }
            if (paperCount == 2 && rockCount == 1 && scissorsCount == 1)
            {
                return validGestures.Where(pg => pg.Value == "PAPER").Select(pg => pg.Key).ToList(); // Papers move on
            }
            if (scissorsCount == 2 && rockCount == 0 && paperCount == 0)
            {
                return validGestures.Where(pg => pg.Value == "SCISSORS").Select(pg => pg.Key).ToList(); // Scissors move on
            }
            if (scissorsCount == 2 && rockCount == 1 && paperCount == 1)
            {
                return validGestures.Where(pg => pg.Value == "SCISSORS").Select(pg => pg.Key).ToList(); // Scissors move on
            }
            if (rockCount == 1 && paperCount == 1 && scissorsCount == 1 && validGestures.Count == 3)
            {
                return validGestures.Select(pg => pg.Key).ToList(); // All move on if one of each gesture and no players without gesture
            }
            if (rockCount == paperCount && paperCount == scissorsCount)
            {
                return validGestures.Select(pg => pg.Key).ToList(); // All move to the next round
            }
            if (rockCount > 0 && paperCount > 0 && scissorsCount == 0)
            {
                return validGestures.Where(pg => pg.Value == "PAPER").Select(pg => pg.Key).ToList(); // Papers move on
            }
            if (rockCount > 0 && scissorsCount > 0 && paperCount == 0)
            {
                return validGestures.Where(pg => pg.Value == "ROCK").Select(pg => pg.Key).ToList(); // Rocks move on
            }
            if (paperCount > 0 && scissorsCount > 0 && rockCount == 0)
            {
                return validGestures.Where(pg => pg.Value == "SCISSORS").Select(pg => pg.Key).ToList(); // Scissors move on
            }
            return validGestures.Select(pg => pg.Key).ToList(); // All move to the next round
        }

        private void DeclareRoundWinner(string winningGesture)
        {
            var winners = playerGestures.Where(pg => pg.Value == winningGesture).Select(pg => pg.Key).ToList();
            if (winners.Count == 1)
            {
                var winnerName = winners.First();
                playerScores[winnerName]++;
                AppendTextSafe(richTextBox1, $"Player {winnerName} wins the round.\n");
                foreach (var client in clients)
                {
                    SendMessage(client, $"Result: Player {winnerName} wins the round.");
                }
                UpdateLeaderboard();
                gameInProgress = false;
                if (waitingQueue.Count > 0)
                {
                    var nextClient = waitingQueue.Dequeue();
                    clients.Add(nextClient);
                    SendMessage(nextClient, "You have been added to the game.");
                    CheckPlayersReady();
                }
            }
            else
            {
                var winnerNames = string.Join(", ", winners);
                foreach (var client in clients)
                {
                    SendMessage(client, $"Result: Round tied. Players {winnerNames} with winning gestures move to the next round.");
                }
                currentRoundPlayers = currentRoundPlayers.Where(p => winners.Contains(p.Name)).ToList();
                playerGestures.Clear();
                PromptGestures();
            }
        }

        private void OnClientDisconnect(TcpClient client)
        {
            lock (clients)
            {
                if (clients.Contains(client))
                {
                    clients.Remove(client);
                    Player playerDisc = activePlayers.FirstOrDefault(p => p.Client == client);
                    playerDisc.HasLeftGame = false;
                    AppendTextSafe(richTextBox1, "Player disconnected. Total players: " + clients.Count + "\n");
                    foreach (var it in clients)
                    {
                        SendMessage(it, $"Player {playerDisc.Name} left the room.\n");
                    }
                }
                else if (waitingQueue.Contains(client))
                {
                }
            }
            lock (activePlayers)
            {
                Player playerToRemove = activePlayers.FirstOrDefault(p => p.Client == client);
                activePlayers.Remove(playerToRemove);
                BroadcastPlayerList();
            }
            if (countdownTokenSource != null && clients.Count < MAX_PLAYERS)
            {
                countdownTokenSource.Cancel();
            }
        }

        private void SendMessage(TcpClient client, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            client.GetStream().BeginWrite(data, 0, data.Length, null, null);
        }

        private void UpdateLeaderboard()
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.BeginInvoke(new MethodInvoker(() => UpdateLeaderboard()));
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Player");
                dt.Columns.Add("Wins");
                foreach (var playerScore in playerScores.OrderByDescending(p => p.Value))
                {
                    dt.Rows.Add(playerScore.Key, playerScore.Value);
                }
                dataGridView1.DataSource = dt;

                string leaderboardJson = JsonConvert.SerializeObject(activePlayers.Select(p => new { p.Name, Wins = playerScores[p.Name] }));
                foreach (var client in clients)
                {
                    SendMessage(client, $"Leaderboard: {leaderboardJson}");
                }
            }
        }

        private void AppendTextSafe(RichTextBox rtb, string text)
        {
            if (rtb.InvokeRequired)
            {
                rtb.BeginInvoke(new MethodInvoker(() => AppendTextSafe(rtb, text)));
            }
            else
            {
                rtb.AppendText(text);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (server != null)
            {
                server.Stop();
            }
            foreach (var client in clients)
            {
                client.Close();
            }
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            string portNumber = portTextbox.Text;
            InitializeServer(portNumber);
            listenButton.Enabled = false;
            closeButton.Enabled = true;
        }

        private void DisconnectServer()
        {
            try
            {
                isListening = false;
                server.Stop();
                foreach (var client in clients)
                {
                    client.Close();
                }
                clients.Clear();
                AppendTextSafe(richTextBox1, "Server stopped.\n");
            }
            catch (Exception ex)
            {
                AppendTextSafe(richTextBox1, $"Error stopping server: {ex.Message}\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisconnectServer();
            listenButton.Enabled = true;
            closeButton.Enabled = false;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private string GetPlayerName(TcpClient client)
        {
            var activePlayer = activePlayers.FirstOrDefault(p => p.Client == client);
            var passivePlayer = passivePlayers.FirstOrDefault(p => p.Client == client);

            if (activePlayer != null)
            {
                return activePlayer.Name;
            }
            else if (passivePlayer != null)
            {
                return passivePlayer.Name;
            }
            else
            {
                return null; // Player not found
            }
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public TcpClient Client { get; set; }
        public bool HasLeftGame { get; set; }

        public Player(string name, TcpClient client)
        {
            Name = name;
            Client = client;
            HasLeftGame = false;
        }

    }
}
