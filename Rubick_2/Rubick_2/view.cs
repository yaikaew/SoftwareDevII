using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Rubick_graphic
{
    internal class view
    {
        public void fill_dice(Rectangle dice, int color)
        {
            // red = 1 orange = 6 white = 2 yellow = 5  green = 3 blue = 4
            if (color == 1) { 
            dice.Fill = Brushes.Red;
            }
            else if(color == 2)
            {
                dice.Fill = Brushes.White;
            }
            else if (color == 3)
            {
                dice.Fill = Brushes.Green;
            }
            else if (color == 4)
            {
                dice.Fill = Brushes.Blue;
            }
            else if (color == 5)
            {
                dice.Fill = Brushes.Yellow;
            }
            else if (color == 6)
            {
                dice.Fill = Brushes.Orange;
            }
        }
    }
}
