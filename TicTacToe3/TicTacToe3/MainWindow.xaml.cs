using System;
using System.IO;
using System.Diagnostics;
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
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Security.Cryptography.X509Certificates;
using Nakov.TurtleGraphics;
using System.Runtime;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        public string LoadCurrentPlayer;
        public string TurnPassed;
        public Dictionary<Player, ImageSource> imageSources = new()
        {
            { Player.X, new BitmapImage(new Uri("pack://application:,,,/Asset/X15.png")) },
            { Player.O, new BitmapImage(new Uri("pack://application:,,,/Asset/O15.png")) }
        };

        private readonly Dictionary<Player, ObjectAnimationUsingKeyFrames> animations = new()
        {
            { Player.X, new ObjectAnimationUsingKeyFrames() },
            { Player.O, new ObjectAnimationUsingKeyFrames() }
        };

        private readonly DoubleAnimation fadeOutAnimation = new DoubleAnimation
        {
            Duration = TimeSpan.FromSeconds(.5),
            From = 1,
            To = 0
        };

        private readonly DoubleAnimation fadeInAnimation = new DoubleAnimation
        {
            Duration = TimeSpan.FromSeconds(.5),
            From = 0,
            To = 1
        };


        private Canvas[,] Grounds;
        private Image[,] imageControls ;
        public GameState gameState = new GameState();

        public MainWindow()
        {
            imageControls = new Image[gameState.generic_value, gameState.generic_value];
            Grounds = new Canvas[gameState.generic_value, gameState.generic_value];
            InitializeComponent();
            SetupGameGrid();
            SetupAnimations();

            gameState.MoveMade += OnMoveMade;
            gameState.GameEnded += OnGameEnded;
            gameState.GameRestarted += OnGameRestarted;

            SetupGrid();

        }

        public void SetupGrid()
        {
            TheGrid.RowDefinitions.Clear();
            TheGrid.ColumnDefinitions.Clear();

            for (int r = 0; r < gameState.generic_value; r++)
            {
                TheGrid.RowDefinitions.Add(new RowDefinition());
                TheGrid.ColumnDefinitions.Add(new ColumnDefinition());

            }
        }

        public void SetupGameGrid()
        {
            GameGrid.Children.Clear();

   
                for (int r = 0; r < gameState.generic_value; r++)
                {
                    for (int c = 0; c < gameState.generic_value; c++)
                    {
                        Canvas playground = new Canvas();
                        GameGrid.Children.Add(playground);
                        Grounds[r, c] = playground;

                    }

                }

        }

        private void SetupAnimations()
        {
            animations[Player.X].Duration = TimeSpan.FromSeconds(.25);
            animations[Player.O].Duration = TimeSpan.FromSeconds(.25);

            for (int i = 0; i < 16; i++)
            {
                Uri xUri = new Uri($"pack://application:,,,/Asset/X{i}.png");
                BitmapImage xImg = new BitmapImage(xUri);
                DiscreteObjectKeyFrame xKeyFrame = new DiscreteObjectKeyFrame(xImg);
                animations[Player.X].KeyFrames.Add(xKeyFrame);

                Uri oUri = new Uri($"pack://application:,,,/Asset/O{i}.png");
                BitmapImage oImg = new BitmapImage(oUri);
                DiscreteObjectKeyFrame oKeyFrame = new DiscreteObjectKeyFrame(oImg);
                animations[Player.O].KeyFrames.Add(oKeyFrame);
            }
        }

        private async Task FadeOut(UIElement uiElement)
        {
            uiElement.BeginAnimation(OpacityProperty, fadeOutAnimation);
            await Task.Delay(fadeOutAnimation.Duration.TimeSpan);
            uiElement.Visibility = Visibility.Hidden;
        }

        private async Task FadeIn(UIElement uiElement)
        {
            uiElement.Visibility = Visibility.Visible;
            uiElement.BeginAnimation(OpacityProperty, fadeInAnimation);
            await Task.Delay(fadeInAnimation.Duration.TimeSpan);
        }

        private async Task TransitionToEndScreen(string text, ImageSource winnerImage)
        {
            await Task.WhenAll(FadeOut(TurnPanel), FadeOut(GameCanvas));
            ResultText.Text = text;
            WinnerImage.Source = winnerImage;
            await FadeIn(EndScreen);
        }

        private async Task TransitionToGameScreen()
        {
            await FadeOut(EndScreen);
            Line.Visibility = Visibility.Hidden;
            await Task.WhenAll(FadeIn(TurnPanel), FadeIn(GameCanvas));
        }

        private (Point, Point) FindLinePoints(WinInfo winInfo)
        {
            double squareSize = GameGrid.Width / gameState.generic_value;
            double margin = squareSize / gameState.generic_value;

            if (winInfo.Type == WinType.Row)
            {
                double y = winInfo.Number * squareSize + margin;
                return (new Point(0, y), new Point(GameGrid.Width, y));
            }
            if (winInfo.Type == WinType.Column)
            {
                double x = winInfo.Number * squareSize + margin;
                return (new Point(x, 0), new Point(x, GameGrid.Height));
            }
            if (winInfo.Type == WinType.MainDiagonal)
            {
                return (new Point(0, 0), new Point(GameGrid.Width, GameGrid.Height));
            }

            return (new Point(GameGrid.Width, 0), new Point(0, GameGrid.Height));
        }

        private async Task ShowLine(WinInfo winInfo)
        {
            (Point start, Point end) = FindLinePoints(winInfo);

            Line.X1 = start.X;
            Line.Y1 = start.Y;

            DoubleAnimation x2Animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(.25),
                From = start.X,
                To = end.X
            };

            DoubleAnimation y2Animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(.25),
                From = start.Y,
                To = end.Y
            };

            Line.Visibility = Visibility.Visible;
            Line.BeginAnimation(Line.X2Property, x2Animation);
            Line.BeginAnimation(Line.Y2Property, y2Animation);
            await Task.Delay(x2Animation.Duration.TimeSpan);
        }

        private async void Draw_X (int r , int c)
        {
            double squareSize = GameGrid.Width / gameState.generic_value;

            Line line = new Line();

            Grounds[r, c].Children.Add(line);

            line.StrokeThickness = 4;
            line.Stroke = Brushes.Black;

            (Point start, Point end) = (new Point(0, 0), new Point(squareSize, squareSize));

            line.X1 = start.X;
            line.Y1 = start.Y;

            DoubleAnimation x2Animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(.25),
                From = start.X,
                To = end.X
            };

            DoubleAnimation y2Animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(.25),
                From = start.Y,
                To = end.Y
            };

            line.Visibility = Visibility.Visible;
            line.BeginAnimation(Line.X2Property, x2Animation);
            line.BeginAnimation(Line.Y2Property, y2Animation);

            // Delay

            await Task.Delay(250);

            Line line2 = new Line();

            Grounds[r, c].Children.Add(line2);

            line2.StrokeThickness = 4;
            line2.Stroke = Brushes.Black;



            //##############################################################################

            (Point start2, Point end2) = (new Point(squareSize, 0), new Point(0, squareSize));

            line2.X1 = start2.X;
            line2.Y1 = start2.Y;

            DoubleAnimation x3Animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(.25),
                From = start2.X,
                To = end2.X
            };

            DoubleAnimation y3Animation = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(.25),
                From = start2.Y,
                To = end2.Y
            };

            line2.Visibility = Visibility.Visible;
            line2.BeginAnimation(Line.X2Property, x3Animation);
            line2.BeginAnimation(Line.Y2Property, y3Animation);


        }

        private void Draw_O (int r, int c)
        {
            double squareSize = GameGrid.Width / gameState.generic_value;
           
            //DRAW O 

            Ellipse ellipse = new Ellipse();


            // Set the width and height of the Ellipse
            ellipse.Width = squareSize;
            ellipse.Height = squareSize;

            // Set the stroke color and thickness of the Ellipse
            ellipse.Stroke = Brushes.White;
            ellipse.StrokeThickness = 5;


            //TURTLE HERE
            Grounds[r, c].Children.Add(ellipse);
        }


        // DRAW XO HERE!!
        private async void OnMoveMade(int r, int c)
        {
            //Size of Canvas
            double squareSize = GameGrid.Width / gameState.generic_value;

            //Get player on Grid
            Player player = gameState.GameGrid[r, c];

            if (player == Player.X)
            {
                Draw_X(r,c);
            }

            else
            {
                Draw_O(r, c);
            }

            //imageControls[r, c].BeginAnimation(Image.SourceProperty, animations[player]);
            PlayerImage.Source = imageSources[gameState.CurrentPlayer];

        }

        private void LoadOnMoveMade(int r, int c, char PlayerMarked, string LoadCurrentPlayer)
        {
            if (string.Equals(LoadCurrentPlayer[0], 'x'))
            {
                gameState.CurrentPlayer = Player.X;
                PlayerImage.Source = imageSources[gameState.CurrentPlayer];


            }
            else if (string.Equals(LoadCurrentPlayer[0], 'o'))
            {
                gameState.CurrentPlayer = Player.O;
                PlayerImage.Source = imageSources[gameState.CurrentPlayer];

            }


            if (string.Equals(PlayerMarked, 'x'))
            {
                gameState.GameGrid[r, c] = Player.X;
                Player player = gameState.GameGrid[r, c];
                Draw_X(r, c);

            }
            else if (string.Equals(PlayerMarked, 'o'))
            {
                gameState.GameGrid[r, c] = Player.O;
                Player player = gameState.GameGrid[r, c];
                Draw_O(r, c);

            }
        }

        private async void OnGameEnded(GameResult gameResult)
        {
            await Task.Delay(1000);

            if (gameResult.Winner == Player.None)
            {
                await TransitionToEndScreen("It's a tie!", null);
            }
            else
            {
                await ShowLine(gameResult.WinInfo);
                await Task.Delay(1000);
                await TransitionToEndScreen("Winner:", imageSources[gameResult.Winner]);
            }
        }

        private async void OnGameRestarted()
        {
            GameGrid.Children.Clear();

            for (int r = 0; r < gameState.generic_value; r++)
            {
                for (int c = 0; c < gameState.generic_value; c++)
                {
                    Canvas playground = new Canvas();
                    GameGrid.Children.Add(playground);
                    Grounds[r, c] = playground;

                }
            }

            

            PlayerImage.Source = imageSources[gameState.CurrentPlayer];
            await TransitionToGameScreen();
        }

        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double squareSize = GameGrid.Width / gameState.generic_value;
            Point clickPosition = e.GetPosition(GameGrid);
            int row = (int)(clickPosition.Y / squareSize);
            int col = (int)(clickPosition.X / squareSize);
            gameState.MakeMove(row, col);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (gameState.GameOver)
            {
                gameState.Reset();
            }
        }
        private void SaveGame(object sender, RoutedEventArgs e)
        {

            
            string messageBoxText = "Do you want to save this game?";
            string caption = "Save game";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            Trace.WriteLine(result);

            if (string.Equals(result.ToString(), "Yes"))
            {
                gameState.SaveGame();

            }

        }

        private void LoadGame(object sender, RoutedEventArgs e)
        {


            //Load save all data 
            StreamReader reader = new StreamReader("C:\\TicTacToe\\Save.txt");

            //Load nxn_array
            string line;
            line = reader.ReadLine();
            int nxn_array = Convert.ToInt32(line);

            //Set generic_value = nxn_array
            gameState.generic_value = nxn_array;
            Trace.WriteLine("This is " + nxn_array + "X" + nxn_array + " array");
            //LoadSetupValue = nxn_array;



            //Clear GameGrid 
            imageControls = new Image[gameState.generic_value, gameState.generic_value];
            gameState.GameGrid = new Player[gameState.generic_value, gameState.generic_value];

            //Set up GameGrid

            SetupGrid();
            SetupGameGrid();

            //Load Grid Marked
            line = reader.ReadLine();
            //Trace.WriteLine(line);
            int length = line.Length; length--;
            int pos = 0; int row = 0; int col = 0;

            //Load CurrentPlayer
            string LoadCurrentPlayer;
            LoadCurrentPlayer = reader.ReadLine();
            Trace.WriteLine("CurrentPlayer is " + LoadCurrentPlayer);

            while (pos < length)
            {
                if (col == nxn_array)
                {
                    row++;
                    Trace.WriteLine("######################");
                    col = 0;
                }

                char PlayerMarked = line[pos];

                LoadOnMoveMade(row, col, PlayerMarked, LoadCurrentPlayer);

                Trace.WriteLine(line[pos] + ", " + "row = " + row + ", " + "col = " + col);

                pos++;
                col++;
            }

            //close the file
            reader.Close();
            Console.ReadLine();
        }
    }
}