using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Space_Race_Game
{
    public partial class Form1 : Form
    {
        bool wDown = false;
        bool sDown = false;
        bool upArrow = false;
        bool downArrow = false;

        string gameState = "waiting";

        int player1X = 150;
        int player1Y = 575;
        int player2Y = 575;
        int player2X = 650;

        int playerWidth = 15;
        int playerHeight = 25;

        int playerSpeed = 6;

        List<int> leftObstacleX = new List<int>();
        List<int> leftObstacleY = new List<int>();
        List<int> rightObstacleX = new List<int>();
        List<int> rightObstacleY = new List<int>();

        int obstacleWidth = 35;
        int obstacleHeight = 10;
        int obstacleSpeed = 9;
        int obstacleCounter = 0;

        int countDown = 800;

        int countX = 400;
        int countY = 0;
        int countWidth = 5;
        int countHeight = 800;

        int player1Score = 0;
        int player2Score = 0;

        string winner;

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush player1brush = new SolidBrush(Color.LightCoral);
        SolidBrush player2brush = new SolidBrush(Color.CornflowerBlue);

        Random ranGen = new Random();

        public Form1()
        {
            InitializeComponent();

            titleLabel.Text = "SPACE RACE";
            subTitleLabel.Text = "Press Space to Start or Esc to Exit";
            p1scoreLabel.Hide();
            p2scoreLabel.Hide();

            gameState = "waiting";
        }

        public void GameInitialize()
        {
            p1scoreLabel.Show();
            p2scoreLabel.Show();
            titleLabel.Text = "";
            subTitleLabel.Text = "";
            titleLabel.BackColor = Color.Transparent;
            subTitleLabel.BackColor = Color.Transparent;

            gameTimer.Enabled = true;
            gameState = "running";
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "running")
            {
                e.Graphics.FillRectangle(player1brush, player1X, player1Y, playerWidth, playerHeight);
                e.Graphics.FillRectangle(player2brush, player2X, player2Y, playerWidth, playerHeight);

                for(int i = 0; i <countDown; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, countX, countY, countWidth, countHeight - countDown);
                }
                for (int i = 0; i < leftObstacleX.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, leftObstacleX[i], leftObstacleY[i], obstacleWidth, obstacleHeight);
                }
                for (int i = 0; i < rightObstacleX.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, rightObstacleX[i], rightObstacleY[i], obstacleWidth, obstacleHeight);
                }
            }
            else if (gameState == "over")
            {
                if(player1Score > player2Score)
                {
                    winner = "Player1 Wins";
                }
                else if(player2Score > player1Score)
                {
                    winner = "Player2 Wins";
                }
                else if(player2Score == player1Score)
                {
                    winner = "Tie";
                }

                titleLabel.Text = "GAME OVER";
                subTitleLabel.Text = $"P1 = {player1Score}";
                subTitleLabel.Text += $"\nP2 = {player2Score}";
                subTitleLabel.Text += $"\n\n = {winner}";

                gameTimer.Enabled = false;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upArrow = true;
                    break;
                case Keys.Down:
                    downArrow = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrow = false;
                    break;
                case Keys.Down:
                    downArrow = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            countDown--;
            obstacleCounter++;

            if(countDown == 0)
            {
                gameState = "over";
            }
            if(obstacleCounter == 10)
            {
                leftObstacleY.Add(ranGen.Next(10, this.Height - 50));
                leftObstacleX.Add(10);

                rightObstacleY.Add(ranGen.Next(10, this.Height - 50));
                rightObstacleX.Add(this.Width - obstacleWidth);

                obstacleCounter = 0;
            }

            if (wDown == true && player1Y > 0)
            {
                player1Y -= playerSpeed;
            }
            if (sDown == true && player1Y < this.Height - playerHeight)
            {
                player1Y += playerSpeed;
            }
            if (upArrow == true && player2Y > 0)
            {
                player2Y -= playerSpeed;
            }
            if (downArrow == true && player2Y < this.Height - playerHeight)
            {
                player2Y += playerSpeed;
            }

            for (int i = 0; i < leftObstacleX.Count; i++)
            {
                leftObstacleX[i] += obstacleSpeed;
            }
            for (int i = 0; i < rightObstacleX.Count; i++)
            {
                rightObstacleX[i] -= obstacleSpeed;
            }
            for (int i = 0; i < leftObstacleX.Count; i++)
            {
                if (leftObstacleX[i] > this.Width)
                {
                    leftObstacleX.RemoveAt(i);
                    leftObstacleY.RemoveAt(i);
                }
            }
            for (int i = 0; i < rightObstacleX.Count; i++)
            {
                if (rightObstacleX[i] > this.Width)
                {
                    rightObstacleX.RemoveAt(i);
                    rightObstacleY.RemoveAt(i);
                }
            }

            //Collision
            Rectangle player1Box = new Rectangle(player1X, player1Y, playerWidth, playerHeight);
            Rectangle player2Box = new Rectangle(player2X, player2Y, playerWidth, playerHeight);
            for (int i = 0; i < leftObstacleX.Count; i++)
            {
                Rectangle leftObstacleBox = new Rectangle(leftObstacleX[i], leftObstacleY[i], obstacleWidth, obstacleHeight);

                if (player1Box.IntersectsWith(leftObstacleBox))
                {
                     player1X = 150;
                     player1Y = 575;
                }
                if (player2Box.IntersectsWith(leftObstacleBox))
                {
                    player2X = 650;
                    player2Y = 575;
                }
            }
            for (int i = 0; i < rightObstacleX.Count; i++)
            {
                Rectangle rightObstacleBox = new Rectangle(rightObstacleX[i], rightObstacleY[i], obstacleWidth, obstacleHeight);
                if (player1Box.IntersectsWith(rightObstacleBox))
                {
                    player1X = 150;
                    player1Y = 575;
                }
                if (player2Box.IntersectsWith(rightObstacleBox))
                {
                    player2X = 650;
                    player2Y = 575;
                }
            }
            if(player1Y <= 0)
            {
                player1Score++;
                p1scoreLabel.Text = $"{player1Score}";

                player1X = 150;
                player1Y = 575;
            }
            if(player2Y <= 0)
            {
                player2Score++;
                p2scoreLabel.Text = $"{player2Score}";

                player2X = 650;
                player2Y = 575;
            }

            Refresh();
        }
    }
}
