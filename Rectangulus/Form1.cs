using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Rectangulus
{
    public partial class Form1 : Form
    {
        Panel store;
        Rectangle p1 = new Rectangle(300, 200, 23, 55);
        Rectangle healthBar = new Rectangle(300, 200, 23, 55);
        Rectangle attackSwipeW = new Rectangle(287, 178, 47, 30);
        Rectangle attackSwipeD = new Rectangle(320, 202, 25, 50);
        Rectangle attackSwipeS = new Rectangle(287, 245, 47, 30);
        Rectangle attackSwipeA = new Rectangle(280, 202, 25, 50);
        Rectangle attackBar = new Rectangle(211, 380, 0, 15);

        List<Rectangle> circleList = new List<Rectangle>();
        List<int> atkColor = new List<int>();
        List<int> circlesHealth = new List<int>();

        int waveHighscore = 0;
        int wave = 1;
        int playerSpeed = 3;
        int playerDamage = 1;
        int playerMaxHealth = 25;
        int playerHealth = 25;
        int playerHealthBar = 55;
        int playerCoins = 0;
        int knockback = 5;

        int circleHealth = 26;
        int circleSpeed = 2;
        int circleDamage = 25;
        int circleCount = 1;
        int circleAlive = 1;
        int circleWidth = 20;
        int circleHeight = 20;
        int circleKills = 0;

        int Timer = 0;

        string direction = "W";

        bool wPressed = false;
        bool sPressed = false;
        bool aPressed = false;
        bool dPressed = false;
        bool enterPressed = false;
        bool gameEnd = false;
        bool inShop = false;
        bool canAttack = false;

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush grayBrush = new SolidBrush(Color.LightGray);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        Pen redPen = new Pen(Color.Red, 2);
        Pen blackPen = new Pen(Color.Black, 2);
        Pen whitePen = new Pen(Color.White, 2);
        Font drawFont = new Font("Ariel", 11);

        Random randGen = new Random();
        int randValue = 0;

        Stopwatch Stopwatch = new Stopwatch();
        public Form1()
        {
            InitializeComponent();
        }

        public void InitializeGame()
        {
            p1 = new Rectangle(300, 200, 23, 55);
            healthBar = new Rectangle(300, 200, 23, 55);
            attackSwipeW = new Rectangle(287, 178, 47, 30);
            attackSwipeD = new Rectangle(320, 202, 25, 50);
            attackSwipeS = new Rectangle(287, 245, 47, 30);
            attackSwipeA = new Rectangle(280, 202, 25, 50);

            waveHighscore = 0;
            wave = 1;
            titleLabel.Text = "";
            subtitleLabel.Text = "";
            playerSpeed = 3;
            playerDamage = 10;
            playerMaxHealth = 25;
            playerHealth = 25;
            playerHealthBar = 55;
            playerCoins = 0;
            knockback = 50;

            circleHealth = 10;
            circleSpeed = 2;
            circleDamage = 1;
            circleCount = 1;
            circleAlive = circleCount;
            circleCountLabel.Text = $"{circleAlive}";
            circleWidth = 20;
            circleHeight = 20;
            circleKills = 0;

            Timer = 0;

            direction = "W";

            gameEnd = false;

            circleList.Clear();
            atkColor.Clear();
            circlesHealth.Clear();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
                case Keys.A:
                    aPressed = false;
                    break;
                case Keys.D:
                    dPressed = false;
                    break;
                case Keys.Enter:
                    enterPressed = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
                case Keys.A:
                    aPressed = true;
                    break;
                case Keys.D:
                    dPressed = true;
                    break;
                case Keys.Enter:
                    enterPressed = true;
                    break;
                case Keys.Escape:
                    if (gameTimer.Enabled == false)
                    {
                        Application.Exit();
                    }
                    break;
                case Keys.Space:
                    if (gameTimer.Enabled == false)
                    {
                        gameTimer.Enabled = true;
                        Stopwatch.Start();
                        InitializeGame();
                    }
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            circleCountLabel.Text = circleAlive + "";
            Timer++;
            keyCheck();

            //spawns Circles
            if (circleCount > 0)
            {
                randValue = randGen.Next(0, 300); //Spawn Circles
                if (randValue < 2)
                {
                    randValue = randGen.Next(1, 5);
                    if (randValue == 1) //Spawn along top
                    {
                        randValue = randGen.Next(20, this.Width - circleWidth * 2);
                        Rectangle circle = new Rectangle(randValue, 0, circleWidth, circleHeight);
                        circleList.Add(circle);
                    }
                    else if (randValue == 2) //Spawn along bottom
                    {
                        randValue = randGen.Next(20, this.Width - circleWidth * 2);
                        Rectangle circle = new Rectangle(randValue, this.Height - circleHeight, circleWidth, circleHeight);
                        circleList.Add(circle);
                    }
                    else if (randValue == 3) //Spawn along left
                    {
                        randValue = randGen.Next(20, this.Height - circleHeight * 2);
                        Rectangle circle = new Rectangle(0, randValue, circleWidth, circleHeight);
                        circleList.Add(circle);
                    }
                    else if (randValue == 4) //Spawn along right
                    {
                        randValue = randGen.Next(20, this.Height - circleHeight * 2);
                        Rectangle circle = new Rectangle(this.Width - circleWidth, randValue, circleWidth, circleHeight);
                        circleList.Add(circle);
                    }
                    circleCount--;
                    circlesHealth.Add(circleHealth);
                    atkColor.Add(1);
                }
            }

            for (int i = 0; i < circleList.Count(); i++) //Move Circles 
            {
                if (circleList[i].IntersectsWith(p1))
                {

                }
                else
                {
                    if (circleList[i].Y > p1.Y) //Move up
                    {
                        int y = circleList[i].Y - circleSpeed;
                        circleList[i] = new Rectangle(circleList[i].X, y, circleList[i].Width, circleList[i].Height);
                    }
                    if (circleList[i].Y < p1.Y)//Move down
                    {
                        int y = circleList[i].Y + circleSpeed;
                        circleList[i] = new Rectangle(circleList[i].X, y, circleList[i].Width, circleList[i].Height);
                    }
                    if (circleList[i].X > p1.X)//Move left
                    {
                        int x = circleList[i].X - circleSpeed;
                        circleList[i] = new Rectangle(x, circleList[i].Y, circleList[i].Width, circleList[i].Height);
                    }
                    if (circleList[i].X < p1.X)//Move Right
                    {
                        int x = circleList[i].X + circleSpeed;
                        circleList[i] = new Rectangle(x, circleList[i].Y, circleList[i].Width, circleList[i].Height);
                    }
                }
            }

            if (canAttack == true && enterPressed == true) //Attacking
            {
                canAttack = false;
                attackBar.Width = 0;
                for (int i = 0; i < circleList.Count(); i++)
                {
                    if (direction == "W")
                    {
                        if (circleList[i].IntersectsWith(attackSwipeW)) //Attack facing up
                        {
                            circlesHealth[i] -= playerDamage;
                            atkColor[i] = 20;

                            int y = circleList[i].Y - knockback;
                            circleList[i] = new Rectangle(circleList[i].X, y, circleList[i].Width, circleList[i].Height);
                        }
                    }
                    if (direction == "S")
                    {
                        if (circleList[i].IntersectsWith(attackSwipeS)) //Attack facing down
                        {
                            circlesHealth[i] -= playerDamage;
                            atkColor[i] = 20;

                            int y = circleList[i].Y + knockback;
                            circleList[i] = new Rectangle(circleList[i].X, y, circleList[i].Width, circleList[i].Height);
                        }
                    }
                    if (direction == "A")
                    {
                        if (circleList[i].IntersectsWith(attackSwipeA)) //Attack facing left
                        {
                            circlesHealth[i] -= playerDamage;
                            atkColor[i] = 20;

                            int x = circleList[i].X - knockback;
                            circleList[i] = new Rectangle(x, circleList[i].Y, circleList[i].Width, circleList[i].Height);
                        }
                    }
                    if (direction == "D")
                    {
                        if (circleList[i].IntersectsWith(attackSwipeD)) //Attack facing right
                        {
                            circlesHealth[i] -= playerDamage;
                            atkColor[i] = 20;

                            int x = circleList[i].X + knockback;
                            circleList[i] = new Rectangle(x, circleList[i].Y, circleList[i].Width, circleList[i].Height);
                        }
                    }

                    if (circlesHealth[i] <= 0) //Kill circle
                    {
                        randValue = randGen.Next(1, 4);
                        playerCoins += randValue;
                        circleKills++;
                        circleAlive--;
                        circleList.RemoveAt(i);
                        circlesHealth.RemoveAt(i);
                        atkColor.RemoveAt(i);
                    }
                }
            }

            if (circleAlive <= 0)
            {
                gameTimer.Stop();
                inShop = true;
                store = new Panel();
                store = panel1;
                store.Enabled = true;
                store.Visible = true;
                

                // panel1.Visible = true;
                // panel1.Enabled = true;

            }

            if (Timer % 100 == 0) //Circles attack
            {
                for (int i = 0; i < circleList.Count(); i++)
                {
                    if (circleList[i].IntersectsWith(p1))
                    {
                        if (playerHealth > 1)
                        {
                            playerHealth -= circleDamage;
                            double healthPercent = (playerHealth * 100) / playerMaxHealth;
                            healthPercent = (healthPercent * 55) / 100;
                            timerLabel.Text = healthPercent.ToString("##");
                            try
                            {
                                playerHealthBar = Convert.ToInt16(timerLabel.Text);
                            }
                            catch
                            {
                                playerHealthBar = 0;
                            }
                            healthBar.Height = playerHealthBar;

                            int temp = p1.Height - healthBar.Height;
                            healthBar.Y = p1.Y + temp;
                        }
                        else
                        {
                            playerHealth = 0;
                        }

                    }
                }
            }

            for (int i = 0; i < circleList.Count(); i++) //Allows circles to turn red when attacked then turn back to white
            {
                if (atkColor[i] > 0)
                {
                    atkColor[i]--;
                }
            }

            if (playerHealth == 0)
            {
                gameTimer.Stop();
                gameEnd = true;
            }

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameTimer.Enabled == false)
            {
                if (gameEnd == true)
                {
                    titleLabel.Text = "Game Over | You died";
                    if (wave > waveHighscore)
                    {
                        waveHighscore = wave;
                    }
                    subtitleLabel.Text = $"Highest Wave: {waveHighscore}";
                    subtitleLabel.Text += $"\nCurrent Wave: {wave}";
                    subtitleLabel.Text += $"\nCoins: {playerCoins}";
                    subtitleLabel.Text += $"\nKills: {circleKills}";
                    subtitleLabel.Text += $"\nPress SPACE to play again or ESC to exit";
                }
                else
                {
                    titleLabel.Text = "Welcome to Rectangulus";
                    subtitleLabel.Text = "Press SPACE to begin or ESC to exit";
                }
            }
            else if (gameTimer.Enabled == true)
            {
                e.Graphics.FillRectangle(whiteBrush, p1);
                e.Graphics.FillRectangle(redBrush, healthBar);
                e.Graphics.DrawRectangle(blackPen, p1);

                if (direction == "W")
                {
                    e.Graphics.DrawArc(blackPen, attackSwipeW, 180, 180);
                }
                if (direction == "D")
                {
                    e.Graphics.DrawArc(blackPen, attackSwipeD, 270, 180);
                }
                if (direction == "S")
                {
                    e.Graphics.DrawArc(blackPen, attackSwipeS, 360, 180);
                }
                if (direction == "A")
                {
                    e.Graphics.DrawArc(blackPen, attackSwipeA, 90, 180);
                }


                for (int i = 0; i < circleList.Count(); i++)
                {
                    if (atkColor[i] > 2)
                    {
                        e.Graphics.FillEllipse(redBrush, circleList[i]);
                        e.Graphics.DrawEllipse(blackPen, circleList[i]);
                    }
                    else
                    {
                        e.Graphics.FillEllipse(whiteBrush, circleList[i]);
                        e.Graphics.DrawEllipse(blackPen, circleList[i]);
                    }
                }
                if (attackBar.Width <= 199)
                {
                    if (Timer % 2 == 0)
                    {
                        attackBar.Width += 1;
                    }
                }
                else
                {
                    canAttack = true;
                }
                e.Graphics.FillRectangle(grayBrush, attackBar);
            }
        }
        public void keyCheck()
        {
            if (wPressed == true && p1.Y >= 0)
            {
                p1.Y -= playerSpeed;
                healthBar.Y -= playerSpeed;
                attackSwipeW.Y -= playerSpeed;
                attackSwipeD.Y -= playerSpeed;
                attackSwipeS.Y -= playerSpeed;
                attackSwipeA.Y -= playerSpeed;
                direction = "W";
            }
            if (sPressed == true && p1.Y < this.Height - p1.Height)
            {
                p1.Y += playerSpeed;
                healthBar.Y += playerSpeed;
                attackSwipeW.Y += playerSpeed;
                attackSwipeD.Y += playerSpeed;
                attackSwipeS.Y += playerSpeed;
                attackSwipeA.Y += playerSpeed;
                direction = "S";
            }

            if (aPressed == true && p1.X >= 0)
            {
                p1.X -= playerSpeed;
                healthBar.X -= playerSpeed;
                attackSwipeW.X -= playerSpeed;
                attackSwipeD.X -= playerSpeed;
                attackSwipeS.X -= playerSpeed;
                attackSwipeA.X -= playerSpeed;
                direction = "A";
            }
            if (dPressed == true && p1.X < this.Width - p1.Width)
            {
                p1.X += playerSpeed;
                healthBar.X += playerSpeed;
                attackSwipeW.X += playerSpeed;
                attackSwipeD.X += playerSpeed;
                attackSwipeS.X += playerSpeed;
                attackSwipeA.X += playerSpeed;
                direction = "D";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            store.Dispose();
            inShop = false;
            //   Refresh();

            playerDamage = 100;
        }
    }
}
