using Metro.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Metro.Views
{
    /// <summary>
    /// Interaction logic for UtvonalView.xaml
    /// </summary>
    public partial class UtvonalView : UserControl
    {
        public UtvonalView()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetRequiredService<UtvonalViewModel>();
        }
    }
}
