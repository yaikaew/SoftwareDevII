using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleOX1
{
    class view
    {

        public void Board(string CurrentPlayer,int generic_value ,List<string> list)
        {
            Console.Clear();// whenever loop will be again start then screen will be clear
            Console.WriteLine("Command");
            Console.WriteLine("save : for save the current game");
            Console.WriteLine("load : for load the last game that you save");
            Console.WriteLine("\n");


            String[] arr = list.ToArray();

            //checking the chance of the player
            if (CurrentPlayer == "X")
            {
                Console.WriteLine("Player X Chance");
            }
            else
            {
                Console.WriteLine("Player O Chance");
            }
            Console.WriteLine("\n");

            //generate table format 
            for (int i = 0; i < generic_value; i++)
            {
                for (int j = 0; j < generic_value; j++)
                {
                    if (i * generic_value + j + 1 > 9)
                    {
                        Console.Write(" | {0} | ", arr[i * generic_value + j + 1 ]);
                    }
                    else
                    {
                        Console.Write(" | {0}  | ", arr[i * generic_value + j + 1 ]);
                    }
                    
                }
                Console.WriteLine(" ");
                Console.WriteLine(" ");

            }
               
        }

        public void DisplayResult(string winner , string wininfo )
        {
            Console.WriteLine("\n");

            if (winner != "None")
            {
                Console.WriteLine("WINNER IS " + "PLAYER : " + winner );
                Console.WriteLine("WINTYPE : " + wininfo);
            }

            else
            {
                Console.WriteLine("DRAW");
            }
        }


    }
}
