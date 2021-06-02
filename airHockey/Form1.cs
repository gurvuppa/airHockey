using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace airHockey
{
    public partial class Form1 : Form
    {
        //global variables 
        Rectangle player1 = new Rectangle(260, 150, 60, 60);
        Rectangle player2 = new Rectangle(260, 780, 60, 60);
        Rectangle ball = new Rectangle(281, 420, 30, 30);

        int player1Score = 0;
        int player2Score = 0;

        int PLAYER_SPEED = 6ss;
        int ballXSpeed = 6;
        int ballYSpeed = 6;

        int lastP1X;
        int lastP1Y;
        int lastP2X;
        int lastP2Y;

        bool wDown = false;
        bool aDown = false;
        bool sDown = false;
        bool dDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool leftArrowDown = false;
        bool rightArrowDown = false;

        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        Pen whitePen = new Pen(Color.YellowGreen, 1);


        public Form1()
        {
            InitializeComponent();
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
            //last move
            int p1X = player1.X;
            int p1Y = player1.Y;
            int p2X = player2.X;
            int p2Y = player2.Y;
            int puckX = ball.X;
            int puckY = ball.Y;

            //move ball
            ball.X += ballXSpeed;
            ball.Y += ballYSpeed;

            //move player1
            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= PLAYER_SPEED;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += PLAYER_SPEED;
            }

            if (aDown == true && player1.X > 0)
            {
                player1.X -= PLAYER_SPEED;
            }

            if (dDown == true && player1.X < this.Width - player1.Width)
            {
                player1.X += PLAYER_SPEED;
            }

            //move player2
            if (upArrowDown == true && player2.Y > 0)
            {
                player2.Y -= PLAYER_SPEED;
            }

            if (downArrowDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += PLAYER_SPEED;
            }

            if (leftArrowDown == true && player2.X > 0)
            {
                player2.X -= PLAYER_SPEED;
            }

            if (rightArrowDown == true && player2.X < this.Width - player2.Width)
            {
                player2.X += PLAYER_SPEED;
            }

            //ball collision with top and bottom walls
            if (ball.Y < 0 || ball.Y > this.Height - ball.Height)
            {
                ballYSpeed *= -1;  
            }

            //ball collision with left and right side wall
            if (ball.X < 0 || ball.X >= this.Width - ball.Width)
            {
                ballXSpeed *= -1;
            }

            //collision box for paddels
            Rectangle p1Top = new Rectangle(p1X, p1Y, 60, 1);
            Rectangle p1Bot = new Rectangle(p1X, p1Y + 60, 60, 1);
            Rectangle p1Right = new Rectangle(p1X + 60, p1Y, 1, 60);
            Rectangle p1Left = new Rectangle(p1X, p1Y, 1, 60);

            Rectangle p2Top = new Rectangle(p2X, p2Y, 60, 1);
            Rectangle p2Bot = new Rectangle(p2X, p2Y + 60, 60, 1);
            Rectangle p2Right = new Rectangle(p2X + 60, p2Y, 1, 60);
            Rectangle p2Left = new Rectangle(p2X, p2Y, 1, 60);

            //ball collision with player
            if (p1Top.IntersectsWith(ball))
            {
                ballYSpeed *= -1;

                ball.X = puckX;
                ball.Y = puckY;

                player1.X = p1X;
                player1.Y = p1Y;
            }

            if (p1Bot.IntersectsWith(ball))
            {
                ballYSpeed *= -1;

                ball.X = puckX;
                ball.Y = puckY;

                player1.X = p1X;
                player1.Y = p1Y;
            }

            if (p1Right.IntersectsWith(ball))
            {
                ballXSpeed *= -1;

                ball.X = puckX;
                ball.Y = puckY;

                player1.X = p1X;
                player1.Y = p1Y;
            }

            if (p1Left.IntersectsWith(ball))
            {
                ballXSpeed *= -1;

                ball.X = puckX;
                ball.Y = puckY;

                player1.X = p1X;
                player1.Y = p1Y;
            }

            if (p2Top.IntersectsWith(ball))
            {
                ballYSpeed *= -1;

                ball.Y = puckY;

                player2.X = p2X;
                player2.Y = p2Y;
            }

            if (p2Bot.IntersectsWith(ball))
            {
                ballYSpeed *= -1;

                ball.Y = puckY;

                player2.X = p2X;
                player2.Y = p2Y;
            }

            if (p2Right.IntersectsWith(ball))
            {
                ballXSpeed *= -1;

                ball.X = puckX;

                player2.X = p2X;
                player2.Y = p2Y;
            }

            if (p2Left.IntersectsWith(ball))
            {
                ballXSpeed *= -1;

                ball.X = puckX;

                player2.X = p2X;
                player2.Y = p2Y;
            }

            //lattest movement
            lastP1X = player1.X;
            lastP1Y = player1.Y;
            lastP2X = player2.X;
            lastP2Y = player2.Y;

        //prevent ball getting stuck in p1
            if (p1Top.IntersectsWith(ball) && lastP1Y == p1Y)
            {
                player1.Y = player1.Y + 1;
            }

            else if (p1Bot.IntersectsWith(ball) && lastP1Y == p1Y)
            {
                player1.Y = player1.Y - 1;
            }

            else if (p1Right.IntersectsWith(ball) && lastP1X == p1X)
            {
                player1.X = player1.X - 1;
            }

            else if (p1Left.IntersectsWith(ball) && lastP1X == p1X)
            {
                player1.X = player1.X + 1;
            }

            //prevent ball getting stuck in p2
            if (p2Top.IntersectsWith(ball) && lastP2Y == p2Y)
            {
                player2.Y = player2.Y + 1;
            }

            else if (p2Bot.IntersectsWith(ball) && lastP2Y == p2Y)
            {
                player2.Y = player2.Y - 1;
            }

            else if (p2Right.IntersectsWith(ball) && lastP2X == p2X)
            {
                player2.X = player2.X - 1;
            }

            else if (p2Left.IntersectsWith(ball) && lastP2X == p2X)
            {
                player2.X = player2.X + 1;
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int p1X = player1.X;
            int p1Y = player1.Y;
            int p2X = player2.X;
            int p2Y = player2.Y;
            int puckX = ball.X;
            int puckY = ball.Y;

            Rectangle p1Top = new Rectangle(p1X, p1Y, 60, 1);
            Rectangle p1Bot = new Rectangle(p1X, p1Y + 60, 60, 1);
            Rectangle p1Right = new Rectangle(p1X + 60, p1Y, 1, 60);
            Rectangle p1Left = new Rectangle(p1X, p1Y, 1, 60);

            Rectangle p2Top = new Rectangle(p2X, p2Y, 60, 1);
            Rectangle p2Bot = new Rectangle(p2X, p2Y + 60, 60, 1);
            Rectangle p2Right = new Rectangle(p2X + 60, p2Y, 1, 60);
            Rectangle p2Left = new Rectangle(p2X, p2Y, 1, 60);

            e.Graphics.FillEllipse(blueBrush, player1);
            e.Graphics.FillEllipse(redBrush, player2);
            e.Graphics.FillEllipse(blackBrush, ball);

            e.Graphics.DrawRectangle(whitePen, p1Top);
            e.Graphics.DrawRectangle(whitePen, p1Bot);
            e.Graphics.DrawRectangle(whitePen, p1Right);
            e.Graphics.DrawRectangle(whitePen, p1Left);

            e.Graphics.DrawRectangle(whitePen, p2Top);
            e.Graphics.DrawRectangle(whitePen, p2Bot);
            e.Graphics.DrawRectangle(whitePen, p2Right);
            e.Graphics.DrawRectangle(whitePen, p2Left);
        }

    }
}


