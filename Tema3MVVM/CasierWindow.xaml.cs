using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Tema3MVVM.ViewModels;

namespace Tema3MVVM
{
    public partial class CasierWindow : Window
    {
        public CasierWindow(int casierId)
        {
            InitializeComponent();
            DataContext = new CasierViewModel(casierId);
        }
    }
}