using GraphVisual.Models;
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

namespace GraphVisual
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _addVertexuttonIsPressed = false;
        public MainWindow()
        {
            InitializeComponent();
        }

       
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }


        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }
        private void RenderVerter(VertexView vertex, Color color)
        {
            Ellipse ellipse = new Ellipse()
            {
                Width = VertexView.Radius,
                Height = VertexView.Radius,
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 14
            };
            GraphCanvas.Children.Add(ellipse);
            ellipse.SetValue(Canvas.LeftProperty, vertex.X - VertexView.Radius);
            ellipse.SetValue(Canvas.RightProperty, vertex.Y - VertexView.Radius);

            TextBlock text = new TextBlock();
            text.Text = vertex.Name;
            text.FontSize = 18;
            text.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            Canvas.SetLeft(text, vertex.X - 10);
            Canvas.SetTop(text, vertex.Y + 10);
            GraphCanvas.Children.Add(text);
        }

        private void AddVertexButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_addVertexuttonIsPressed)
            {
                _addVertexuttonIsPressed = true;
                AddEdgeButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                SelectVertexButton.IsEnabled = false;
            }
            else
            {
                _addVertexuttonIsPressed = false;
                AddEdgeButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                SelectVertexButton.IsEnabled = true;
            }    
        }
    }
}

