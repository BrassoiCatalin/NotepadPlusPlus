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
using System.Windows.Shapes;
using NotepadPlusPlus.ViewModels;

namespace NotepadPlusPlus.Views
{
    /// <summary>
    /// Interaction logic for ReplaceAllWindow.xaml
    /// </summary>
    public partial class ReplaceAllWindow : Window
    {
        public ReplaceAllWindow()
        {
            InitializeComponent();
            DataContext = new ReplaceAllViewModel();
        }

        private void ReplaceAllButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
