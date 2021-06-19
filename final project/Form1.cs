//emmanuel bailey
//2021 06 18
//Mr.t 3U computer sience


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

namespace final_project
{
    public partial class Form1 : Form
    {

        Rectangle hero = new Rectangle(180, 450, 40, 40);

        SoundPlayer musicPlayer;



        int heroSpeed = 10;

        int bulletSpeed = -10;

        List<Rectangle> asteroid1List = new List<Rectangle>();
        List<Rectangle> asteroid2List = new List<Rectangle>();
        List<Rectangle> landingShipList = new List<Rectangle>();
        List<int> ballSpeeds = new List<int>();
        List<string> ballColours = new List<string>();
        int ballSize = 40;

        List<Rectangle> bullet = new List<Rectangle>();

        int score = 0;
        int lives = 3;


        bool leftDown = false;
        bool rightDown = false;



        Random randGen = new Random();
        int randValue = 0;

        SolidBrush redBrush = new SolidBrush(Color.Red);
        Image asteroid1 = Properties.Resources.asteroid1;
        Image asteroid2 = Properties.Resources.asteroid2;
        Image landingRocket = Properties.Resources.landingRocket;
        Image playerShip = Properties.Resources.playerShip;
        Image city = Properties.Resources.city;


        int groundHeight = 200;

        string gameState = "waiting";



        public Form1()
        {
            InitializeComponent();

        }

