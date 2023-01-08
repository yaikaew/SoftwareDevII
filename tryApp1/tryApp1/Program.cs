using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using TicTacToe;

namespace TIC_TAC_TOE
{
    class Program
    {
        public static model gamestate = new model();
        public static GameResult gameresult = new GameResult();

        //making array and
        //by default I am providing 0-9 where no use of zero
        static char[] arr = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        static int player = 1; //By default player 1 is set
        static int choice; //This holds the choice at which position user want to mark
        static string order; 
        // The flag variable checks who has won if it's value is 1 then someone has won the match
        //if -1 then Match has Draw if 0 then match is still running
        static int flag = 0;
        static int row = 0 , col = 0;

        //Change number in grid to row and column
        public static void DigitToArray(int Digit)
            {
                Digit --;
                row = Digit / 3;
                col = Digit % 3;
            }
        public static void Main(string[] args)
        {
            do
            {
 
                view.Board(gamestate.CurrentPlayer.ToString());// calling the board Function


                string input = Console.ReadLine();
                int number;
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

                //choice = int.Parse(Console.ReadLine());//Taking users choice

                // checking that position where user want to run is marked (with X or O) or not

                

                if (gamestate.GameGrid[row, col] == Player.X) //if chance is of player 2 then mark O else mark X
                {
                    view.arr[number] = "X";

                }
                else
                {
                    view.arr[number] = "O";

                }

                //else
                ////If there is any possition where user want to run
                ////and that is already marked then show message and load board again
                //{
                //    Console.WriteLine("Sorry the row {0} is already marked with {1}", choice, arr[choice]);
                //    Console.WriteLine("\n");
                //    Console.WriteLine("Please wait 2 second board is loading again.....");
                //    Thread.Sleep(2000);
                //}
                //flag = CheckWin();// calling of check win
            }
            while (!gamestate.DidMoveEndGame(row, col,out gameresult)) ; 
            // This loop will be run until all cell of the grid is not marked
            //with X and O or some player is not win
            Console.Clear();// clearing the console
            view.Board(gamestate.CurrentPlayer.ToString());// getting filled board again
            view.DisplayResult(gameresult.Winner.ToString());
            //if (flag == 1)
            //// if flag value is 1 then someone has win or
            ////means who played marked last time which has win
            //{
            //    Console.WriteLine("Player {0} has won", (player % 2) + 1);
            //}
            //else// if flag value is -1 the match will be draw and no one is winner
            //{
            //    Console.WriteLine("Draw");
            //}
            //Console.ReadLine();
        }
        // Board method which creats board


        //private static void Board()
        //{
        //    Console.WriteLine("     |     |      ");
        //    Console.WriteLine("  {0}  |  {1}  |  {2}", arr[1], arr[2], arr[3]);
        //    Console.WriteLine("_____|_____|_____ ");
        //    Console.WriteLine("     |     |      ");
        //    Console.WriteLine("  {0}  |  {1}  |  {2}", arr[4], arr[5], arr[6]);
        //    Console.WriteLine("_____|_____|_____ ");
        //    Console.WriteLine("     |     |      ");
        //    Console.WriteLine("  {0}  |  {1}  |  {2}", arr[7], arr[8], arr[9]);
        //    Console.WriteLine("     |     |      ");
        //}


        //Checking that any player has won or not
        //private static int CheckWin()
        //{
        //    #region Horzontal Winning Condtion
        //    //Winning Condition For First Row
        //    if (arr[1] == arr[2] && arr[2] == arr[3])
        //    {
        //        return 1;
        //    }
        //    //Winning Condition For Second Row
        //    else if (arr[4] == arr[5] && arr[5] == arr[6])
        //    {
        //        return 1;
        //    }
        //    //Winning Condition For Third Row
        //    else if (arr[6] == arr[7] && arr[7] == arr[8])
        //    {
        //        return 1;
        //    }
        //    #endregion
        //    #region vertical Winning Condtion
        //    //Winning Condition For First Column
        //    else if (arr[1] == arr[4] && arr[4] == arr[7])
        //    {
        //        return 1;
        //    }
        //    //Winning Condition For Second Column
        //    else if (arr[2] == arr[5] && arr[5] == arr[8])
        //    {
        //        return 1;
        //    }
        //    //Winning Condition For Third Column
        //    else if (arr[3] == arr[6] && arr[6] == arr[9])
        //    {
        //        return 1;
        //    }
        //    #endregion
        //    #region Diagonal Winning Condition
        //    else if (arr[1] == arr[5] && arr[5] == arr[9])
        //    {
        //        return 1;
        //    }
        //    else if (arr[3] == arr[5] && arr[5] == arr[7])
        //    {
        //        return 1;
        //    }
        //    #endregion
        //    #region Checking For Draw
        //    // If all the cells or values filled with X or O then any player has won the match
        //    else if (arr[1] != '1' && arr[2] != '2' && arr[3] != '3' && arr[4] != '4' && arr[5] != '5' && arr[6] != '6' && arr[7] != '7' && arr[8] != '8' && arr[9] != '9')
        //    {
        //        return -1;
        //    }
        //    #endregion
        //    else
        //    {
        //        return 0;
        //    }
        //}



    }
}
