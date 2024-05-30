using System.Windows;
using Tema3MVVM.ViewModels;

namespace Tema3MVVM
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            DataContext = new AdminViewModel();
        }
    }
}