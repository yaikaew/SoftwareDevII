﻿using ConsoleOX1;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace ConsoleOX1
{
    class Program
    {
        public static model gamestate = new model();
        public static GameResult gameresult = new GameResult();
        static int row = 0, col = 0;

        //Change number in grid to row and column
        public static void DigitToArray(int Digit)
        {
            Digit--;
            row = Digit / gamestate.generic_value;// generic value
            col = Digit % gamestate.generic_value;// generic value
        }
        public static void Main(string[] args)
        {
            do
            {

                view.Board(gamestate.CurrentPlayer.ToString());// calling the board Function

                string input = Console.ReadLine();
                int number ;
                bool success = Int32.TryParse(input, out number);
                if (success)
                {
                    // Input can be parsed to an int.
                    // number contains the int value of the input.
                    DigitToArray(number);
                    gamestate.MakeMove(row, col);
                }
                else
                {
                    // Input cannot be parsed to an int.
                    Console.WriteLine(input);
                    if (input == "save")
                    {
                        gamestate.SaveGame();
                    }
                    else if (input == "load")
                    {
                        gamestate.LoadGame();
                    }
                }

                if (gamestate.GameGrid[row, col] == Player.X) //if chance is of player 2 then mark O else mark X
                {
                    view.list[number] = "X";
                }
                else
                {
                    view.list[number] = "O";
                }
            }

            while (!gamestate.DidMoveEndGame(row, col, out gameresult));
            Console.Clear();// clearing the console
            view.Board(gamestate.CurrentPlayer.ToString());// getting filled board again
            view.DisplayResult(gameresult.Winner.ToString());
        }
    }
}
