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
    /// Логика взаимодействия для AddNewEdgeWindow.xaml
    /// </summary>
    public partial class AddNewEdgeWindow : Window
    {
        public AddNewEdgeWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        public int Distance
        {
            get { return int.Parse(DistanceTextBox.Text); }
        }

        public bool? IsOriented
        {
            get { return OrientedEdgeRadioButton.IsChecked; }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
