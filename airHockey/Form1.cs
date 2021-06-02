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
        Rectangle player1 = new Rectangle(260, 25, 60, 60);
        Rectangle player2 = new Rectangle(260, 780, 60, 60);
        Rectangle puck = new Rectangle(281, 420, 30, 30);
        Rectangle player1Goal = new Rectangle(159, 15, 265, 2);
        Rectangle player2Goal = new Rectangle(159, 845, 265, 2);

        int player1Score = 0;
        int player2Score = 0;

        int PLAYER_SPEED = 6;
        int puckXSpeed = 6;
        int puckYSpeed = 6;

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

            //Make game boarder
            Rectangle boarder = new Rectangle(15, 15, this.Width - 30, this.Height - 30);

            //last move
            int p1X = player1.X;
            int p1Y = player1.Y;
            int p2X = player2.X;
            int p2Y = player2.Y;
            int puckX = puck.X;
            int puckY = puck.Y;

            //move puck
            puck.X += puckXSpeed;
            puck.Y += puckYSpeed;

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

            //puck collision with top and bottom walls
            if (puck.Y < 15 || puck.Y > this.Height - puck.Height - 15)
            {
                puckYSpeed *= -1;
            }
            
            //puck collision with left and right side wall
            if (puck.X < 15 || puck.X >= this.Width - puck.Width - 15)
            {
                puckXSpeed *= -1;
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

            //puck collision with player
            if (p1Top.IntersectsWith(puck))
            {
                puckYSpeed *= -1;

                puck.X = puckX;
                puck.Y = puckY;

                player1.X = p1X;
                player1.Y = p1Y;
            }

            if (p1Bot.IntersectsWith(puck))
            {
                puckYSpeed *= -1;

                puck.X = puckX;
                puck.Y = puckY;

                player1.X = p1X;
                player1.Y = p1Y;
            }

            if (p1Right.IntersectsWith(puck))
            {
                puckXSpeed *= -1;

                puck.X = puckX;
                puck.Y = puckY;

                player1.X = p1X;
                player1.Y = p1Y;
            }

            if (p1Left.IntersectsWith(puck))
            {
                puckXSpeed *= -1;

                puck.X = puckX;
                puck.Y = puckY;

                player1.X = p1X;
                player1.Y = p1Y;
            }

            if (p2Top.IntersectsWith(puck))
            {
                puckYSpeed *= -1;

                puck.X = puckX;
                puck.Y = puckY;

                player2.X = p2X;
                player2.Y = p2Y;
            }

            if (p2Bot.IntersectsWith(puck))
            {
                puckYSpeed *= -1;

                puck.X = puckX;
                puck.Y = puckY;

                player2.X = p2X;
                player2.Y = p2Y;
            }

            if (p2Right.IntersectsWith(puck))
            {
                puckXSpeed *= -1;

                puck.X = puckX;
                puck.Y = puckY;

                player2.X = p2X;
                player2.Y = p2Y;
            }

            if (p2Left.IntersectsWith(puck))
            {
                puckXSpeed *= -1;

                puck.X = puckX;
                puck.Y = puckY;

                player2.X = p2X;
                player2.Y = p2Y;
            }

            //lattest movement
            lastP1X = player1.X;
            lastP1Y = player1.Y;
            lastP2X = player2.X;
            lastP2Y = player2.Y;

            //prevent puck getting stuck in p1
            if (p1Top.IntersectsWith(puck) && lastP1Y == p1Y && lastP1X == p1X)
            {
                player1.Y++;

            }

            else if (p1Bot.IntersectsWith(puck) && lastP1Y == p1Y && lastP1X == p1X)
            {
                player1.Y--;
            }

            else if (p1Right.IntersectsWith(puck) && lastP1Y == p1Y && lastP1X == p1X)
            {
                player1.X--;
                puck.X++;
            }

            else if (p1Left.IntersectsWith(puck) && lastP1Y == p1Y && lastP1X == p1X)
            {
                player1.X++;
            }

            //prevent puck getting stuck in p2
            if (p2Top.IntersectsWith(puck) && lastP2Y == p2Y && lastP2X == p2X)
            {
                player2.Y++;
            }

            else if (p2Bot.IntersectsWith(puck) && lastP2Y == p2Y && lastP2X == p2X)
            {
                player2.Y++;
            }

            else if (p2Right.IntersectsWith(puck) && lastP2Y == p2Y && lastP2X == p2X)
            {
                player2.X--;
            }

            else if (p2Left.IntersectsWith(puck) && lastP2Y == p2Y && lastP2X == p2X)
            {
                player2.X++;
            }


            if (player1Goal.IntersectsWith(puck))
            {
                player2Score++;
                p2Score.Text = $"{player2Score}";

                puck.X = 281;
                puck.Y = 420;

                player1.X = 260;
                player1.Y = 258;
                player2.X = 260;
                player2.Y = 780;
            }
            else if (player2Goal.IntersectsWith(puck))
            {
                player1Score++;
                p1Score.Text = $"{player1Score}";

                puck.X = 281;
                puck.Y = 420;

                player1.X = 260;
                player1.Y = 258;
                player2.X = 260;
                player2.Y = 780;
            }



            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
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

            e.Graphics.DrawImage(Properties.Resources.bluePaddel, player1);
            e.Graphics.DrawImage(Properties.Resources.redPaddle, player2);
            e.Graphics.FillEllipse(blackBrush, puck);
        }
    }
}


