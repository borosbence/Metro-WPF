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
            SolidColorBrush fekete = new(Colors.Black);
            SolidColorBrush szurke = new(Colors.LightGray);
            szurke.Opacity = 0.75;
            foreach (var allomas in viewModel.Allomasok)
            {
                Ellipse kor = new()
                {
                    Width = 10,
                    Height = 10,
                    Stroke = fekete,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(kor, allomas.X - 5);
                Canvas.SetTop(kor, allomas.Y - 5);
                cnvTerkep.Children.Add(kor);

                TextBlock szoveg = new()
                {
                    Text = allomas.AllomasNev,
                    MaxWidth = 100,
                    TextWrapping = TextWrapping.Wrap,
                    Background = szurke,
                    FontSize = 8
                };
                Canvas.SetLeft(szoveg, allomas.X + 5);
                Canvas.SetTop(szoveg, allomas.Y + 2);
                Panel.SetZIndex(szoveg, 3);
                cnvTerkep.Children.Add(szoveg);
            }
        }

        private void DrawnRailLines()
        {
            SolidColorBrush sarga = new(Colors.Yellow);
            SolidColorBrush piros = new(Colors.Red);
            SolidColorBrush sotetkek = new(Colors.DarkBlue);
            SolidColorBrush zold = new(Colors.Green);

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

                        Line vonal = new()
                        {
                            X1 = startX,
                            X2 = endX,
                            Y1 = startY,
                            Y2 = endY,
                            StrokeThickness = 4
                        };

                        switch (metroVonal.VonalNev)
                        {
                            case "M1":
                                vonal.Stroke = sarga;
                                break;
                            case "M2":
                                vonal.Stroke = piros;
                                break;
                            case "M3":
                                vonal.Stroke = sotetkek;
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
