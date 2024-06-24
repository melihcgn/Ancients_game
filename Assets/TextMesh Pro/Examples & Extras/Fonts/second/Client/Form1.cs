using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace deneme_cs408project
{
    public partial class ClientMain : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;
        private List<Player> currentPlayers = new List<Player>();
        private bool gameStarted = false;

        public ClientMain()
        {
            InitializeComponent();
            client = new TcpClient();
        }

        private void ClientMain_Load(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (client == null)
                {
                    client = new TcpClient();
                }

                string serverIP = clientIPtextbox.Text;
                int serverPort;

                if (!int.TryParse(clientPort.Text, out serverPort))
                {
                    MessageBox.Show("Invalid port number.");
                    return;
                }

                if (!client.Connected)
                {
                    client.Connect(serverIP, serverPort);
                }

                if (client.Connected)
                {
                    stream = client.GetStream();
                    clientRTB.AppendText("Connected to server.\n");

                    string username = playerName.Text;
                    byte[] data = Encoding.ASCII.GetBytes(username);
                    stream.Write(data, 0, data.Length);
                    clientRTB.AppendText("Username sent: " + username + "\n");

                    clientConnectBtn.Enabled = false;
                    disconnectBtn.Enabled = true;

                    receiveThread = new Thread(new ThreadStart(ReceiveMessages));
                    receiveThread.IsBackground = true;
                    receiveThread.Start();
                }
                else
                {
                    MessageBox.Show("Failed to connect to the server.");
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show("SocketException: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server: " + ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e) { }

        private void clientPort_TextChanged(object sender, EventArgs e) { }

        private void playerName_TextChanged(object sender, EventArgs e) { }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (client != null && client.Connected)
            {
                try
                {
                    stream.Close();
                    client.Close();

                    if (receiveThread != null && receiveThread.IsAlive)
                    {
                        if (!receiveThread.Join(1000))
                        {
                            receiveThread.Abort();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during closing: " + ex.Message);
                }
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytes;
                while (client.Connected && stream != null && (bytes = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytes);
                    if (message.StartsWith("Players:"))
                    {
                        string playersData = message.Substring(8);
                        string[] players = playersData.Split(',');
                        Invoke((MethodInvoker)delegate
                        {
                            playerListBox.Items.Clear();
                            foreach (var player in players)
                            {
                                playerListBox.Items.Add(player);
                            }
                        });
                    }
                    else if (message.StartsWith("Countdown:"))
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            clientRTB.AppendText("Server: Game starts in " + message.Substring(10) + " seconds\n");
                        });
                    }
                    else if (message.StartsWith("GameStart"))
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            clientRTB.AppendText("Server: Game started!\n");
                            gameStarted = true;
                        });
                    }
                    else if (message.StartsWith("Result:"))
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            clientRTB.AppendText("Server: " + message.Substring(7) + "\n");
                        });
                    }
                    else if (message.StartsWith("Leaderboard:"))
                    {
                        string leaderboardData = message.Substring(12);
                        List<Player> leaderboard = JsonConvert.DeserializeObject<List<Player>>(leaderboardData);
                        Invoke((MethodInvoker)delegate
                        {
                            leaderboardListBox.Items.Clear();
                            foreach (var player in leaderboard)
                            {
                                leaderboardListBox.Items.Add(player.Name + " - Wins: " + player.Wins);
                            }
                        });
                    }

                    else if (message.StartsWith("A player with this name is already connected."))
                    {
                        MessageBox.Show("Your name is already exists.");
                        client = null;
                        clientConnectBtn.Enabled = true;
                        disconnectBtn.Enabled = false;
                    }
                    else
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            clientRTB.AppendText("Server: " + message + "\n");
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.IsHandleCreated)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        //clientRTB.AppendText("Disconnected from server: " + ex.Message + "\n");
                    });
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (client != null && client.Connected)
            {
                try
                {
                    stream.Close();
                    client.Close();
                    playerListBox.Items.Clear();
                    stream = null;
                    client = null;

                    clientConnectBtn.Enabled = true;
                    disconnectBtn.Enabled = false;
                    clientRTB.AppendText("Disconnected from the server.\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error disconnecting: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("You are not connected to the server.");
                client = null;
                clientConnectBtn.Enabled = true;
                disconnectBtn.Enabled = false;
            }
        }

        private void btnRock_Click(object sender, EventArgs e)
        {
            SendChoice("ROCK");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendChoice("PAPER");
        }

        private void btnScissors_Click(object sender, EventArgs e)
        {
            SendChoice("SCISSORS");
        }

        private void SendChoice(string choice)
        {
            if (client.Connected && gameStarted)
            {
                byte[] data = Encoding.ASCII.GetBytes("G:" + choice);
                stream.Write(data, 0, data.Length);
                clientRTB.AppendText("Choice sent: " + choice + "\n");
            }
            else
            {
                MessageBox.Show("You must be connected and the game must be started before sending a choice.");
            }
        }

        private void UpdatePlayerListBox()
        {
            playerListBox.Items.Clear();
            foreach (Player player in currentPlayers)
            {
                string playerInfo = $"{player.Name} (Wins: {player.Wins})";
                playerListBox.Items.Add(playerInfo);
            }
            playerListBox.Refresh();
        }

        private void HandleReceivedPlayerList(string playerListJson)
        {
            try
            {
                List<Player> receivedPlayers = JsonConvert.DeserializeObject<List<Player>>(playerListJson);
                currentPlayers = receivedPlayers;
                UpdatePlayerListBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error handling received player list: {ex.Message}");
            }
        }

        private void SimulateReceivedPlayerList()
        {
            string sampleJson = "[{\"Name\":\"Player1\",\"Wins\":5},{\"Name\":\"Player2\",\"Wins\":3}]";
            HandleReceivedPlayerList(sampleJson);
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            SimulateReceivedPlayerList();
        }

        private void LeaveTheGameBtn_Click(object sender, EventArgs e)
        {
            if (client != null && client.Connected)
            {
                byte[] data = Encoding.ASCII.GetBytes("LEAVE");
                stream.Write(data, 0, data.Length);
                clientRTB.AppendText("You have left the game.\n");
            }
            else
            {
                MessageBox.Show("You are not connected to the server.");
            }
        }

        private void RejoinTheGameBtn_Click(object sender, EventArgs e)
        {
            if (client.Connected)
            {
                byte[] data = Encoding.ASCII.GetBytes("REJOIN");
                stream.Write(data, 0, data.Length);
                clientRTB.AppendText("You have rejoined the game.\n");
            }
            else
            {
                MessageBox.Show("You are not connected to the server.");
            }
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public int Wins { get; set; }
    }
}
