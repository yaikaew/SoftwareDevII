using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dice1
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            view.Show_digit(model.front,Show);
            view.Draw_rectangle(Dice);
            view.Draw_point(Point,model.front);
        }
 
        Model model = new Model();
        View view = new View();
 

        private void Click_Roll_up(object sender, RoutedEventArgs e)
        {
            model.Roll_Up();
            view.Show_digit(model.front,Show);
            view.Draw_point(Point, model.front);

        }

        private void Click_Roll_down(object sender, RoutedEventArgs e)
        {
            model.Roll_Down();
            view.Show_digit(model.front,Show);
            view.Draw_point(Point, model.front);
        }

        private void Click_Roll_right(object sender, RoutedEventArgs e)
        {
            model.Roll_Right();
            view.Show_digit(model.front,Show);
            view.Draw_point(Point, model.front);

        }

        private void Click_Roll_left(object sender, RoutedEventArgs e)
        {
            model.Roll_Left();
            view.Show_digit(model.front,Show);
            view.Draw_point(Point, model.front);
        }
    }
}
