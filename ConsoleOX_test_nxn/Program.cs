using ConsoleOX1;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace ConsoleOX1
{
    class Program
    {
        //Move in to main function
        //private static model gamestate = new model();
        //private static GameResult gameresult = new GameResult();
        //private static WinInfo wininfo = new WinInfo();
        //private static view View = new view();
        //int row = 0, col = 0;

        //Change number in grid to row and column

        public static void Main(string[] args)
        {
            model gamestate = new model();
            GameResult gameresult = new GameResult();
            WinInfo wininfo = new WinInfo();
            view View = new view();

            int row = 0, col = 0;

            void DigitToArray(int Digit)
            {
                Digit--;
                row = Digit / gamestate.generic_value;// generic value
                col = Digit % gamestate.generic_value;// generic value
            }

            //recieve input from user
            Console.Write("EnterTableSize : ");
            string generic = Console.ReadLine();
            Int32.TryParse(generic,out gamestate.generic_value);

            //set gamegrid of table
            //recommend to use method reset
            //gamestate.GameGrid = new Player[gamestate.generic_value, gamestate.generic_value];
            gamestate.Reset();

            //create list
            for (int i = 0; i < gamestate.generic_value * gamestate.generic_value + 1; i++)
            {
                gamestate.list.Add(i.ToString());
                Trace.WriteLine(gamestate.list[i]);
            }

            do
            {

                View.Board(gamestate.CurrentPlayer.ToString(), gamestate.generic_value, gamestate.list);// calling the board Function

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

                //check on gamegrid playermarked and add into list
                if (gamestate.GameGrid[row, col] == Player.X) 
                {

                    gamestate.list[number] = "X";

                }
                else
                {
                    gamestate.list[number] = "O";
                }
            }

            while (!gamestate.DidMoveEndGame(row, col, out gameresult));

            Console.Clear();// clearing the console
            View.Board(gamestate.CurrentPlayer.ToString(), gamestate.generic_value, gamestate.list);// getting filled board again
            View.DisplayResult(gameresult.Winner.ToString(),gameresult.WinInfo.Type.ToString());//display result of winner
        }
    }
}