        public void GameInitialize()
        {
            titleLabel.Text = "";
            subTitleLabel.Text = "";

            gameTimer.Enabled = true;
            gameState = "running";

            score = 0;
            lives = 3;

            livesLabel.Text = "Lives:♥♥♥";
            asteroid1List.Clear();
            asteroid2List.Clear();
            landingShipList.Clear();
            ballColours.Clear();
            ballSpeeds.Clear();

            hero.X = this.Width / 2 - hero.Width / 2;
            hero.Y = this.Height - groundHeight - hero.Height;


        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
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
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move hero 
            if (leftDown == true && hero.X > 0)
            {
                hero.X -= heroSpeed;
            }

            if (rightDown == true && hero.X < this.Width - hero.Width)
            {
                hero.X += heroSpeed;
            }

            for (int i = 0; i < bullet.Count(); i++)
            {

                int y = bullet[i].Y + bulletSpeed;


                bullet[i] = new Rectangle(bullet[i].X, y, 5, 20);
            }

            //check to see if a new object should be created 
            randValue = randGen.Next(0, 101);

            if (randValue < 1) //landingShip
            {
                int x = randGen.Next(10, this.Width - ballSize * 2);
                landingShipList.Add(new Rectangle(x, 10, ballSize, ballSize));
                ballSpeeds.Add(randGen.Next(2, 10));


            }
            else if (randValue < 6) //asteroid1
            {
                int x = randGen.Next(10, this.Width - ballSize * 2);
                asteroid1List.Add(new Rectangle(x, 10, ballSize, ballSize));

                ballSpeeds.Add(randGen.Next(2, 10));



            }
            else if (randValue < 11) //asteroid2
            {
                int x = randGen.Next(10, this.Width - ballSize * 2);
                asteroid2List.Add(new Rectangle(x, 10, ballSize, ballSize));

                ballSpeeds.Add(randGen.Next(2, 10));


            }



            // move balls 
            for (int i = 0; i < landingShipList.Count(); i++)
            {
                //find the new postion of y based on speed 
                int y = landingShipList[i].Y + ballSpeeds[i];

                //replace the rectangle in the list with updated one using new y 
                landingShipList[i] = new Rectangle(landingShipList[i].X, y, ballSize, ballSize);
            }
            for (int i = 0; i < asteroid1List.Count(); i++)
            {
                  
                int y = asteroid1List[i].Y + ballSpeeds[i];

                
                asteroid1List[i] = new Rectangle(asteroid1List[i].X, y, ballSize, ballSize);
            }
            for (int i = 0; i < asteroid2List.Count(); i++)
            {
               
                int y = asteroid2List[i].Y + ballSpeeds[i];

                 
                asteroid2List[i] = new Rectangle(asteroid2List[i].X, y, ballSize, ballSize);
            }




            //check if ball is below play area and remove it if it is 
            for (int i = 0; i < landingShipList.Count(); i++)
            {


                if (landingShipList[i].Y > this.Height - ballSize - groundHeight)
                {
                    score += 250;

                    landingShipList.RemoveAt(i);
                    ballSpeeds.RemoveAt(i);

                }
            }
            for (int i = 0; i < asteroid1List.Count(); i++)
            {


                if (asteroid1List[i].Y > this.Height - ballSize - groundHeight)
                {
                    score -= 100;

                    asteroid1List.RemoveAt(i);
                    ballSpeeds.RemoveAt(i);

                }
            }
            for (int i = 0; i < asteroid2List.Count(); i++)
            {


                if (asteroid2List[i].Y > this.Height - ballSize - groundHeight)
                {
                    score -= 50;

                    asteroid2List.RemoveAt(i);
                    ballSpeeds.RemoveAt(i);

                }
            }

            if (lives == 3)
            {
                livesLabel.Text = "Lives:♥♥♥";
            }
            else if (lives == 2)
            {
                livesLabel.Text = "Lives:♥♥";

            }
            else if (lives == 1)
            {
                livesLabel.Text = "Lives:♥";
            }
            else if (lives == 0)
            {
                gameState = "over";


            }

            //collistion with hero and falling object

            for (int i = 0; i < landingShipList.Count(); i++)
            {
                if (hero.IntersectsWith(landingShipList[i]))
                {
                    lives -= 3;


                    musicPlayer = new SoundPlayer(Properties.Resources.asteroidHit);
                    musicPlayer.Play();

                    landingShipList.RemoveAt(i);
                    ballSpeeds.RemoveAt(i);

                }
            }
            for (int i = 0; i < asteroid1List.Count(); i++)
            {
                if (hero.IntersectsWith(asteroid1List[i]))
                {
                    lives -= 1;


                    musicPlayer = new SoundPlayer(Properties.Resources.asteroidHit);
                    musicPlayer.Play();

                    asteroid1List.RemoveAt(i);
                    ballSpeeds.RemoveAt(i);

                }


            }
            for (int i = 0; i < asteroid2List.Count(); i++)
            {
                if (hero.IntersectsWith(asteroid2List[i]))
                {
                    lives -= 1;

                    musicPlayer = new SoundPlayer(Properties.Resources.asteroidHit);
                    musicPlayer.Play();

                    asteroid2List.RemoveAt(i);
                    ballSpeeds.RemoveAt(i);

                }
            }

            //bulet collitions with faling object
            for (int i = 0; i < bullet.Count(); i++)
            {
                for (int j = 0; j < landingShipList.Count(); j++)
                {

                    if (bullet[i].IntersectsWith(landingShipList[j]))
                    {
                        musicPlayer = new SoundPlayer(Properties.Resources.asteroidHit);
                        musicPlayer.Play();

                        bullet.RemoveAt(i);
                        landingShipList.RemoveAt(j);
                        score = score + 200;
                        break;
                    }
                }
            }

            for (int i = 0; i < bullet.Count(); i++)
            {
                for (int j = 0; j < asteroid1List.Count(); j++)
                {

                    if (bullet[i].IntersectsWith(asteroid1List[j]))
                    {

                        musicPlayer = new SoundPlayer(Properties.Resources.asteroidHit);
                        musicPlayer.Play();

                        bullet.RemoveAt(i);
                        asteroid1List.RemoveAt(j);
                        score = score + 200;
                        break;
                    }
                }
            }
            for (int i = 0; i < bullet.Count(); i++)
            {
                for (int j = 0; j < asteroid2List.Count(); j++)
                {

                    if (bullet[i].IntersectsWith(asteroid2List[j]))
                    {

                        musicPlayer = new SoundPlayer(Properties.Resources.asteroidHit);
                        musicPlayer.Play();

                        bullet.RemoveAt(i);
                        asteroid2List.RemoveAt(j);
                        score = score + 200;
                        break;
                    }
                }
            }

            Refresh();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //update labels 

            scoreLabel.Text = $"Score: {score}";

            //draw hero playerRocket

            e.Graphics.DrawImage(playerShip, hero);

            e.Graphics.DrawImage(city, 0, this.Height - groundHeight,

       this.Width, groundHeight);



            //game waitig screen
            if (gameState == "waiting")

            {
                titleLabel.Text = "Asteroid";
                subTitleLabel.Text = "Press Space Bar to Start or Escape to Exit";
            }

            else if (gameState == "running")

            {

                scoreLabel.Text = $"Score: {score}";

                //falling object immage
                for (int i = 0; i < landingShipList.Count(); i++)
                {



                    e.Graphics.DrawImage(landingRocket, landingShipList[i]);

                }
                for (int i = 0; i < asteroid1List.Count(); i++)
                {


                    e.Graphics.DrawImage(asteroid1, asteroid1List[i]);
                }


                for (int i = 0; i < asteroid2List.Count(); i++)
                {
                    e.Graphics.DrawImage(asteroid2, asteroid2List[i]);
                }

                for (int i = 0; i < bullet.Count(); i++)
                {


                    e.Graphics.FillRectangle(redBrush, bullet[i]);
                }
            }

            //game over screen
            else if (gameState == "over")

            {
                scoreLabel.Text = "";

                titleLabel.Text = "GAME OVER";

                subTitleLabel.Text = $"Your final score was {score}";

                subTitleLabel.Text += "\nPress Space Bar to Start or Escape to Exit";

            }




        }

        //bullet shooting
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            bullet.Add(new Rectangle(hero.X + 18, hero.Y, 5, 5));

            musicPlayer = new SoundPlayer(Properties.Resources.lazer);
            musicPlayer.Play();


        }
    }
}
