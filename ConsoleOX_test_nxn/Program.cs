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
        //public List<string> list = new List<string>();
        //int row = 0, col = 0;

        //Change number in grid to row and column

        public static void Main(string[] args)
        {
            model gamestate = new model();
            //GameResult gameresult = new GameResult();
            //WinInfo wininfo = new WinInfo();
            view View = new view();
            List<string> Gameboard = gamestate.GetBoardArray().Cast<string>().ToList();

            int row = 0, col = 0;

            void DigitToIndex(int Digit)
            {
                Digit--;
                row = Digit / gamestate.GetBoardSize();// generic value
                col = Digit % gamestate.GetBoardSize();// generic value
            }

            //recieve input from user
            Console.Write("EnterTableSize : ");
            string generic = Console.ReadLine();
            Int32.TryParse(generic,out int size);

            //set gamegrid of table
            //recommend to use method reset
            //Reset to set generic value
            gamestate.NewGame(size);

            //create list
            for (int i = 0; i < gamestate.GetBoardSize() * gamestate.GetBoardSize() + 1; i++)
            {

                Gameboard.Add(i.ToString());
               
            }

            do
            {

                View.Board(gamestate.GetCurrentTurn(), gamestate.GetBoardSize(), gamestate.GetBoardArray());// calling the board Function

                string input = Console.ReadLine();
                int number ;
                bool success = Int32.TryParse(input, out number);

                if (success)
                {
                    // Input can be parsed to an int.
                    // number contains the int value of the input.
                    DigitToIndex(number);
                    gamestate.MakeMove(row, col);
                    //check on gamegrid playermarked and add into gametable
                    

                }
                else
                {
                    // Input cannot be parsed to an int.
                    Console.WriteLine(input);
                    if (input == "save")
                    {
                        string path = Directory.GetCurrentDirectory();
                        path += "\\Save.txt";
                        gamestate.SaveGame(path);
  
                    }
                    else if (input == "load")
                    {
                        string path = Directory.GetCurrentDirectory();
                        path += "\\Save.txt";
                        gamestate.LoadGame(path);
                        Gameboard = gamestate.GetBoardArray().Cast<string>().ToList();
                        Trace.WriteLine(Gameboard);

                    }
                }

                //if (gamestate.GetBoardValue(row, col) == "X")
                //{

                //    Gameboard[number] = "X";

                //}
                //else
                //{
                //    Gameboard[number] = "O";
                //}

                //check on gamegrid playermarked and add into gametable
                /*if (gamestate.GetBoardValue(row, col) == "X")
                {

                    Gameboard[number] = "X";

                }
                else
                {
                    Gameboard[number] = "O";
                }*/

            }

            while (gamestate.CheckWin(row,col) == 0);

            Console.Clear();// clearing the console
            View.Board(gamestate.GetCurrentTurn() , gamestate.GetBoardSize(), gamestate.GetBoardArray());// getting filled board again
            View.DisplayResult(gamestate.CheckWin(row,col),gamestate.GetCurrentTurn());//display result of winner
        }
    }
}
