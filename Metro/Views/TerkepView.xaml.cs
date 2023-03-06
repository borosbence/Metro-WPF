using Metro.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Metro.Views
{
    /// <summary>
    /// Interaction logic for TerkepView.xaml
    /// </summary>
    public partial class TerkepView : UserControl
    {
        private readonly TerkepViewModel viewModel;
        public TerkepView()
        {
            InitializeComponent();
            viewModel = App.Current.Services.GetRequiredService<TerkepViewModel>();
            DataContext = viewModel;
            DrawnRailLines();
            DrawMap();
        }

        private void DrawMap()
        {
            SolidColorBrush fekete = new SolidColorBrush(Colors.Black);
            foreach (var allomas in viewModel.Allomasok)
            {
                Ellipse kor = new Ellipse()
                {
                    Width = 10,
                    Height = 10,
                    Fill = fekete
                };
                Canvas.SetLeft(kor, allomas.X - 5);
                Canvas.SetTop(kor, allomas.Y - 5);
                cnvTerkep.Children.Add(kor);

                TextBlock szoveg = new TextBlock()
                {
                    Text = allomas.AllomasNev,
                    Width = 100,
                    TextWrapping = TextWrapping.Wrap
                };
                Canvas.SetLeft(szoveg, allomas.X + 5);
                Canvas.SetTop(szoveg, allomas.Y + 2);
                cnvTerkep.Children.Add(szoveg);
            }
        }

        private void DrawnRailLines()
        {
            SolidColorBrush piros = new SolidColorBrush(Colors.Red);
            SolidColorBrush zold = new SolidColorBrush(Colors.Green);

            foreach (var metroVonal in viewModel.MetroVonalak)
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

        private void cnvTerkep_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pont = e.GetPosition(cnvTerkep);
            foreach (var allomas in viewModel.Allomasok)
            {
                if (Math.Abs(allomas.X - pont.X) < 5 && Math.Abs(allomas.Y - pont.Y) < 5)
                {
                    viewModel.SendMessage(allomas.AllomasNev);
                }
            }
        }
    }
}
