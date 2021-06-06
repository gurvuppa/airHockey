/*Gurvir Uppal
 * Mr. T 
 * Air Hockey Game
 * ICS3U 
 * June 3rd 2021
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace airHockey
{
    public partial class Form1 : Form
    {
        //global variables 
        Rectangle player1 = new Rectangle(260, 25, 60, 60);
        Rectangle player2 = new Rectangle(260, 780, 60, 60);
        Rectangle puck = new Rectangle(281, 420, 30, 30);
        Rectangle player1Goal = new Rectangle(159, 15, 265, 2);
        Rectangle player2Goal = new Rectangle(159, 843, 265, 2);

        int player1Score = 0;
        int player2Score = 0;

        int PLAYER_SPEED = 6;
        int puckXSpeed = 6;
        int puckYSpeed = 6;

        int lastP1X;
        int lastP1Y;
        int lastP2X;
        int lastP2Y;

        int counter = 0;
        int speedCounter = 0;

        bool wDown = false;
        bool aDown = false;
        bool sDown = false;
        bool dDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool leftArrowDown = false;
        bool rightArrowDown = false;

        bool touch = false;

        string gameState = "waiting";

        string soundState = "off";

        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        Pen whitePen = new Pen(Color.YellowGreen, 1);

        SoundPlayer musicPlayer;
        SoundPlayer goalPlayer;
        SoundPlayer whistlePlayer;

        Random RandGen = new Random();

        public Form1()
        {
            InitializeComponent();
            touch = true;

            musicPlayer = new SoundPlayer(Properties.Resources.gameMusic);
            musicPlayer.Play();
        }

        public void GameInitialize()
        {
            if (soundState == "on")
            {
                goalPlayer.Stop();
            }

            titleLabel.Text = "";
            subTitleLabel.Text = "";

            gameTimer.Enabled = true;
            gameState = "running";
            player1Score = 0;
            player2Score = 0;
            p1Score.Text = $"{player1Score}";
            p2Score.Text = $"{player2Score}";

            Rectangle player1 = new Rectangle(260, 25, 60, 60);
            Rectangle player2 = new Rectangle(260, 780, 60, 60);
            Rectangle puck = new Rectangle(281, 420, 30, 30);
            Rectangle player1Goal = new Rectangle(159, 15, 265, 2);
            Rectangle player2Goal = new Rectangle(159, 843, 265, 2);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
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
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {

            //Make game boarder
            Rectangle boarder = new Rectangle(15, 15, this.Width - 30, this.Height - 30);

            //last move
            int p1X = player1.X;
            int p1Y = player1.Y;
            int p2X = player2.X;
            int p2Y = player2.Y;
            int puckX = puck.X;
            int puckY = puck.Y;

            //collision box for paddels
            Rectangle p1Top = new Rectangle(p1X, p1Y, 60, 1);
            Rectangle p1Bot = new Rectangle(p1X, p1Y + 60, 60, 1);
            Rectangle p1Right = new Rectangle(p1X + 60, p1Y, 1, 60);
            Rectangle p1Left = new Rectangle(p1X, p1Y, 1, 60);

            Rectangle p2Top = new Rectangle(p2X, p2Y, 60, 1);
            Rectangle p2Bot = new Rectangle(p2X, p2Y + 60, 60, 1);
            Rectangle p2Right = new Rectangle(p2X + 60, p2Y, 1, 60);
            Rectangle p2Left = new Rectangle(p2X, p2Y, 1, 60);

            //move player
            movePlayer();

            //move puck
            movePuck(p1Top, p1Bot, p1Right, p1Left, p2Top, p2Bot, p2Right, p2Left);

            //puck collision with top and bottom walls
            puckCollisionWalls();

            //puck collision with player1
            puckCollisionPaddle(p1Top, p1Bot, p1Right, p1Left, p2Top, p2Bot, p2Right, p2Left, p1X, p1Y, p2X, p2Y, puckX, puckY);

            //how long the puck speed boost will last
            randomSpeed();

            //prevent puck getting stuck on paddle
            puckStuckOnPaddle(p1Top, p1Bot, p1Right, p1Left, p2Top, p2Bot, p2Right, p2Left);

            //prevent puck getting stuck on corner of p1
            /* An Attempt was made to fix the issue of the ball getting stuck on the corner
             * 
            if (player1.X == lastP1X && player1.Y == lastP1Y)
            {
                if (p1Top.IntersectsWith(puck) && p1Right.IntersectsWith(puck))
                {
                    player1.Y++;

                    debugLabel.Text = "top Right hit";
                }
                else if (p1Top.IntersectsWith(puck) && p1Left.IntersectsWith(puck))
                {
                    player1.Y++;

                    debugLabel.Text = "top Left hit";
                }
                else if (p1Bot.IntersectsWith(puck) && p1Right.IntersectsWith(puck))
                {
                    player1.Y--;

                    debugLabel.Text = "bot Right hit";
                }
                else if (p1Bot.IntersectsWith(puck) && p1Left.IntersectsWith(puck))
                {
                    player1.Y--;

                    debugLabel.Text = "bot Left hit";
                }
            }
            //prevent puck getting stuck on corner of p2

            if (player2.X == lastP2X && player2.Y == lastP2Y)
            {
                if (p2Top.IntersectsWith(puck) && p2Right.IntersectsWith(puck))
                {
                    player2.Y++;
                }
                else if (p2Top.IntersectsWith(puck) && p2Left.IntersectsWith(puck))
                {
                    player2.Y++;
                }
                else if (p2Bot.IntersectsWith(puck) && p2Right.IntersectsWith(puck))
                {
                    player2.Y--;
                }
                else if (p2Bot.IntersectsWith(puck) && p2Left.IntersectsWith(puck))
                {
                    player2.Y--;
                }
            }
            */
            //if there is a goal
            goal();

            //if there is a winner
            winner();

            //lattest movement
            lastP1X = player1.X;
            lastP1Y = player1.Y;
            lastP2X = player2.X;
            lastP2Y = player2.Y;

            //play goal sound till counter reaches 50
            if (soundState == "on")
            {
                counter++;
            }
            if (counter == 50)
            {
                soundState = "off";
                counter = 0;
                goalPlayer.Stop();
                musicPlayer.Play();
                titleLabel.Text = "";
            }

            Refresh();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //only for troubleshoot (visible hit boxes)
            /*
            int p1X = player1.X;
            int p1Y = player1.Y;
            int p2X = player2.X;
            int p2Y = player2.Y;
            int puckX = puck.X;
            int puckY = puck.Y;

            Rectangle boarder = new Rectangle(15, 15, this.Width - 30, this.Height - 30);
            Rectangle p1Top = new Rectangle(p1X, p1Y, 60, 1);
            Rectangle p1Bot = new Rectangle(p1X, p1Y + 60, 60, 1);
            Rectangle p1Right = new Rectangle(p1X + 60, p1Y, 1, 60);
            Rectangle p1Left = new Rectangle(p1X, p1Y, 1, 60);
            Rectangle p2Top = new Rectangle(p2X, p2Y, 60, 1);
            Rectangle p2Bot = new Rectangle(p2X, p2Y + 60, 60, 1);
            Rectangle p2Right = new Rectangle(p2X + 60, p2Y, 1, 60);
            Rectangle p2Left = new Rectangle(p2X, p2Y, 1, 60);

            e.Graphics.DrawRectangle(whitePen, p1Top);
            e.Graphics.DrawRectangle(whitePen, p1Bot);
            e.Graphics.DrawRectangle(whitePen, p1Right);
            e.Graphics.DrawRectangle(whitePen, p1Left);
            e.Graphics.DrawRectangle(whitePen, player1Goal);
            e.Graphics.DrawRectangle(whitePen, player2Goal);
            e.Graphics.DrawRectangle(whitePen, boarder);
            */

            if (gameState == "waiting")
            {
                p1Score.Text = "";
                p2Score.Text = "";
                titleLabel.Text = "AIR HOCKEY";
                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
            }
            else if (gameState == "running")
            {
                //Draw paddles and puck
                e.Graphics.DrawImage(Properties.Resources.bluePaddel, player1);
                e.Graphics.DrawImage(Properties.Resources.redPaddle, player2);
                e.Graphics.FillEllipse(blackBrush, puck);
            }
            else if (gameState == "over")
            {
                subTitleLabel.Text = "\nPress Space Bar to play Again or Escape to Exit";
            }
        }

        private void movePlayer()
        {
            if (wDown == true && player1.Y > 15)
            {
                player1.Y -= PLAYER_SPEED;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height - 15)
            {
                player1.Y += PLAYER_SPEED;
            }

            if (aDown == true && player1.X > 15)
            {
                player1.X -= PLAYER_SPEED;
            }

            if (dDown == true && player1.X < this.Width - player1.Width - 15)
            {
                player1.X += PLAYER_SPEED;
            }
            //move player2
            if (upArrowDown == true && player2.Y > 15)
            {
                player2.Y -= PLAYER_SPEED;
            }

            if (downArrowDown == true && player2.Y < this.Height - player2.Height - 15)
            {
                player2.Y += PLAYER_SPEED;
            }

            if (leftArrowDown == true && player2.X > 15)
            {
                player2.X -= PLAYER_SPEED;
            }

            if (rightArrowDown == true && player2.X < this.Width - player2.Width - 15)
            {
                player2.X += PLAYER_SPEED;
            }
        }
        public void movePuck(Rectangle p1Top, Rectangle p1Bot, Rectangle p1Right, Rectangle p1Left,
                             Rectangle p2Top, Rectangle p2Bot, Rectangle p2Right, Rectangle p2Left)
        {
            //move puck
            if (touch == true)
            {
                if (touch == true)
                {
                    if ((p1Top.IntersectsWith(puck)) || (p1Bot.IntersectsWith(puck)) || (p1Right.IntersectsWith(puck)) || (p1Left.IntersectsWith(puck))
                        || (p2Top.IntersectsWith(puck)) || (p2Bot.IntersectsWith(puck)) || (p2Right.IntersectsWith(puck)) || (p2Left.IntersectsWith(puck)))
                    {
                        puck.X += puckXSpeed;
                        puck.Y += puckYSpeed;
                        touch = false;
                    }
                }
                else
                {
                    puck.X += puckXSpeed;
                    puck.Y += puckYSpeed;
                }
            }
            else
            {
                puck.X += puckXSpeed;
                puck.Y += puckYSpeed;
            }
        }
        public void puckCollisionWalls()
        {
            //puck collision with top and bottom walls
            if (puck.Y <= 15)
            {
                //setting this to 6 instead of *= -1 because it ignors the speed multiplyer
                //and slows the ball down when contact is made and sends it in the opposite direction
                puckYSpeed = 6;
                puck.Y = 16;
            }
            if (puck.Y >= this.Height - puck.Height - 15)
            {
                //setting this to -6 instead of *= -1 because it ignors the speed multiplyer
                //and slows the ball down when contact is made and sends it in the opposite direction
                puckYSpeed = -6;
                puck.Y = this.Height - puck.Height - 16;
            }
            //puck collision with left and right side wall
            if (puck.X <= 15)
            {
                puckXSpeed = 6;
                puck.X = 16;
            }
            if (puck.X >= this.Width - puck.Width - 15)
            {
                puckXSpeed = -6;
                puck.X = this.Width - puck.Width - 16;
            }
        }
        public void puckCollisionPaddle(Rectangle p1Top, Rectangle p1Bot, Rectangle p1Right, Rectangle p1Left,
                                        Rectangle p2Top, Rectangle p2Bot, Rectangle p2Right, Rectangle p2Left,
                                        int p1X, int p1Y, int p2X, int p2Y, int puckX, int puckY)
        {
            int speedChance;

            int variableSpeed = 10;

            if (p1Top.IntersectsWith(puck))
            {
                speedChance = RandGen.Next(0, 10);
                if (speedChance <= 3)
                {
                    puckYSpeed = puckYSpeed * -1 - variableSpeed;   //speed multiplyer
                }
                else
                {
                    puckYSpeed *= -1;
                }

                player1Shift(puckX, puckY, p1X, p1Y);
            }
            else if (p1Bot.IntersectsWith(puck))
            {
                speedChance = RandGen.Next(0, 10);
                if (speedChance <= 3)
                {
                    puckYSpeed = puckYSpeed * -1 + variableSpeed;
                }
                else
                {
                    puckYSpeed *= -1;
                }

                player1Shift(puckX, puckY, p1X, p1Y);
            }
            else if (p1Right.IntersectsWith(puck))
            {
                speedChance = RandGen.Next(0, 10);
                if (speedChance <= 3)
                {
                    puckXSpeed = puckXSpeed * -1 + variableSpeed;
                }
                else
                {
                    puckXSpeed *= -1;
                }

                player1Shift(puckX, puckY, p1X, p1Y);
            }
            else if (p1Left.IntersectsWith(puck))
            {
                speedChance = RandGen.Next(0, 10);
                if (speedChance <= 3)
                {
                    puckXSpeed = puckXSpeed * -1 - variableSpeed;
                }
                else
                {
                    puckXSpeed *= -1;
                }

                player1Shift(puckX, puckY, p1X, p1Y);
            }
            //puck collision with player2
            if (p2Top.IntersectsWith(puck))
            {
                speedChance = RandGen.Next(0, 10);
                if (speedChance <= 3)
                {
                    puckYSpeed = puckYSpeed * -1 - variableSpeed;
                }
                else
                {
                    puckYSpeed *= -1;
                }

                player2Shift(puckX, puckY, p2X, p2Y);
            }
            else if (p2Bot.IntersectsWith(puck))
            {
                speedChance = RandGen.Next(0, 10);
                if (speedChance <= 3)
                {
                    puckYSpeed = puckYSpeed * -1 + variableSpeed;
                }
                else
                {
                    puckYSpeed *= -1;
                }

                player2Shift(puckX, puckY, p2X, p2Y);
            }
            else if (p2Right.IntersectsWith(puck))
            {
                speedChance = RandGen.Next(0, 10);
                if (speedChance <= 3)
                {
                    puckXSpeed = puckXSpeed * -1 + variableSpeed;
                }
                else
                {
                    puckXSpeed *= -1;
                }

                player2Shift(puckX, puckY, p2X, p2Y);
            }
            else if (p2Left.IntersectsWith(puck))
            {
                speedChance = RandGen.Next(0, 10);
                if (speedChance <= 3)
                {
                    puckXSpeed = puckXSpeed * -1 - variableSpeed;
                }
                else
                {
                    puckXSpeed *= -1;
                }

                player2Shift(puckX, puckY, p2X, p2Y);
            }
        }
        public void puckStuckOnPaddle(Rectangle p1Top, Rectangle p1Bot, Rectangle p1Right, Rectangle p1Left,
                                      Rectangle p2Top, Rectangle p2Bot, Rectangle p2Right, Rectangle p2Left)
        {
            if (player1.X == lastP1X && player1.Y == lastP1Y)
            {
                if (p1Top.IntersectsWith(puck))
                {
                    player1.Y++;
                }
                else if (p1Bot.IntersectsWith(puck))
                {
                    player1.Y--;
                }
                else if (p1Right.IntersectsWith(puck))
                {
                    player1.X--;
                }
                else if (p1Left.IntersectsWith(puck))
                {
                    player1.X++;
                }
            }
            //prevent puck getting stuck in p2
            if (player2.X == lastP2X && player2.Y == lastP2Y)
            {
                if (p2Top.IntersectsWith(puck))
                {
                    player2.Y++;
                }
                else if (p2Bot.IntersectsWith(puck))
                {
                    player2.Y--;
                }
                else if (p2Right.IntersectsWith(puck))
                {
                    player2.X--;
                }
                else if (p2Left.IntersectsWith(puck))
                {
                    player2.X++;
                }
            }
        }
        public void goal()
        {
            if (player1Goal.IntersectsWith(puck))
            {
                player2Score++;
                p2Score.Text = $"{player2Score}";
                titleLabel.Text = "PLAYER 1 GOAL!!";

                goalPlayer = new SoundPlayer(Properties.Resources.Toronto_Maple_Leafs_2020_Goal_Horn);
                goalPlayer.Play();
                soundState = "on";

                resetPosition();
            }
            else if (puck.IntersectsWith(player2Goal))
            {
                player1Score++;
                p1Score.Text = $"{player1Score}";
                titleLabel.Text = "PLAYER 2 GOAL!!";

                goalPlayer = new SoundPlayer(Properties.Resources.Toronto_Maple_Leafs_2020_Goal_Horn);
                goalPlayer.Play();
                soundState = "on";

                resetPosition();
            }
        }
        public void winner()
        {
            //game end
            if (player1Score == 3)
            {
                gameTimer.Enabled = false;
                titleLabel.Visible = true;
                titleLabel.Text = $"Player 1 Wins!!\nThe Score was {player1Score} to {player2Score}";
                gameState = "over";
            }
            else if (player2Score == 3)
            {
                gameTimer.Enabled = false;
                titleLabel.Visible = true;
                titleLabel.Text = $"Player 2 Wins!!\nThe Score was {player2Score} to {player1Score}";
                gameState = "over";
            }
        }
        public void resetPosition()
        {
            puck.X = 281;
            puck.Y = 420;

            player1.X = 260;
            player1.Y = 25;
            player2.X = 260;
            player2.Y = 780;

            touch = true;
        }
        public void player1Shift(int puckX, int puckY, int p1X, int p1Y)
        {
            puck.X = puckX;
            puck.Y = puckY;

            player1.X = p1X;
            player1.Y = p1Y;
        }
        public void player2Shift(int puckX, int puckY, int p2X, int p2Y)
        {
            puck.X = puckX;
            puck.Y = puckY;

            player2.X = p2X;
            player2.Y = p2Y;
        }
        public void randomSpeed()
        {
            if (puckXSpeed > 6 || puckXSpeed < -6 || puckYSpeed > 6 || puckYSpeed < -6)
            {
                speedCounter++;
            }
            if (speedCounter == 40 && puckXSpeed > 6)
            {
                puckXSpeed = 6;
                speedCounter = 0;
            }
            else if (speedCounter == 40 && puckXSpeed < -6)
            {
                puckXSpeed = -6;
                speedCounter = 0;
            }
            else if (speedCounter == 40 && puckYSpeed > 6)
            {
                puckYSpeed = -6;
                speedCounter = 0;
            }
            else if (speedCounter == 40 && puckYSpeed < -6)
            {
                puckYSpeed = 6;
                speedCounter = 0;
            }
        }
    }
}


