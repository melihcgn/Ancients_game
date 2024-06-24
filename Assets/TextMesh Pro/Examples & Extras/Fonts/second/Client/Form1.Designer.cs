namespace deneme_cs408project
{
    partial class ClientMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.clientConnectBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.clientIPtextbox = new System.Windows.Forms.TextBox();
            this.clientPort = new System.Windows.Forms.TextBox();
            this.playerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.clientRTB = new System.Windows.Forms.RichTextBox();
            this.disconnectBtn = new System.Windows.Forms.Button();
            this.btnScissors = new System.Windows.Forms.Button();
            this.btnRock = new System.Windows.Forms.Button();
            this.btnPaper = new System.Windows.Forms.Button();
            this.playerListBox = new System.Windows.Forms.ListBox();
            this.gameRoomName = new System.Windows.Forms.Label();
            this.leaderboardListBox = new System.Windows.Forms.ListBox();
            this.leaderboardLabel = new System.Windows.Forms.Label();
            this.LeaveTheGameBtn = new System.Windows.Forms.Button();
            this.RejoinTheGameBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clientConnectBtn
            // 
            this.clientConnectBtn.Location = new System.Drawing.Point(301, 266);
            this.clientConnectBtn.Name = "clientConnectBtn";
            this.clientConnectBtn.Size = new System.Drawing.Size(161, 44);
            this.clientConnectBtn.TabIndex = 0;
            this.clientConnectBtn.Text = "Connect to the game";
            this.clientConnectBtn.UseVisualStyleBackColor = true;
            this.clientConnectBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(254, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP address";
            // 
            // clientIPtextbox
            // 
            this.clientIPtextbox.Location = new System.Drawing.Point(332, 76);
            this.clientIPtextbox.Name = "clientIPtextbox";
            this.clientIPtextbox.Size = new System.Drawing.Size(130, 22);
            this.clientIPtextbox.TabIndex = 2;
            // 
            // clientPort
            // 
            this.clientPort.Location = new System.Drawing.Point(332, 139);
            this.clientPort.Name = "clientPort";
            this.clientPort.Size = new System.Drawing.Size(130, 22);
            this.clientPort.TabIndex = 3;
            this.clientPort.TextChanged += new System.EventHandler(this.clientPort_TextChanged);
            // 
            // playerName
            // 
            this.playerName.Location = new System.Drawing.Point(332, 200);
            this.playerName.Name = "playerName";
            this.playerName.Size = new System.Drawing.Size(130, 22);
            this.playerName.TabIndex = 4;
            this.playerName.TextChanged += new System.EventHandler(this.playerName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Port";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(244, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Player name";
            // 
            // clientRTB
            // 
            this.clientRTB.Location = new System.Drawing.Point(494, 222);
            this.clientRTB.Name = "clientRTB";
            this.clientRTB.Size = new System.Drawing.Size(174, 154);
            this.clientRTB.TabIndex = 7;
            this.clientRTB.Text = "";
            // 
            // disconnectBtn
            // 
            this.disconnectBtn.Enabled = false;
            this.disconnectBtn.Location = new System.Drawing.Point(301, 332);
            this.disconnectBtn.Name = "disconnectBtn";
            this.disconnectBtn.Size = new System.Drawing.Size(161, 44);
            this.disconnectBtn.TabIndex = 8;
            this.disconnectBtn.Text = "Disconnect from the game";
            this.disconnectBtn.UseVisualStyleBackColor = true;
            this.disconnectBtn.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnScissors
            // 
            this.btnScissors.Location = new System.Drawing.Point(620, 401);
            this.btnScissors.Name = "btnScissors";
            this.btnScissors.Size = new System.Drawing.Size(46, 42);
            this.btnScissors.TabIndex = 11;
            this.btnScissors.Text = "S";
            this.btnScissors.UseVisualStyleBackColor = true;
            this.btnScissors.Click += new System.EventHandler(this.btnScissors_Click);
            // 
            // btnRock
            // 
            this.btnRock.Location = new System.Drawing.Point(494, 401);
            this.btnRock.Name = "btnRock";
            this.btnRock.Size = new System.Drawing.Size(46, 42);
            this.btnRock.TabIndex = 12;
            this.btnRock.Text = "R";
            this.btnRock.UseVisualStyleBackColor = true;
            this.btnRock.Click += new System.EventHandler(this.btnRock_Click);
            // 
            // btnPaper
            // 
            this.btnPaper.Location = new System.Drawing.Point(558, 401);
            this.btnPaper.Name = "btnPaper";
            this.btnPaper.Size = new System.Drawing.Size(46, 42);
            this.btnPaper.TabIndex = 13;
            this.btnPaper.Text = "P";
            this.btnPaper.UseVisualStyleBackColor = true;
            this.btnPaper.Click += new System.EventHandler(this.button2_Click);
            // 
            // playerListBox
            // 
            this.playerListBox.FormattingEnabled = true;
            this.playerListBox.ItemHeight = 16;
            this.playerListBox.Location = new System.Drawing.Point(26, 74);
            this.playerListBox.Name = "playerListBox";
            this.playerListBox.Size = new System.Drawing.Size(189, 308);
            this.playerListBox.TabIndex = 14;
            // 
            // gameRoomName
            // 
            this.gameRoomName.AutoSize = true;
            this.gameRoomName.Location = new System.Drawing.Point(70, 45);
            this.gameRoomName.Name = "gameRoomName";
            this.gameRoomName.Size = new System.Drawing.Size(84, 16);
            this.gameRoomName.TabIndex = 15;
            this.gameRoomName.Text = "Game Room";
            // 
            // leaderboardListBox
            // 
            this.leaderboardListBox.FormattingEnabled = true;
            this.leaderboardListBox.ItemHeight = 16;
            this.leaderboardListBox.Location = new System.Drawing.Point(494, 45);
            this.leaderboardListBox.Name = "leaderboardListBox";
            this.leaderboardListBox.Size = new System.Drawing.Size(174, 164);
            this.leaderboardListBox.TabIndex = 16;
            // 
            // leaderboardLabel
            // 
            this.leaderboardLabel.AutoSize = true;
            this.leaderboardLabel.Location = new System.Drawing.Point(524, 18);
            this.leaderboardLabel.Name = "leaderboardLabel";
            this.leaderboardLabel.Size = new System.Drawing.Size(86, 16);
            this.leaderboardLabel.TabIndex = 17;
            this.leaderboardLabel.Text = "Leaderboard";
            // 
            // LeaveTheGameBtn
            // 
            this.LeaveTheGameBtn.Location = new System.Drawing.Point(699, 222);
            this.LeaveTheGameBtn.Name = "LeaveTheGameBtn";
            this.LeaveTheGameBtn.Size = new System.Drawing.Size(127, 25);
            this.LeaveTheGameBtn.TabIndex = 18;
            this.LeaveTheGameBtn.Text = "Leave the game";
            this.LeaveTheGameBtn.UseVisualStyleBackColor = true;
            this.LeaveTheGameBtn.Click += new System.EventHandler(this.LeaveTheGameBtn_Click);
            // 
            // RejoinTheGameBtn
            // 
            this.RejoinTheGameBtn.Location = new System.Drawing.Point(699, 266);
            this.RejoinTheGameBtn.Name = "RejoinTheGameBtn";
            this.RejoinTheGameBtn.Size = new System.Drawing.Size(127, 23);
            this.RejoinTheGameBtn.TabIndex = 19;
            this.RejoinTheGameBtn.Text = "Rejoin the game";
            this.RejoinTheGameBtn.UseVisualStyleBackColor = true;
            this.RejoinTheGameBtn.Click += new System.EventHandler(this.RejoinTheGameBtn_Click);
            // 
            // ClientMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 528);
            this.Controls.Add(this.RejoinTheGameBtn);
            this.Controls.Add(this.LeaveTheGameBtn);
            this.Controls.Add(this.leaderboardLabel);
            this.Controls.Add(this.leaderboardListBox);
            this.Controls.Add(this.gameRoomName);
            this.Controls.Add(this.playerListBox);
            this.Controls.Add(this.btnPaper);
            this.Controls.Add(this.btnRock);
            this.Controls.Add(this.btnScissors);
            this.Controls.Add(this.disconnectBtn);
            this.Controls.Add(this.clientRTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.playerName);
            this.Controls.Add(this.clientPort);
            this.Controls.Add(this.clientIPtextbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clientConnectBtn);
            this.Name = "ClientMain";
            this.Text = "Client";
            this.Load += new System.EventHandler(this.ClientMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button clientConnectBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox clientIPtextbox;
        private System.Windows.Forms.TextBox clientPort;
        private System.Windows.Forms.TextBox playerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox clientRTB;
        private System.Windows.Forms.Button disconnectBtn;
        private System.Windows.Forms.Button btnScissors;
        private System.Windows.Forms.Button btnRock;
        private System.Windows.Forms.Button btnPaper;
        private System.Windows.Forms.ListBox playerListBox;
        private System.Windows.Forms.Label gameRoomName;
        private System.Windows.Forms.ListBox leaderboardListBox;
        private System.Windows.Forms.Label leaderboardLabel;
        private System.Windows.Forms.Button LeaveTheGameBtn;
        private System.Windows.Forms.Button RejoinTheGameBtn;
    }
}
