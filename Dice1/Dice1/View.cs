using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Security.Permissions;
using System.Diagnostics;

namespace Dice1
{
    internal class View
    {


        public void Show_digit(int front , TextBox Show)
        {
            Show.Text = front.ToString();
        }

        public void Draw_rectangle(Canvas Dice)
        {
            Rectangle rect = new Rectangle()
            {
                Height = Dice.Height,
                Width= Dice.Width,
                Stroke = Brushes.Black,
                StrokeThickness= 10,
            };

            Dice.Children.Add(rect);
            
        }

        public void Draw_point(Canvas Point , int digit)
        {

            Point.Children.Clear();

            switch (digit)
            {
                case 1:
                   
                    Ellipse circle = new Ellipse()
                    {
                        Width = Point.Width / 3,
                        Height = Point.Height / 3,
                        Fill = Brushes.Black,
                        Stroke = Brushes.Black,
                        StrokeThickness = 5,

                    };

                    Canvas.SetLeft(circle, Point.Height / 3);
                    Canvas.SetTop(circle, Point.Height / 3);
                    Point.Children.Add(circle);
                    break;

                case 2:
 

                    for (int i = 0; i < digit; i++)
                    {
                        circle = new Ellipse()
                        {
                            Width = Point.Width/3,
                            Height = Point.Height/3,
                            Fill = Brushes.Black,
                            Stroke = Brushes.Black,
                            StrokeThickness = 5,

                        };

                        if (i == 1)
                        {
                            Canvas.SetLeft(circle, 10);
                            Canvas.SetTop(circle, 10);
                        }
                        else
                        {
                            Canvas.SetRight(circle, 10);
                            Canvas.SetBottom(circle, 10);
                        }

                        Point.Children.Add(circle);

                    }
                    break;

                case 3:

                    for (int i = 0; i < digit; i++)
                    {
                        circle = new Ellipse()
                        {
                            Width = Point.Width / 3,
                            Height = Point.Height / 3,
                            Fill = Brushes.Black,
                            Stroke = Brushes.Black,
                            StrokeThickness = 5,

                        };

                        if (i == 1)
                        {
                            Canvas.SetLeft(circle, 10);
                            Canvas.SetTop(circle, 10);
                        }
                        else if(i == 2)
                        {
                            Canvas.SetRight(circle, 10);
                            Canvas.SetBottom(circle, 10);
                        }
                        else
                        {
                            Canvas.SetRight(circle, Point.Width/3);
                            Canvas.SetBottom(circle, Point.Width/3);
                        }
                        Point.Children.Add(circle);

                    }
                    break;

                case 4:

                    for (int i = 0; i < digit; i++)
                    {
                        circle = new Ellipse()
                        {
                            Width = Point.Width / 3,
                            Height = Point.Height / 3,
                            Fill = Brushes.Black,
                            Stroke = Brushes.Black,
                            StrokeThickness = 5,

                        };

                        if (i == 1)
                        {
                            Canvas.SetLeft(circle, 10);
                            Canvas.SetTop(circle, 10);
                        }
                        else if (i == 2)
                        {
                            Canvas.SetRight(circle, 10);
                            Canvas.SetTop(circle, 10);
                        }
                        else if (i == 3)
                        {
                            Canvas.SetLeft(circle, 10);
                            Canvas.SetBottom(circle, 10);
                        }
                        else
                        {
                            Canvas.SetRight(circle, 10);
                            Canvas.SetBottom(circle, 10);
                        }
                        Point.Children.Add(circle);

                    }
                    break;
                case 5:

                    for (int i = 0; i < digit; i++)
                    {
                        circle = new Ellipse()
                        {
                            Width = Point.Width / 3,
                            Height = Point.Height / 3,
                            Fill = Brushes.Black,
                            Stroke = Brushes.Black,
                            StrokeThickness = 5,

                        };
                        if (i == 1)
                        {
                            Canvas.SetLeft(circle, 10);
                            Canvas.SetTop(circle, 10);
                        }
                        else if (i == 2)
                        {
                            Canvas.SetRight(circle, 10);
                            Canvas.SetTop(circle, 10);
                        }
                        else if (i == 3)
                        {
                            Canvas.SetLeft(circle, 10);
                            Canvas.SetBottom(circle, 10);
                        }
                        else if (i == 4)
                        {
                            Canvas.SetRight(circle, 10);
                            Canvas.SetBottom(circle, 10);
                        }
                        else 
                        {
                            Canvas.SetRight(circle, Point.Width / 3);
                            Canvas.SetBottom(circle, Point.Width / 3);
                        }
                        Point.Children.Add(circle);
                    }

                    break;

                case 6:
                    for (int i = 0; i < digit; i++)
                    {
                        circle = new Ellipse()
                        {
                            Width = Point.Width / 3,
                            Height = Point.Height / 3,
                            Fill = Brushes.Black,
                            Stroke = Brushes.Black,
                            StrokeThickness = 5,

                        };
                        if (i < 3)
                        {
                            Canvas.SetLeft(circle, 5);
                            Canvas.SetTop(circle, 5 +(i*75));
                        }
                        
                        else
                        {
                            Canvas.SetRight(circle, 10);
                            Canvas.SetTop(circle, 5 + ((i-3) * 75));
                        }
                        Point.Children.Add(circle);
                    }
                    break;
            }
  
        }
    }
}
