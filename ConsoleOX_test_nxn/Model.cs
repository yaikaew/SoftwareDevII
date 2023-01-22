using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleOX1
{

    //keep data winplayer
    //public class WinInfo
    //{
    //    public WinType Type { get; set; }
    //    public int Number { get; set; }
    //}

    //keep GameResult

    //set player
    //public enum Player
    //{
    //    None, X, O
    //}

    //set wintype
    //public enum WinType
    //{
    //    Row, Column, MainDiagonal, AntiDiagonal
    //}

    //Model class
    public class model
    {
        //declare n
        private int boardSize;

        //public List<string> list = new List<string>();
        private string[,] GameGrid { get; set; }
        private string CurrentPlayer { get; set; }
        public int TurnsPassed { get; set; }
        public bool GameOver { get; private set; }

        //public event Action<int, int> MoveMade;
        //public event Action<GameResult> GameEnded;
        //public event Action GameRestarted;

        //start game
        public model()
        {
            GameGrid = new string[boardSize, boardSize];
            CurrentPlayer = "X";
            TurnsPassed = 0;
            GameOver = false;
        }
        public int GetBoardSize()
        {
            return boardSize ;
        }

        //Get current Turn
        public string GetCurrentTurn()
        {
            return CurrentPlayer.ToString();
        }

        public string GetBoardValue(int r , int c)
        {
            return GameGrid[r,c];
        }

        public string[,] GetBoardArray()
        {
            return GameGrid;
        }


        //public string GetBoardArrayToString()
        //{

        //    return "x";
        //}

        //check ch don't have XO ?
        private bool CanMakeMove(int r, int c)
        {
            return !GameOver && GameGrid[r, c] == "None";
        }

        //check IsGridFull ?
        private bool IsGridFull()
        {
            return TurnsPassed == (boardSize * boardSize);
        }

        private void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == "X" ? "O" : "X";
        }

        //Use for check win
        private bool AreSquaresMarked((int, int)[] squares, string player)
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
        //private bool DidMoveWin(int r, int c, out WinInfo winInfo)
        //{
        //    // Check win here !!
        //    (int, int)[] row = { };
        //    for (int i = 0; i < boardSize; i++)
        //    {
        //        row = row.Append((r, i)).ToArray();
        //    }
        //    (int, int)[] col = { };
        //    for (int i = 0; i < boardSize; i++)
        //    {
        //        col = col.Append((i, c)).ToArray();
        //    }
        //    (int, int)[] mainDiag = { };
        //    for (int i = 0; i < boardSize; i++)
        //    {
        //        mainDiag = mainDiag.Append((i, i)).ToArray();
        //    }
        //    (int, int)[] antiDiag = { };
        //    for (int i = 0; i < boardSize; i++)
        //    {
        //        antiDiag = antiDiag.Append((i, boardSize - i - 1)).ToArray();
        //    }

        //    if (AreSquaresMarked(row, CurrentPlayer))
        //    {
        //        winInfo = new WinInfo { Type = WinType.Row, Number = r };
        //        return true;
        //    }

        //    if (AreSquaresMarked(col, CurrentPlayer))
        //    {
        //        winInfo = new WinInfo { Type = WinType.Column, Number = c };
        //        return true;
        //    }

        //    if (AreSquaresMarked(mainDiag, CurrentPlayer))
        //    {
        //        winInfo = new WinInfo { Type = WinType.MainDiagonal };
        //        return true;
        //    }

        //    if (AreSquaresMarked(antiDiag, CurrentPlayer))
        //    {
        //        winInfo = new WinInfo { Type = WinType.AntiDiagonal };
        //        return true;
        //    }

        //    winInfo = null;
        //    return false;
        //}

        //condition foe end game
        /*public bool DidMoveEndGame(int r, int c, out GameResult gameResult)
        {
            if (DidMoveWin(r, c, out WinInfo winInfo))
            {
                gameResult = new GameResult { Winner = CurrentPlayer, WinInfo = winInfo };
                return true;
            }

            if (IsGridFull())
            {
                gameResult = new GameResult { Winner = "None" };
                return true;
            }

            gameResult = null;
            return false;
        }*/
        public int CheckWin(int r ,int c) // return integers -1(draw) 0(continue) 1(win row) 2(win column) 3(win diag) 4(win counterdiag)
        {
            // check row
            for(int i = 0; i < boardSize; i++)
            {
                if (GameGrid[r, i] == CurrentPlayer)
                {
                    if (i == boardSize - 1)
                    {
                        return 1;
                    }
                }
                else break;
            }
            //check column
            for(int i = 0; i< boardSize; i++)
            {
                if (GameGrid[i, c] == CurrentPlayer)
                {
                    if (i == boardSize - 1)
                    {
                        return 2;
                    }
                }
                else break;
            }
            // check diag
            for (int i = 0; i < boardSize; i++)
            {
                if (GameGrid[i, i] == CurrentPlayer)
                {
                    if (i == boardSize - 1)
                    {
                        return 3;
                    }
                }
                else break;
            }
            // check anti diag
            for (int i = 0; i < boardSize; i++)
            {
                if (GameGrid[i, boardSize - i -1] == CurrentPlayer)
                {
                    if (i == boardSize - 1)
                    {
                        return 4;
                    }
                }
                else break;
            }
            // if grid full
            if(TurnsPassed == boardSize*boardSize)
            {
                return -1;
            }
            return 0;
        }

        //use when player click
        public void MakeMove(int r, int c)
        {
            /*if (!CanMakeMove(r, c))
            {
                return;
            }*/
            //check if can make move

            if (GameGrid[r, c] == null)
            {
                GameGrid[r, c] = CurrentPlayer;
                TurnsPassed++;
                if (CheckWin(r, c) == 0)
                {
                    SwitchPlayer();
                }
                else return;
                
                //CheckWin(r, c); // get -1 0 1 2 3 4
            }
            else // already have marked
            {
                return;
            }
            
            /*
            GameGrid[r, c] = CurrentPlayer;
            TurnsPassed++;*/
            /*
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
            }*/
        }

        private void LoadGrid(int r, int c, char PlayerMarked, string LoadCurrentPlayer)
        {
            if (string.Equals(LoadCurrentPlayer[0], 'x'))
            {
                CurrentPlayer = "X";

            }
            else if (string.Equals(LoadCurrentPlayer[0], 'o'))
            {
                CurrentPlayer = "O";
            }

            if (string.Equals(PlayerMarked, 'x'))
            {
                GameGrid[r, c] = "X";
                //Player player = GameGrid[r, c];

            }
            else if (string.Equals(PlayerMarked, 'o'))
            {
                GameGrid[r, c] = "O";
                //Player player = GameGrid[r, c];

            }
        }

        public void NewGame(int size)
        {
            boardSize = size;
            GameGrid = new string[size, size];
            CurrentPlayer = "X";
            TurnsPassed = 0;
            GameOver = false;
            //GameRestarted?.Invoke();
        }

        public void SaveGame(string path)
        {
            // create file save all data
            FileStream SaveGame = new FileStream(path, FileMode.Create);

            StreamWriter writer_SaveGame = new StreamWriter(SaveGame);

            int turn_passed = 0;

            //Save nxn grid 
            writer_SaveGame.WriteLine(boardSize);
                
            //Save grid that PlayersMarked
            for (int i = 0; i < (boardSize); i++)
            {
                for (int j = 0; j < (boardSize); j++)
                {   
                    if (GameGrid[i, j] != null)
                    {
                        writer_SaveGame.Write(GameGrid[i, j].ToLower());

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
            writer_SaveGame.WriteLine(CurrentPlayer.ToLower());

            //Save Turn_passed
            writer_SaveGame.WriteLine(turn_passed);

            writer_SaveGame.Close();
            SaveGame.Close();
        }
        public void LoadGame(string path)
        {

            //Load save all data 
            StreamReader reader = new StreamReader(path);
            //Load nxn_array
            string line;
            line = reader.ReadLine();
            int nxn_array = Convert.ToInt32(line);

            //Set generic_value = nxn_array
            boardSize = nxn_array;
            Trace.WriteLine("This is " + nxn_array + "X" + nxn_array + " array");
            
            NewGame(boardSize);

            //Load Grid Marked
            line = reader.ReadLine();
            int length = line.Length; 
            length--;
            int pos = 0; 
            int row = 0; 
            int col = 0;

            //Load CurrentPlayer
            CurrentPlayer = reader.ReadLine().ToUpper();
            Console.WriteLine("CurrentPlayer is " + CurrentPlayer);
            //CurrentPlayer.ToUpper();
            //Console.WriteLine("CurrentPlayer after using ToUpper " + CurrentPlayer);
            TurnsPassed = Int32.Parse(reader.ReadLine());
            //Console.WriteLine("Current player is " + CurrentPlayer);
            //view.Board(LoadCurrentPlayer);
            //pos start at 0
            for ( row = 0; row < boardSize ;row++)
            {
                for ( col = 0; col < boardSize; col++)
                {
                    char PlayerMarked = line[pos];
                    if (PlayerMarked.ToString() != "n")
                    {
                        GameGrid[row,col] = PlayerMarked.ToString().ToUpper();
                        
                        //TurnsPassed++;
                    }
                    else
                    {
                        GameGrid[row,col] = null ;
                        
                    }

                    //LoadGrid(row, col, PlayerMarked, LoadCurrentPlayer);
                    pos++;
                }
            }
            for (row = 0; row < boardSize; row++)
            {
                for (col = 0; col < boardSize; col++)
                {
                    Console.Write(GameGrid[row, col] + " ");
                }
                Console.WriteLine("   new line");
            }
            Console.WriteLine("TurnsPassed : " + TurnsPassed);
            Console.WriteLine("CurrentPlayer : "+ CurrentPlayer);

            /*while (pos < length)
            {
                if (col == nxn_array)
                {
                    row++;
                    col = 0;
                }

                char PlayerMarked = line[pos];
                if (PlayerMarked.ToString() != "n")
                {
                    GameGrid[pos + 1] = PlayerMarked.ToString().ToUpper();
                    TurnsPassed++;
                }
                else
                {
                    list[pos + 1] = (pos + 1).ToString();
                }

                LoadGrid(row, col, PlayerMarked, LoadCurrentPlayer);

                pos++;
                col++;

            }*/

            //close the file
            reader.Close();

            //View.Board(CurrentPlayer.ToString(), generic_value);
        }
    }
}
