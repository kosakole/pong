using Game.Grid;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string BEST_SCORE_FILE_NAME = "best score.txt";
        private bool isStarted = false;
        private readonly int rows = 19, columns = 18;
        private readonly int refreshTime = 70;
        private readonly int pauseTime = 1000;
        private readonly int borderLenght =4;
        private decimal time = 0;
        private readonly Image[,] gridImages;
        private GameState State;
        private static readonly Dictionary<GridValue, ImageSource> gridValueToImage = new Dictionary<GridValue, ImageSource>()
        {
            {GridValue.Empty, Images.Empty },
            {GridValue.Border, Images.Border},
            {GridValue.Ball, Images.Ball },
            {GridValue.Target, Images.Target}
        };
        public MainWindow()
        {
            InitializeComponent();
            gridImages = new Image[rows, columns];
            init();
            DrawGrid();
            
        }

        private void init()
        {
            GameGrid.Rows = rows;
            GameGrid.Columns = columns;
            State = new GameState(rows, columns, borderLenght);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    gridImages[i, j] = new Image()
                    {
                        Source = Images.Empty
                    };
                }
            }
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isStarted)
            {
                time = 0;
                isStarted = true;
                Overlay.Visibility = Visibility.Collapsed;
                DrawGrid();
                await GameLoop();
            }

            if (State.GameOver) 
            {
                await Task.Delay(pauseTime);
                isStarted = false;
                State = new GameState(rows, columns, borderLenght);
            }
                
            switch (e.Key)
            {
                case Key.Left:
                        State.MoveLeft();
                    break;
                case Key.Right:
                    State.MoveRight();
                    break;
            }
        }

        private async Task GameLoop()
        {
            int i = 0;
            while (!State.GameOver)
            {
                i++;
                await Task.Delay(refreshTime);
                time = time + 0.1m;
                TextTime.Text = $"Time: {time.ToString()}s";
                DrawGrid();
                if(i == 2) 
                {
                    State.Move();
                    i = 0;
                }
                
            }
            Overlay.Visibility = Visibility.Visible;
            OverlayTextScore.Text = $"Score: {getScore().ToString()}\nBest score is: {getBestScore()}";
            OverlayTextPNK.VerticalAlignment = VerticalAlignment.Bottom;
           ;
        }

        private void DrawGrid()
        {
            GameGrid.Children.Clear();
            for (int i = 0; i < rows; i++)
            {
                for(int j =  0; j < columns; j++)
                {
                    GridValue gridValue = State.Grid[i, j];
                    gridImages[i, j].Source = 
                        gridValueToImage[gridValue];
                    GameGrid.Children.Add(gridImages[i, j]);
                }
            }
        }

        private int getScore()
        {
            int numTarget = 0;
            for(int i = 0; i < State.numTargetRows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    if (State.Grid[i, j] == GridValue.Target) 
                        numTarget++;
                }
            }
            return columns * State.numTargetRows - numTarget;
        }

        private int getBestScore()
        {
            int bestScore = 0;
            StreamReader sr;
            try
            {
                 sr = new StreamReader(BEST_SCORE_FILE_NAME);
            }
            catch (Exception e) 
            {
                FileStream fs = File.Create(BEST_SCORE_FILE_NAME);
                fs.Close();
                sr = new StreamReader(BEST_SCORE_FILE_NAME);
            }
            try
            {
                bestScore = Int32.Parse(sr.ReadLine());
            }
            catch (Exception)
            { 

            }
            sr.Close();
            if(bestScore < getScore())
            {
                StreamWriter sw = new StreamWriter(BEST_SCORE_FILE_NAME);
                sw.WriteLine(getScore().ToString());
                sw.AutoFlush = true;
                sw.Close();
            }
            return bestScore;
        }
    }
}
