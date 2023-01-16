using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dice1
{
    internal class Model
    {
        public int front = 1 ;
        public int temp;
        public int top = 2;
        public int right = 4;
        public void Roll_Up()
        {
            temp = front;
            front = 7 - top;
            top = temp;

        }

        public void Roll_Left()
        {
            temp = front;
            front = right;
            right = 7 - temp;
        }

        public void Roll_Right()
        {
            Roll_Left();
            Roll_Left();
            Roll_Left();
        }

        public void Roll_Down()
        {
            Roll_Up();
            Roll_Up();
            Roll_Up();
        }
    }
}
