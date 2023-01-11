using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleOX1
{
    class view
    {
        // arr need to be 1 2 3 4 5 ... nxn
        public static string[] arr = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" ,"10","11","12","13","14","15","16"};
        public static int i,j,k = 0;
        //get generic value from model class
        private static int generic_value=4;

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
            for (i = 0; i < generic_value; i++)//start print value
            {
                for (j = 0; j < generic_value; j++)
                {
                    if (i * 4 + j + 1 > 9)
                    {
                        Console.Write("  {0}  ", arr[i * 4 + j + 1]);
                    }
                    else
                    {
                        Console.Write("  {0}   ", arr[i * 4 + j + 1]);
                    }
                    
                }
                Console.WriteLine(" ");
            }
                /*
                Console.Write("     ");// print top
                for(k=0;k<generic_value;k++)
                {
                    Console.Write("|     ");
                }
                Console.WriteLine(" ");
                for (i = 0; i < generic_value; i++)//start print value
                {
                    for (j = 0; j < generic_value; j++)
                    {
                        if(i * 4 + j + 1 > 9)
                        {
                            Console.Write("  {0} |", arr[i * 4 + j + 1]);
                        }
                        else
                        {
                            Console.Write("  {0}  |", arr[i*4+j+1]);
                        }                                        
                    }
                    Console.WriteLine(" ");
                    Console.Write("_____");
                    for (k = 0; k < generic_value; k++)
                    {
                        Console.Write("|_____");
                    }
                    Console.WriteLine(" ");
                }
                Console.Write("     ");// print bottom
                for (k = 0; k < generic_value; k++)
                {
                    Console.Write("|     ");
                }
                Console.WriteLine(" ");
                */
                /*Console.WriteLine("     |     |      ");
                Console.WriteLine("  {0}  |  {1}  |  {2}", arr[1], arr[2], arr[3]);
                Console.WriteLine("_____|_____|_____ ");
                Console.WriteLine("     |     |      ");
                Console.WriteLine("  {0}  |  {1}  |  {2}", arr[4], arr[5], arr[6]);
                Console.WriteLine("_____|_____|_____ ");
                Console.WriteLine("     |     |      ");
                Console.WriteLine("  {0}  |  {1}  |  {2}", arr[7], arr[8], arr[9]);
                Console.WriteLine("     |     |      ");*/
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
