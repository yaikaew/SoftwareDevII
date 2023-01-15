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
using System.Reflection;
using static System.Windows.Forms.LinkLabel;
using TicTacToe3;

namespace TicTacToe
{
    public partial class MainWindow 
    {
        //Declare var
        public string LoadCurrentPlayer;
        public string TurnPassed;

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


        //Declare canvas name Grounds
        private Canvas[,] Grounds;

        //calling Model(GameState) in name is gameState
        public GameState gameState = new GameState();
  

        public MainWindow()
        {
            //start game
            Grounds = new Canvas[gameState.generic_value, gameState.generic_value];

            InitializeComponent();
            
            gameState.MoveMade += OnMoveMade;
            gameState.GameEnded += OnGameEnded;
            gameState.GameRestarted += OnGameRestarted;

            Draw_GridLine();
            SetupGameGrid();
            X_drawCurrent();





        }
        //View
        //Draw x for show
        private void X_drawCurrent()
        {
            //line 1
            Line line1 = new Line();

            line1.X1 = 0;
            line1.Y1 = 0;
            line1.X2 = 50;
            line1.Y2 = 50;


            //line 2
            Line line2 = new Line();

            line2.X1 = 50;
            line2.Y1 = 0;
            line2.X2 = 0;
            line2.Y2 = 50;

            // Create a red Brush  
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;

            // Set Line's width and color  
            line1.StrokeThickness = 4;
            line1.Stroke = blackBrush;
            line2.StrokeThickness = 4;
            line2.Stroke = blackBrush;

            Current.Children.Clear();
            Current.Children.Add(line1);
            Current.Children.Add(line2);
        }
        //View
        private void X_drawWinner( )
        {
            //line 1
            Line line1 = new Line();

            line1.X1 = 0;
            line1.Y1 = 0;
            line1.X2 = 70;
            line1.Y2 = 70;


            //line 2
            Line line2 = new Line();

            line2.X1 = 70;
            line2.Y1 = 0;
            line2.X2 = 0;
            line2.Y2 = 70;

            // Create a red Brush  
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;

            // Set Line's width and color  
            line1.StrokeThickness = 4;
            line1.Stroke = blackBrush;
            line2.StrokeThickness = 4;
            line2.Stroke = blackBrush;

            Winner.Children.Clear();
            Winner.Children.Add(line1);
            Winner.Children.Add(line2);
        }
        //View
        private void O_drawCurrent()
        {
            Ellipse circle = new Ellipse()
            {
                Width = 50,
                Height = 50,
                Stroke = Brushes.White,
                StrokeThickness = 4
            };
            Current.Children.Clear();
            Current.Children.Add(circle);

        }
        //View
        private void O_drawWinner()
        {
            Ellipse circle = new Ellipse()
            {
                Width = 70,
                Height = 70,
                Stroke = Brushes.Black,
                StrokeThickness = 4
            };
            Winner.Children.Clear();
            Winner.Children.Add(circle);

        }
        //View
        //made gridline for ui
        public void Draw_GridLine()
        {
            TheGrid.Children.Clear();
            double SquareSize = TheGrid.Width / gameState.generic_value ;
            for (int r = 1; r < gameState.generic_value; r++)
            {
                Line column_line = new Line()
                {
                    X1 = r*SquareSize,
                    Y1 = 0,
                    X2 = r*SquareSize,
                    Y2 = TheGrid.Width,
                    Stroke = Brushes.Black,
                    StrokeThickness = 3
                };

                Line row_line = new Line()
                {
                    X1 = 0,
                    Y1 = r * SquareSize,
                    X2 = TheGrid.Width,
                    Y2 = r * SquareSize,
                    Stroke = Brushes.Black,
                    StrokeThickness = 3
                };

                TheGrid.Children.Add(column_line);
                TheGrid.Children.Add(row_line);

            }
        }
        //View
        //made gridline for calculate
        public void SetupGameGrid()
        {
            GameGrid.Children.Clear();
            Grounds = new Canvas[gameState.generic_value, gameState.generic_value];

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
        //View
        //fadeout for element to change page 
        private async Task FadeOut(UIElement uiElement)
        {
            uiElement.BeginAnimation(OpacityProperty, fadeOutAnimation);
            await Task.Delay(fadeOutAnimation.Duration.TimeSpan);
            uiElement.Visibility = Visibility.Hidden;
        }
        //View
        //fadein for element to change page 
        private async Task FadeIn(UIElement uiElement)
        {
            uiElement.Visibility = Visibility.Visible;
            uiElement.BeginAnimation(OpacityProperty, fadeInAnimation);
            await Task.Delay(fadeInAnimation.Duration.TimeSpan);
        }
        //View
        //change page when end game calling method fadeout/in
        private async Task TransitionToEndScreen(string text)
        {
            await Task.WhenAll(FadeOut(TurnPanel), FadeOut(GameCanvas));
            ResultText.Text = text;
            //WinnerImage.Source = winnerImage;
            await FadeIn(EndScreen);
        }
        //View
        //change page when start game calling method fadeout/in
        private async Task TransitionToGameScreen()
        {
            await FadeOut(EndScreen);
            Line.Visibility = Visibility.Hidden;
            await Task.WhenAll(FadeIn(TurnPanel), FadeIn(GameCanvas));
        }
        //View
        //find point win
        private (Point, Point) FindLinePoints(WinInfo winInfo)
        {
            double squareSize = GameGrid.Width / gameState.generic_value;
            double margin = squareSize / 2;

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
        //View
        //showline by findpoint
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
        //View
        //Draw X
        private async void Draw_X (int r , int c)
        {
            double squareSize = GameGrid.Width / gameState.generic_value;

            Line line = new Line();

            Grounds[r, c].Children.Add(line);

            line.StrokeThickness = 4;
            line.Stroke = Brushes.Black;

            //line 1
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

            line.BeginAnimation(Line.X2Property, x2Animation);
            line.BeginAnimation(Line.Y2Property, y2Animation);

            // Delay

            await Task.Delay(250);

            Line line2 = new Line();

            Grounds[r, c].Children.Add(line2);

            line2.StrokeThickness = 4;
            line2.Stroke = Brushes.Black;



            //line 2##############################################################################

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

            line2.BeginAnimation(Line.X2Property, x3Animation);
            line2.BeginAnimation(Line.Y2Property, y3Animation);


        }
        //View
        // Draw O
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

        //View
        // DRAW XO HERE when player click!!
        private async void OnMoveMade(int r, int c)
        {
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

            if (gameState.CurrentPlayer == Player.X)
            {
                X_drawCurrent();
            }
            else
            {
                O_drawCurrent();
            }

        }

        //View
        //DRAW XO HERE when Load 
        public void LoadOnMoveMade(int r, int c, char PlayerMarked, string LoadCurrentPlayer)
        {
            
            if (string.Equals(LoadCurrentPlayer[0], 'x'))
            {
                gameState.CurrentPlayer = Player.X;
                X_drawCurrent();
                //PlayerImage.Source = imageSources[gameState.CurrentPlayer];


            }
            else if (string.Equals(LoadCurrentPlayer[0], 'o'))
            {
                gameState.CurrentPlayer = Player.O;
                O_drawCurrent();
                //PlayerImage.Source = imageSources[gameState.CurrentPlayer];

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
        //View
        //when game end
        private async void OnGameEnded(GameResult gameResult)
        {
            await Task.Delay(1000);
            Current.Children.Clear();
            if (gameResult.Winner == Player.None)
            {
                await TransitionToEndScreen("It's a tie!");
            }
            else
            {
                await ShowLine(gameResult.WinInfo);
                await Task.Delay(1000);

                if (gameResult.Winner == Player.X)
                {
                    X_drawWinner();
                }
                else
                {
                    O_drawWinner();
                }

                await TransitionToEndScreen("Winner:");
            }
        }
        //View
        //when restart will reset
        private async void OnGameRestarted()
        {
            GameGrid.Children.Clear();
            Grounds = new Canvas[gameState.generic_value, gameState.generic_value];
            for (int r = 0; r < gameState.generic_value; r++)
            {
                for (int c = 0; c < gameState.generic_value; c++)
                {
                    Canvas playground = new Canvas();
                    GameGrid.Children.Add(playground);
                    Grounds[r, c] = playground;

                }
            }

            if (gameState.CurrentPlayer == Player.X)
            {
                X_drawCurrent();
            }
            else
            {
                O_drawCurrent();
            }

            await TransitionToGameScreen();
        }

        //View
        //whenplayer click then get position row column and order to make X O
        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double squareSize = GameGrid.Width / gameState.generic_value;
            Point clickPosition = e.GetPosition(GameGrid);
            int row = (int)(clickPosition.Y / squareSize);
            int col = (int)(clickPosition.X / squareSize);
            gameState.MakeMove(row, col);
        }

        //Controller
        //Button playagain
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (gameState.GameOver)
            {
                gameState.Reset();
            }
        }

        //Controller
        // Savegame call model(gamestate)
        private void SaveGame(object sender, RoutedEventArgs e)
        {

            string messageBoxText = "Do you want to save this game?";
            string caption = "Save game";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (string.Equals(result.ToString(), "Yes"))
            {
                gameState.SaveGame();

            }

        }

        //Controller
        //Load game all
        private void LoadGame(object sender, RoutedEventArgs e)
        {

            gameState.LoadGame(out string gamegrid, out string loadcurrentplayer);
            int length = gamegrid.Length; length--;
            int pos = 0; int row = 0; int col = 0;

            Draw_GridLine();
            SetupGameGrid();

            while (pos < length)
            {
                if (col == gameState.generic_value)
                {
                    row++;
                    Trace.WriteLine("######################");
                    col = 0;
                }

                char PlayerMarked = gamegrid[pos];

                LoadOnMoveMade(row, col, PlayerMarked, loadcurrentplayer);

                pos++;
                col++;
            }

        }

        //button for get n grid
        private void Button_getn(object sender, RoutedEventArgs e)
        {

            if (int.TryParse(TextInput.Text, out int value))
            {
                // Use the integer value here
                gameState.generic_value = value;
                Draw_GridLine();
                gameState.Reset();
            }
            else
            {
                // Show an error message or do something else if the text is not a valid integer
                MessageBox.Show("The text is not a valid integer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }

      
    }
}