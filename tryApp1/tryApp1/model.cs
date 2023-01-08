using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using TIC_TAC_TOE;
//using System.Windows.Controls;


namespace TicTacToe
{
    //keep data winplayer
    public class WinInfo
    {
        public WinType Type { get; set; }
        public int Number { get; set; }
    }

    //keep GameResult
    public class GameResult
    {
        public Player Winner { get; set; }
        public WinInfo WinInfo { get; set; }
    }

    //set player
    public enum Player
    {
        None, X, O
    }

    //set wintype
    public enum WinType
    {
        Row, Column, MainDiagonal, AntiDiagonal
    }

    //Model class
    public class model
    {
        //declare n
        public int generic_value = 3  ;
      
        public Player[,] GameGrid { get;  set; }

        public Player CurrentPlayer { get; set; }
        public int TurnsPassed { get; set; }
        public bool GameOver { get; private set; }
        public event Action<int, int> MoveMade;
        public event Action<GameResult> GameEnded;
        public event Action GameRestarted;

        //start game
        public  model()
        {
            GameGrid = new Player[generic_value, generic_value];
            CurrentPlayer = Player.X;
            TurnsPassed = 0;
            GameOver = false;

        }

        //check ch don't have XO ?
        private bool CanMakeMove(int r, int c)
        {
            return !GameOver && GameGrid[r, c] == Player.None;
        }

        //check IsGridFull ?
        private bool IsGridFull()
        {
            return TurnsPassed == (generic_value * generic_value);
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == Player.X ? Player.O : Player.X;
        }

        //Use for check win
        private bool AreSquaresMarked((int, int)[] squares, Player player)
        {

            foreach ((int r, int c) in squares)
            {
                if (GameGrid[r, c] != player)
                {
                    return false;
                }
            }

            return true;
        }

        //checkwin
        public bool DidMoveWin(int r, int c, out WinInfo winInfo)
        {
            // Check win here !!
            (int, int)[] row = { };
            for (int i = 0; i < generic_value; i++)
            {
                row = row.Append((r, i)).ToArray();
            }
            (int, int)[] col = { };
            for (int i = 0; i < generic_value; i++)
            {
                col = col.Append((i, c)).ToArray();
            }
            (int, int)[] mainDiag = { };
            for (int i = 0; i < generic_value; i++)
            {
                mainDiag = mainDiag.Append((i, i)).ToArray();
            }
            (int, int)[] antiDiag = { };
            for (int i = 0; i < generic_value; i++)
            {
                antiDiag = antiDiag.Append((i, generic_value - i - 1)).ToArray();
            }

            if (AreSquaresMarked(row, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.Row, Number = r };
                return true;
            }

            if (AreSquaresMarked(col, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.Column, Number = c };
                return true;
            }

            if (AreSquaresMarked(mainDiag, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.MainDiagonal };
                return true;
            }

            if (AreSquaresMarked(antiDiag, CurrentPlayer))
            {
                winInfo = new WinInfo { Type = WinType.AntiDiagonal };
                return true;
            }

            winInfo = null;
            return false;
        }

        //condition foe end game
        public bool DidMoveEndGame(int r, int c, out GameResult gameResult)
        {
            if (DidMoveWin(r, c, out WinInfo winInfo))
            {
                gameResult = new GameResult { Winner = CurrentPlayer, WinInfo = winInfo };
                return true;
            }

            if (IsGridFull())
            {
                gameResult = new GameResult { Winner = Player.None };
                return true;
            }

            gameResult = null;
            return false;
        }

        //use when player click
        public void MakeMove(int r, int c)
        {
            if (!CanMakeMove(r, c))
            {
                return;
            }

            GameGrid[r, c] = CurrentPlayer;
            TurnsPassed++;

            if (DidMoveEndGame(r, c, out GameResult gameResult))
            {
                GameOver = true;
                MoveMade?.Invoke(r, c);
                GameEnded?.Invoke(gameResult);
            }
            else
            {
                SwitchPlayer();
                MoveMade?.Invoke(r, c);
            }
        }

