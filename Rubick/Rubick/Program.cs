﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubick
{
    public class dice
    {
        private int front { get; set; }
        private int top { get; set; }
        private int right { get; set; }
        public dice()
        {
            front = 1;
            top = 2;
            right = 3;
        }
        public int GetColor(int side)
        {
            if (side == 1) { return front; }
            else if (side == 2) { return top; }
            return right;
        }
        public void rollup()
        {
            int temp = top;
            top = front;
            front = 7 - temp;
        }
        public void rollright()
        {
            int temp = right;
            right = front;
            front = 7 - temp;
        }
        public void rollleft()
        {
            int temp = top;
            top = right;
            right = 7 - temp;
        }

    }
    public class model
    {
        private dice[,,] rubick { get; set; }
        private int n = 3; // dimension n x n xn
        public model() // constructor coordinate x,y,z 
        {
            rubick = new dice[n,n,n];
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    for(int k = 0; k < n; k++)
                    {
                        rubick[i, j, k] = new dice();
                    }
                }
            }
        }
        public dice GetDice(int x,int y,int z)
        {
            return rubick[x,y,z];
        }
        public void roll(char c,int a) // int a = 0 1 2
        {
            if (c == 'x') // move location
            {// call ' roll up '
                dice temp = rubick[a, 0, 0];
                rubick[a, 0, 0] = rubick[a, 0, 2];
                rubick[a, 0, 2] = rubick[a, 2, 2];
                rubick[a, 2, 2] = rubick[a, 2, 0];
                rubick[a, 2, 0] = temp;
                temp = rubick[a, 1, 0];
                rubick[a, 1, 0] = rubick[a, 0, 1];
                rubick[a, 0, 1] = rubick[a, 1, 2];
                rubick[a, 1, 2] = rubick[a, 2, 1];
                rubick[a, 2, 1] = temp;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        rubick[a, i, j].rollup();
                    }
                }
            }
            else if (c == 'y') // move location
            {// call  ' roll right '
                dice temp = rubick[0, a, 0];
                rubick[0, a, 0] = rubick[0, a, 2];
                rubick[0, a,2 ] = rubick[2, a,2 ];
                rubick[2, a,2 ] = rubick[2, a,0 ];
                rubick[2, a, 0] = temp;
                temp = rubick[1, a,0 ];
                rubick[1, a,0 ] = rubick[0, a,1 ];
                rubick[0,a ,1 ] = rubick[1,a , 2];
                rubick[1,a ,2 ] = rubick[2,a , 1];
                rubick[2,a ,1 ] = temp;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        rubick[i,a, j].rollright();
                    }
                }
            }
            else if (c == 'z') // move location
            {// call ' roll left '
                dice temp = rubick[0,0,a];
                rubick[0, 0, a] = rubick[0, 2, a];
                rubick[0, 2, a] = rubick[2, 2, a];
                rubick[2, 2, a] = rubick[2, 0, a];
                rubick[2, 0, a] = temp;
                temp = rubick[1,0 , a];
                rubick[1, 0, a] = rubick[0, 1, a];
                rubick[0, 1, a] = rubick[1, 2, a];
                rubick[1, 2, a] = rubick[2, 1, a];
                rubick[2, 1, a] = temp;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        rubick[i, j, a].rollleft();
                    }
                }
            }
        }
        public void reset()
        {
            dice temp= new dice();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        rubick[i, j, k] = temp;
                    }
                }
            }
        }
    }
    public class view
    {
        public void display(int n, model m)
        {
            //display top y fix at n-1
            Console.WriteLine("Top");
            for (int i = n-1;i>-1;i--)// for loop only x , z
            {
                for(int j = 0; j < n; j++)
                {
                    Console.Write(m.GetDice(j, n - 1, i).GetColor(2)+" ");
                }
                    Console.WriteLine(" ");
            }
            // display front and right
            Console.WriteLine("Front");
            for (int i = n - 1; i > -1; i--)// for loop only x , z
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(m.GetDice(j, i, 0).GetColor(1) + " ");
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine("          right");
            for (int i = n - 1; i > -1; i--)// for loop only x , z
            {
                Console.Write("          ");
                for (int j = 0; j < n; j++)
                {
                    Console.Write(m.GetDice(n-1, i, j).GetColor(3) + " ");
                }
                Console.WriteLine(" ");
            }

        }
    }
    public class controller
    {
        public static void Main(string[] args)
        {
            model m = new model();
            view v = new view();
            while (true)
            {
                v.display(3, m);
                Console.WriteLine("Insert command");
                string command = Console.ReadLine();
                if (command == "exit")
                {
                    break;
                }
                else if(command == "reset")
                {
                    m.reset();
                }
                else
                {
                    char axis = command[0];
                    string p = Console.ReadLine();
                    int order = Int32.Parse(p);                 
                    m.roll(axis, order);
                    //Console.Clear();
                }
                
            }
        }

        
    }
}
