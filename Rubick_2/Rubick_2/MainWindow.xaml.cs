using Rubick;
using Rubick_graphic;
using System;
using System.Collections.Generic;
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

namespace Rubick_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        view v = new view();
        model m = new model();

        public MainWindow()
        {
            InitializeComponent();
            fill_rubik();
        }

       private void fill_rubik()// z fix at 0
        {

            v.fill_dice(front1, m.GetDice(0 ,0,0).GetColor(1));
            v.fill_dice(front2, m.GetDice(1, 0, 0).GetColor(1));
            v.fill_dice(front3, m.GetDice(2, 0, 0).GetColor(1));
            v.fill_dice(front4, m.GetDice(0, 1, 0).GetColor(1));
            v.fill_dice(front5, m.GetDice(1, 1, 0).GetColor(1));
            v.fill_dice(front6, m.GetDice(2, 1, 0).GetColor(1));
            v.fill_dice(front7, m.GetDice(0, 2, 0).GetColor(1));
            v.fill_dice(front8, m.GetDice(1, 2, 0).GetColor(1));
            v.fill_dice(front9, m.GetDice(2, 2, 0).GetColor(1));
            // top y fix at 2
            v.fill_dice(top1, m.GetDice(0, 2, 0).GetColor(2));
            v.fill_dice(top2, m.GetDice(1, 2, 0).GetColor(2));
            v.fill_dice(top3, m.GetDice(2, 2, 0).GetColor(2));
            v.fill_dice(top4, m.GetDice(0, 2, 1).GetColor(2));
            v.fill_dice(top5, m.GetDice(1, 2, 1).GetColor(2));
            v.fill_dice(top6, m.GetDice(2, 2, 1).GetColor(2));
            v.fill_dice(top7, m.GetDice(0, 2, 2).GetColor(2));
            v.fill_dice(top8, m.GetDice(1, 2, 2).GetColor(2));
            v.fill_dice(top9, m.GetDice(2, 2, 2).GetColor(2));
            // right x fix at 2
            v.fill_dice(side1, m.GetDice(2, 0, 0).GetColor(3));
            v.fill_dice(side2, m.GetDice(2, 0, 1).GetColor(3));
            v.fill_dice(side3, m.GetDice(2, 0, 2).GetColor(3));
            v.fill_dice(side4, m.GetDice(2, 1, 0).GetColor(3));
            v.fill_dice(side5, m.GetDice(2, 1, 1).GetColor(3));
            v.fill_dice(side6, m.GetDice(2, 1, 2).GetColor(3));
            v.fill_dice(side7, m.GetDice(2, 2, 0).GetColor(3));
            v.fill_dice(side8, m.GetDice(2, 2, 1).GetColor(3));
            v.fill_dice(side9, m.GetDice(2, 2, 2).GetColor(3));
        }

        private void Roll_x0(object sender, RoutedEventArgs e)
        {
            m.roll('x', 0);
            fill_rubik();
        }

        private void Roll_x1(object sender, RoutedEventArgs e)
        {
            m.roll('x', 1);
            fill_rubik();
        }

        private void Roll_x2(object sender, RoutedEventArgs e)
        {
            m.roll('x', 2);
            fill_rubik();
        }
        private void Roll_back_x0(object sender, RoutedEventArgs e)
        {
            m.roll('x', 0);
            m.roll('x', 0);
            m.roll('x', 0);
            fill_rubik();
        }
        private void Roll_back_x1(object sender, RoutedEventArgs e)
        {
            m.roll('x', 1);
            m.roll('x', 1);
            m.roll('x', 1);
            fill_rubik();
        }
        private void Roll_back_x2(object sender, RoutedEventArgs e)
        {
            m.roll('x', 2);
            m.roll('x', 2);
            m.roll('x', 2);
            fill_rubik();
        }
        private void Roll_y0(object sender, RoutedEventArgs e)
        {
            m.roll('y', 0);
            fill_rubik();
        }
        private void Roll_y1(object sender, RoutedEventArgs e)
        {
            m.roll('y', 1);
            fill_rubik();
        }
        private void Roll_y2(object sender, RoutedEventArgs e)
        {
            m.roll('y', 2);
            fill_rubik();
        }
        private void Roll_back_y0(object sender, RoutedEventArgs e)
        {
            m.roll('y', 0);
            m.roll('y', 0);
            m.roll('y', 0);
            fill_rubik();
        }
        private void Roll_back_y1(object sender, RoutedEventArgs e)
        {
            m.roll('y', 1);
            m.roll('y', 1);
            m.roll('y', 1);
            fill_rubik();
        }
        private void Roll_back_y2(object sender, RoutedEventArgs e)
        {
            m.roll('y', 2);
            m.roll('y', 2);
            m.roll('y', 2);
            fill_rubik();
        }

        private void Roll_z0(object sender, RoutedEventArgs e)
        {
            m.roll('z', 0);
            fill_rubik();
        }

        private void Roll_z1(object sender, RoutedEventArgs e)
        {
            m.roll('z', 1);
            fill_rubik();
        }

        private void Roll_z2(object sender, RoutedEventArgs e)
        {
            m.roll('z', 2);
            fill_rubik();
        }
        private void Roll_back_z0(object sender, RoutedEventArgs e)
        {
            m.roll('z', 0);
            m.roll('z', 0);
            m.roll('z', 0);
            fill_rubik();
        }
        private void Roll_back_z1(object sender, RoutedEventArgs e)
        {
            m.roll('z', 1);
            m.roll('z', 1);
            m.roll('z', 1);
            fill_rubik();
        }
        private void Roll_back_z2(object sender, RoutedEventArgs e)
        {
            m.roll('z', 2);
            m.roll('z', 2);
            m.roll('z', 2);
            fill_rubik();
        }


    }
}
