using Metro.Models;
using Metro.Repositories;
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

namespace Metro.Views
{
    /// <summary>
    /// Interaction logic for TerkepView.xaml
    /// </summary>
    public partial class TerkepView : UserControl
    {
        private MetroRepository repository = new MetroRepository();
        public TerkepView()
        {
            InitializeComponent();
            DrawRailLines();
            DrawMap();
        }

        private void DrawMap()
        {
            SolidColorBrush fekete = new SolidColorBrush(Colors.Black);
            foreach (var allomas in repository.Allomasok)
            {
                Ellipse pont = new Ellipse()
                {
                    Width = 10,
                    Height = 10,
                    Fill = fekete
                };
                Canvas.SetLeft(pont, allomas.X - 5);
                Canvas.SetTop(pont, allomas.Y - 5);
                cnvTerkep.Children.Add(pont);

                TextBlock szoveg = new TextBlock()
                {
                    Text = allomas.AllomasNev,
                    Width= 100,
                    TextWrapping = TextWrapping.Wrap
                };
                Canvas.SetLeft(szoveg, allomas.X + 5);
                Canvas.SetTop(szoveg, allomas.Y + 2);
                cnvTerkep.Children.Add(szoveg);
            }
        }

        private void DrawRailLines()
        {
            SolidColorBrush piros = new SolidColorBrush(Colors.Red);
            SolidColorBrush zold = new SolidColorBrush(Colors.Green);

            foreach (var metroVonal in repository.MetroVonalak)
            {
                for (int i = 0; i < metroVonal.Allomasok.Count; i++)
                {
                    if (i < metroVonal.Allomasok.Count - 1)
                    {
                        int startX = metroVonal.Allomasok.ElementAt(i).Value.X;
                        int startY = metroVonal.Allomasok.ElementAt(i).Value.Y;
                        int endX = metroVonal.Allomasok.ElementAt(i + 1).Value.X;
                        int endY = metroVonal.Allomasok.ElementAt(i + 1).Value.Y;

                        Line vonal = new Line()
                        {
                            X1 = startX,
                            X2 = endX,
                            Y1 = startY,
                            Y2 = endY,
                            StrokeThickness = 4
                        };

                        switch (metroVonal.VonalNev)
                        {
                            case "M2":
                                vonal.Stroke = piros;
                                break;
                            case "M4":
                                vonal.Stroke = zold;
                                break;
                            default:
                                break;
                        }
                        cnvTerkep.Children.Add(vonal);
                    }
                }
            }
        }

        private void ZoomPlus_Click(object sender, RoutedEventArgs e)
        {
            st.ScaleX += 0.5;
            st.ScaleY += 0.5;
        }

        private void ZoomMinus_Click(object sender, RoutedEventArgs e)
        {
            st.ScaleX -= 0.5;
            st.ScaleY -= 0.5;
        }

        private void ZoomReset_Click(object sender, RoutedEventArgs e)
        {
            st.ScaleX = 1;
            st.ScaleY = 1;
        }
    }
}
