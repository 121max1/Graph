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
        private bool _addVertexButtonIsPressed = false;
        private bool _addEdgeButtonIsPressed = false;
        private bool _deleteButtonIsPressed = false;
        private bool _selectVertexButtonIsPressed = false;
        private static int _cntVertex = 1;
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
            if(_addVertexButtonIsPressed)
            {
                VertexView vertexView = new VertexView()
                {
                    X = e.GetPosition(GraphCanvas).X,
                    Y = e.GetPosition(GraphCanvas).Y,
                    Name = _cntVertex.ToString(),
                    Number = _cntVertex
                };
                _cntVertex += 1;
           
                RenderVerter(vertexView,Color.FromRgb(140,0,0));
            }
            else if (_deleteButtonIsPressed)
            {
                double mx = e.GetPosition(GraphCanvas).X;
                double my = e.GetPosition(GraphCanvas).Y;
                foreach (UIElement child in GraphCanvas.Children)
                {
                    if (child is Ellipse ellipse)
                    {
                        child.MouseLeftButtonDown += Child_MouseLeftButtonDown;
                    }
                }
            }
        }

        private void Child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            if (ellipse != null)
            {
                GraphCanvas.Children.Remove(ellipse);
                TextBlock to_delete = new TextBlock();
                foreach(UIElement child in GraphCanvas.Children)
                {
                    if (child is TextBlock textBlock)
                    {
                        if(textBlock.Tag == ellipse.Tag)
                        {
                            to_delete = textBlock;
                        }
                    }
                }
                GraphCanvas.Children.Remove(to_delete);
            }
        }

        private void RenderVerter(VertexView vertex, Color color)
        {
            Ellipse ellipse = new Ellipse()
            {
                Width = VertexView.Radius * 2,
                Height = VertexView.Radius * 2,
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 14,
                Tag = vertex.Number
            };
            ellipse.SetValue(Canvas.LeftProperty, vertex.X - VertexView.Radius);
            ellipse.SetValue(Canvas.TopProperty, vertex.Y - VertexView.Radius);
            ellipse.SetValue(Canvas.BottomProperty, vertex.Y + VertexView.Radius);
            ellipse.SetValue(Canvas.RightProperty, vertex.X + VertexView.Radius);
            GraphCanvas.Children.Add(ellipse);

            TextBlock text = new TextBlock();
            text.Tag = ellipse.Tag;
            text.Text = vertex.Name;
            text.FontSize = 18;
            text.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            Canvas.SetLeft(text, vertex.X - 10);
            Canvas.SetTop(text, vertex.Y + 10);
            GraphCanvas.Children.Add(text);
            
        }

        private void AddVertexButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_addVertexButtonIsPressed)
            {
                _addVertexButtonIsPressed = true;
                AddEdgeButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                SelectVertexButton.IsEnabled = false;
            }
            else
            {
                _addVertexButtonIsPressed = false;
                AddEdgeButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                SelectVertexButton.IsEnabled = true;
            }    
        }

        private void AddEdgeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_addEdgeButtonIsPressed)
            {
                _addEdgeButtonIsPressed = true;
                AddVertexButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                SelectVertexButton.IsEnabled = false;
            }
            else
            {
                _addEdgeButtonIsPressed = false;
                AddVertexButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                SelectVertexButton.IsEnabled = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_deleteButtonIsPressed)
            {
                _deleteButtonIsPressed = true;
                AddVertexButton.IsEnabled = false;
                AddEdgeButton.IsEnabled = false;
                SelectVertexButton.IsEnabled = false;
            }
            else
            {
                _deleteButtonIsPressed = false;
                AddVertexButton.IsEnabled = true;
                AddEdgeButton.IsEnabled = true;
                SelectVertexButton.IsEnabled = true;
            }


        }

        private void SelectVertexButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_selectVertexButtonIsPressed)
            {
                _selectVertexButtonIsPressed = true;
                AddVertexButton.IsEnabled = false;
                AddEdgeButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            }
            else
            {
                _selectVertexButtonIsPressed = false;
                AddVertexButton.IsEnabled = true;
                AddEdgeButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
            }
        }
    }
}

