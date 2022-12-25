using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;




namespace TicTacToe
{
    public class WinInfo
    {
        public WinType Type { get; set; }
        public int Number { get; set; }
    }


    public class GameResult
    {
        public Player Winner { get; set; }
        public WinInfo WinInfo { get; set; }
    }

    public enum Player
    {
        None, X, O
    }
    
    public class GameState
    {

        public int generic_value = 3;
        
        public Player[,] GameGrid { get;  set; }

        public Player CurrentPlayer { get; set; }
        public int TurnsPassed { get; set; }
        public bool GameOver { get; private set; }
        public event Action<int, int> MoveMade;
        public event Action<GameResult> GameEnded;
        public event Action GameRestarted;


        public GameState()
        {
            GameGrid = new Player[generic_value, generic_value];
            CurrentPlayer = Player.X;
            TurnsPassed = 0;
            GameOver = false;
        }

        private bool CanMakeMove(int r, int c)
        {
            return !GameOver && GameGrid[r, c] == Player.None;
        }

        private bool IsGridFull()
        {
            return TurnsPassed == (generic_value * generic_value);
        }

        private void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == Player.X ? Player.O : Player.X;
        }

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


        private bool DidMoveWin(int r, int c, out WinInfo winInfo)
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

        private bool DidMoveEndGame(int r, int c, out GameResult gameResult)
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
   

    }
}