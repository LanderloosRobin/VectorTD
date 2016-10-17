using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace VectorTD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private Shape[,] gridArray = new Shape[11, 10];
        private bool addingTurret = false;
        private bool removingTurret = false;    
        private DispatcherTimer timerTurret;
        private DispatcherTimer timerLaser = new DispatcherTimer();
        private Line laser;
        private double enemieYPos;
        private double enemieXPos;
        private double turretYPos;
        private double turretXPos;
        private Enemie enemie;
        private const double turretRange = 200;
        private Point enemiePoint;
        private Point turretPoint;
        private List<Ellipse> turretList = new List<Ellipse>();

        public MainWindow()
        {
            InitializeComponent();
            initGrid();
        }

        public void setTurret(int xPos, int yPos)
        {
            Ellipse turret = createTurret();
            turret.Margin = new Thickness(xPos, yPos, 0, 0);
            turretList.Add(turret);
            gameCanvas.Children.Add(turret);
            turretYPos = turret.Margin.Top;
            turretXPos = turret.Margin.Left;
            turretPoint = new Point(turretXPos +25, turretYPos + 25);
        }

        private void TimerTurret_Tick(object sender, EventArgs e)
        {
            checkIfWithinTurretRange();
            testBox.Text = enemieYPos.ToString();
        }

        private void checkIfWithinTurretRange()
        {
            foreach(Ellipse turret in turretList)
            {
                if (enemieYPos >= turretYPos - turretRange + 25 && enemieYPos <= turretYPos + turretRange - 25
                    && enemieXPos >= turretXPos - turretRange + 25 && enemieXPos <= turretXPos + turretRange - 25)
                {
                    drawLaser();
                    enemie.removeEnemie();
                    MessageBox.Show("Hit");
                    timerTurret.Stop();
                }

            }
        }

        private void drawLaser()
        {
            laser = new Line();
            laser.Fill = new SolidColorBrush(Colors.Blue);
            laser.Stroke = new SolidColorBrush(Colors.Blue);
            laser.StrokeThickness = 2;
            laser.X1 = turretPoint.X;
            laser.X2 = enemiePoint.X;
            laser.Y1 = turretPoint.Y;
            laser.Y2 = enemiePoint.Y;
            gameCanvas.Children.Add(laser);
            timerLaser.Tick += TimerLaser_Tick;
            timerLaser.Interval = TimeSpan.FromSeconds(2);
            timerLaser.Start();
        }

        private int i = 0;

        private void TimerLaser_Tick(object sender, EventArgs e)
        {
            i++;
            if (i == 1)
            {
                gameCanvas.Children.Remove(laser);
                timerLaser.Stop();
            }
        }

        public Ellipse createTurret()
        {
            Ellipse turret = new Ellipse();
            turret.Stroke = new SolidColorBrush(Colors.Red);
            turret.Fill = new SolidColorBrush(Colors.Red);
            turret.Width = 50;
            turret.Height = 50;
            return turret;
        }



        public void initGrid()
        {
            int column = 0;
            double x = 0;
            double y = 0;
            Rectangle rect;

            for(int row = 0; row <= 10; row++)
            {
                for (column = 0; column <= 9; column++)
                {
                    rect = createRectangle();
                    rect.Margin = new Thickness(x, y, 0, 0);
                    gridArray[row, column] = rect;
                    x += 90;
                }
                y += 90;
                x = 0;
            }

            for(int i = 0; i <= 10; i++)
            {
                for(int j = 0; j <= 9; j++)
                {
                    gameCanvas.Children.Add(gridArray[i, j]);
                }
            }

        }

        public Rectangle createRectangle()
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Stroke = new SolidColorBrush(Colors.Green);
            rectangle.Width = gameCanvas.Width / 10;
            rectangle.Height = gameCanvas.Height / 10;
            return rectangle;
        }

        private void btnAddTurret_Click(object sender, RoutedEventArgs e)
        {
            addingTurret = true;
        }

        private void gameCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (addingTurret)
            {
                Point mousePos = Mouse.GetPosition(Application.Current.MainWindow);
                double mouseX = mousePos.X - 125;
                double mouseY = mousePos.Y - 25;
                setTurret(Convert.ToInt32(mouseX), Convert.ToInt32(mouseY));
                addingTurret = false;
            }

            if (removingTurret)
            {
                Point mousePos = Mouse.GetPosition(Application.Current.MainWindow);
                double mouseX = mousePos.X - 125;
                double mouseY = mousePos.Y - 25;
                
                foreach(Ellipse turret in turretList)
                {
                    if (turret.Margin.Top <= mouseY && turret.Margin.Top + 50 >= mouseY
                        && turret.Margin.Left <= mouseX && turret.Margin.Left + 50 >= mouseX)
                    {
                        gameCanvas.Children.Remove(turret);
                        removingTurret = false;
                    }
                }
            }
        }

        private void btnStartEnemies_Click(object sender, RoutedEventArgs e)
        {
            enemie = new Enemie(gameCanvas);
            enemie.addEnemie();
            enemieYPos = enemie.enemieYPosition;
            enemieXPos = enemie.enemieXPosition;
            foreach(Ellipse turret in turretList)
            {
                timerTurret = new DispatcherTimer();
                timerTurret.Tick += TimerTurret_Tick;
                timerTurret.Interval = TimeSpan.FromMilliseconds(1);
            }
            enemie.StartTimer();
            timerTurret.Start();
        }

        private void btnRemoveTurret_Click(object sender, RoutedEventArgs e)
        {
            removingTurret = true;
        }
    }
}
