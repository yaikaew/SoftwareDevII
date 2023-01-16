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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Show_dice();
         
        }
        int x = 1;
        private void Show_dice()
        {
            Show.Text = front.ToString();
        }

        int front = 1;
        int temp;
        int top = 2;
        int right = 4;


        private void Roll_Up()
        {

            temp = front;
            front = 7 - top;
            top = temp;
  
        }

        private void Roll_Left()
        {
            temp = front;
            front = right;
            right = 7 - temp;
        }

        private void Roll_Right()
        {
            Roll_Left();
            Roll_Left();
            Roll_Left();
        }

        private void Roll_Down()
        {
            Roll_Up();
            Roll_Up();
            Roll_Up();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Roll_Up();
            Show_dice();
        }

        private void Click_Roll_down(object sender, RoutedEventArgs e)
        {
            Roll_Down();
            Show_dice();
        }

        private void Click_Roll_right(object sender, RoutedEventArgs e)
        {
            Roll_Right();
            Show_dice();

        }

        private void Click_Roll_left(object sender, RoutedEventArgs e)
        {
            Roll_Left();
            Show_dice();
        }
    }
}
