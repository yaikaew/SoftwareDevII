using System;
using System.Threading;
using TicTacToe;

namespace TIC_TAC_TOE
{
    class view
    {
        public static string[] arr = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        public static void Board(string CurrentPlayer)
        {
            Console.Clear();// whenever loop will be again start then screen will be clear
            Console.WriteLine("Player1:X and Player2:O");
            Console.WriteLine("\n");
            


            if (CurrentPlayer == "X")//checking the chance of the player
            {
                Console.WriteLine("Player X Chance");

            }
            else
            {
                Console.WriteLine("Player O Chance");

            }

            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", arr[1], arr[2], arr[3]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", arr[4], arr[5], arr[6]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}  |  {1}  |  {2}", arr[7], arr[8], arr[9]);
            Console.WriteLine("     |     |      ");
        }

        public static void DisplayResult(string winner)
        {
            Console.WriteLine("\n");

            if (winner != "None")
            {
                Console.WriteLine("WINNER IS " + "PLAYER " + winner);
            }

            else
            {
                Console.WriteLine("DRAW");
            }
        }
    }
}
