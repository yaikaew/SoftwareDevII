﻿using System;
using System.IO;
using System.Linq;

namespace TicTacToe
{
    public class GameState
    {
        //public MainWindow mmainWindow = new MainWindow();

        public int generic_value = 100;

        public Player[,] GameGrid { get; set; }
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

        //private void SaveGame()
        //{
        //    // Set the file path and name for the saved game
        //    string filePath = "C:\TicTacToe\savedGame.txt";
        //    // Open the file for writing
        //    StreamWriter writer = new StreamWriter(filePath);

        //    foreach ((int r, int c) in GameGrid)
        //    {
        //        writer.Write("test" + " ");
        //        Console.WriteLine("Game has been saved");
        //    }

        //    writer.Close();
        //}


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
            /*(int, int)[] row = new[] { (r, 0), (r, 1), (r, 2) };
            (int, int)[] col = new[] { (0, c), (1, c), (2, c) };
            (int, int)[] mainDiag = new[] { (0, 0), (1, 1), (2, 2) };
            (int, int)[] antiDiag = new[] { (0, 2), (1, 1), (2, 0) };*/

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
    }
}