        private void LoadOnMoveMade(int r, int c, char PlayerMarked, string LoadCurrentPlayer)
        {
            if (string.Equals(LoadCurrentPlayer[0], 'x'))
            {
                CurrentPlayer = Player.X;
                Trace.WriteLine("t");
                //PlayerImage.Source = imageSources[gameState.CurrentPlayer];


            }
            else if (string.Equals(LoadCurrentPlayer[0], 'o'))
            {
                CurrentPlayer = Player.O;
                //PlayerImage.Source = imageSources[gameState.CurrentPlayer];
                Trace.WriteLine("UUUU");
            }


            if (string.Equals(PlayerMarked, 'x'))
            {
                GameGrid[r, c] = Player.X;
                Player player = GameGrid[r, c];
                //Draw_X(r, c);

            }
            else if (string.Equals(PlayerMarked, 'o'))
            {
                GameGrid[r, c] = Player.O;
                Player player = GameGrid[r, c];
                //Draw_O(r, c);

            }
        }

        public void Reset()
        {
   
            GameGrid = new Player[generic_value, generic_value];
            CurrentPlayer = Player.X;
            TurnsPassed = 0;
            GameOver = false;
            GameRestarted?.Invoke();
        }

        public void SaveGame()
        {
            // create file save all data
            FileStream SaveGame = new FileStream("C:\\TicTacToe\\save.txt", FileMode.Create);

            StreamWriter writer_SaveGame = new StreamWriter(SaveGame);

            int turn_passed = 0;

            //Save nxn grid 
            writer_SaveGame.WriteLine(generic_value);

            //Save grid that PlayersMarked
            for (int i = 0; i < (generic_value); i++)
            {
                for (int j = 0; j < (generic_value); j++)
                {
                    if (GameGrid[i, j] != Player.None)
                    {
                        writer_SaveGame.Write(GameGrid[i, j].ToString().ToLower());

                        turn_passed++;
                    }

                    else
                    {
                        writer_SaveGame.Write("n");
                    }
                }
            }

            //Save Current_Players
            writer_SaveGame.WriteLine(" ");
            writer_SaveGame.WriteLine(CurrentPlayer.ToString().ToLower());

            //Save Turn_passed
            writer_SaveGame.WriteLine(turn_passed);

            writer_SaveGame.Close();
            SaveGame.Close();
        }
        public void LoadGame()
        {


            //Load save all data 
            StreamReader reader = new StreamReader("C:\\TicTacToe\\Save.txt");

            //Load nxn_array
            string line;
            line = reader.ReadLine();
            int nxn_array = Convert.ToInt32(line);

            //Set generic_value = nxn_array
            generic_value = nxn_array;
            Trace.WriteLine("This is " + nxn_array + "X" + nxn_array + " array");
            //LoadSetupValue = nxn_array;



            //Clear GameGrid 
            GameGrid = new Player[generic_value, generic_value];

            //Set up GameGrid

            //SetupGrid();
            //SetupGameGrid();

            //Load Grid Marked
            line = reader.ReadLine();
            //Trace.WriteLine(line);
            int length = line.Length; length--;
            int pos = 0; int row = 0; int col = 0;

            //Load CurrentPlayer
            string LoadCurrentPlayer;
            LoadCurrentPlayer = reader.ReadLine();
            //view.Board(LoadCurrentPlayer);
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
                if (PlayerMarked.ToString() != "n")
                {
                    view.arr[pos+1] = PlayerMarked.ToString().ToUpper();
                    TurnsPassed++;
                }
                else
                {
                    view.arr[pos+1] = (pos+1).ToString();
                }


                LoadOnMoveMade(row, col, PlayerMarked, LoadCurrentPlayer);
               
                //Trace.WriteLine(line[pos] + ", " + "row = " + row + ", " + "col = " + col);
                pos++;
                col++;
                


            }

            //close the file
            reader.Close();
            view.Board(CurrentPlayer.ToString());
            Trace.WriteLine(CurrentPlayer.ToString());
            //Console.ReadLine();



        }
    }
}