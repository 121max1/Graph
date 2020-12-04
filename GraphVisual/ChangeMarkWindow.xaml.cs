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

namespace GraphVisual
{
    /// <summary>
    /// Логика взаимодействия для ChangMarkWindow.xaml
    /// </summary>
    public partial class ChangeMarkWindow : Window
    {
        public bool isToSubmitMark = false;

        public ChangeMarkWindow()
        {
            InitializeComponent();
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            isToSubmitMark = false;
            this.DialogResult = true;
            this.Close();
        }

        private void SubmitMarkButton_Click(object sender, RoutedEventArgs e)
        {
            isToSubmitMark = true;
            this.DialogResult = true;
            this.Close();
        }
    }
}
