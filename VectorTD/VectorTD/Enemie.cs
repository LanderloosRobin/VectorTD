using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace VectorTD
{
    class Enemie
    {

        private DispatcherTimer timerEnemie = new DispatcherTimer();
        private Canvas gameCanvas = new Canvas();
        private Ellipse enemie;

        public Enemie(Canvas gameCanvas)
        {
            this.gameCanvas = gameCanvas;
            enemie = createEnemie();
            timerEnemie.Tick += TimerEnemie_Tick;
            timerEnemie.Interval = TimeSpan.FromMilliseconds(10);
        }


        private void TimerEnemie_Tick(object sender, EventArgs e)
        {
            gameCanvas.Children.Remove(enemie);
            enemieYPos += 2;
            enemie.Margin = new Thickness(enemieXPos, enemieYPos, 0, 0);
            gameCanvas.Children.Add(enemie);
            enemiePoint = new Point(enemieXPos + 15, enemieYPos + 15);

            if (enemieYPos >= 500)
            {
                gameCanvas.Children.Remove(enemie);
                timerEnemie.Stop();
            }
        }

        public void addEnemie()
        {
            gameCanvas.Children.Add(enemie);
        }

        public void removeEnemie()
        {
            gameCanvas.Children.Remove(enemie);
        }

        public Ellipse createEnemie()
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = new SolidColorBrush(Colors.Red);
            ellipse.Fill = new SolidColorBrush(Colors.Red);
            ellipse.Width = 30;
            ellipse.Height = 30;
            ellipse.Margin = new Thickness(100, 0, 0, 0);
            return ellipse;
        }

        public void StartTimer()
        {
            timerEnemie.Start();
        }

        public void StopTimer()
        {
            timerEnemie.Stop();
        }

        private double enemieXPos;

        public double enemieXPosition
        {
            get { return enemieXPos; }
            set { enemieXPos = value; }
        }

        private double enemieYPos;

        public double enemieYPosition
        {
            get { return enemieYPos; }
            set { enemieYPos = value; }
        }

        private Point enemiePoint;

        public Point pointEnemie
        {
            get { return enemiePoint; }
            set { enemiePoint = value; }
        }

    }
}
