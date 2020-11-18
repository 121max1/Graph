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
        private List<Ellipse> _selectedVertexs = new List<Ellipse>();
        private readonly Graph _graph = new Graph();
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
                    Name = Graph.cntVertix.ToString(),
                    Number = Graph.cntVertix
                };
                _graph.AddVertex(vertexView);
                RenderVerter(vertexView,Color.FromRgb(140,0,0));
            }
            else if (_deleteButtonIsPressed)
            {
                foreach (UIElement child in GraphCanvas.Children)
                {
                    if (child is Ellipse ellipse)
                    {
                        child.MouseLeftButtonDown += ChildDelete_MouseLeftButtonDown; ;
                    }
                }
            }
            else if(_addEdgeButtonIsPressed)
            {
                foreach (UIElement child in GraphCanvas.Children)
                {
                    if (child is Ellipse ellipse)
                    {
                        child.MouseLeftButtonDown += ChildSelect_MouseLeftButtonDown;
                    }
                }
                if (_selectedVertexs.Count == 2)
                {
                    AddNewEdgeWindow addNewEdgeWindow = new AddNewEdgeWindow();
                    if (addNewEdgeWindow.ShowDialog() == true)
                    {
                        EdgeView edge = new EdgeView(
                            _graph.GetVertexByNumber((int)_selectedVertexs[0].Tag),
                            _graph.GetVertexByNumber((int)_selectedVertexs[1].Tag),
                            addNewEdgeWindow.Distance);
                        _graph.AddEdge(edge);
                        RenderEdge(addNewEdgeWindow.IsOriented, edge);
                        foreach (var elps in _selectedVertexs)
                        {
                            elps.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                        }
                        _selectedVertexs.Clear();
                    }
                }
            }
        }

        private void ChildDelete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            if (ellipse != null)
            {
                GraphCanvas.Children.Remove(ellipse);
                _selectedVertexs.Remove(ellipse);
                TextBlock toDelete = new TextBlock();
                List<Line> linesToDelete = new List<Line>();
                foreach (UIElement child in GraphCanvas.Children)
                {
                    if (child is TextBlock textBlock)
                    {
                        if (textBlock.Tag == ellipse.Tag)
                        {
                            toDelete = textBlock;
                        }
                        
                    }
                    else if( child is Line line)
                    {
                        int firstVertexNumber = int.Parse(line.Tag.ToString().Split()[0]);
                        int secondVertexNumber = int.Parse(line.Tag.ToString().Split()[0]);
                        if ((int)ellipse.Tag == firstVertexNumber || (int)ellipse.Tag == secondVertexNumber)
                        {
                            linesToDelete.Add(line);
                        }
                    }
                }
                GraphCanvas.Children.Remove(toDelete);
                foreach(Line line in linesToDelete)
                {
                    GraphCanvas.Children.Remove(line);
                }

            }
            _graph.DeleteVertex(_graph.GetVertexByNumber((int)ellipse.Tag));
            
        }

        private void ChildSelect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            bool addToSelectedVertexFlag = true;
            foreach(var elps in _selectedVertexs)
            {
                if (ellipse.Tag.ToString() == elps.Tag.ToString())
                {
                    addToSelectedVertexFlag = false;
                }
            }
            if (ellipse != null && addToSelectedVertexFlag)
            {
                if (_selectedVertexs.Count < 2)
                {
                    ellipse.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 140));
                    _selectedVertexs.Add(ellipse);
                }
                else
                {
                    foreach(var elps in _selectedVertexs)
                    {
                        elps.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                    }
                    _selectedVertexs.Clear();
                }
            }
        }
    

    

        private Ellipse RenderVerter(VertexView vertex, Color color)
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
            return ellipse;
        }



        private void RenderEdge(bool? isOriented, EdgeView edge)
        {
            if(isOriented == true)
            {
                RenderOrientedEdge(edge);
            }
            else
            {
                RenderNonOrientedEdge(edge);
            }
        }
        
        private void RenderOrientedEdge(EdgeView edge)
        {
            double theta = Math.Atan2(edge.V1.Y - edge.V2.Y, edge.V1.X - edge.V2.X);
            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);
            double HeadWidth = 50;
            double HeadHeight = 5;

            Point pt1 = new Point(edge.V1.X - VertexView.Radius / 2, edge.V1.Y - VertexView.Radius / 2);
            Point pt2 = new Point(edge.V2.X - VertexView.Radius / 2, edge.V2.Y - VertexView.Radius / 2);

            Point pt3 = new Point(
                pt2.X + (HeadWidth * cost - HeadHeight * sint),
                pt2.Y + (HeadWidth * sint + HeadHeight * cost));

            Point pt4 = new Point(
                pt2.X + (HeadWidth * cost + HeadHeight * sint),
                pt2.Y - (HeadHeight * cost - HeadWidth * sint));

            Line mainLine = new Line();
            mainLine.X1 = pt1.X;
            mainLine.Y1 = pt1.Y;

            mainLine.X2 = pt2.X;
            mainLine.Y2 = pt2.Y;
            mainLine.Stroke = new SolidColorBrush(Color.FromRgb(0,0,0));
            mainLine.StrokeThickness = 2;
            mainLine.Tag = edge.V1.Number + " " + edge.V2.Number;
            GraphCanvas.Children.Add(mainLine);

            Line arrowLine1 = new Line();
            arrowLine1.X1 = pt2.X;
            arrowLine1.Y1 = pt2.Y;

            arrowLine1.X2 = pt3.X;
            arrowLine1.Y2 = pt3.Y;
            arrowLine1.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            arrowLine1.StrokeThickness = 2;
            arrowLine1.Tag = edge.V1.Number + " " + edge.V2.Number;
            GraphCanvas.Children.Add(arrowLine1);

            Line arrowLine2 = new Line();
            arrowLine2.X1 = pt2.X;
            arrowLine2.Y1 = pt2.Y;

            arrowLine2.X2 = pt4.X;
            arrowLine2.Y2 = pt4.Y;
            arrowLine2.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            arrowLine2.StrokeThickness = 2;
            arrowLine2.Tag = edge.V1.Number + " " + edge.V2.Number;
            GraphCanvas.Children.Add(arrowLine2);

            Point pt5 = new Point(
                Math.Abs(pt2.X + pt1.X) / 2,
                Math.Abs(pt2.Y + pt1.Y) / 2);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = edge.Distance.ToString();
            textBlock.FontSize = 15;
            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            Canvas.SetLeft(textBlock, pt5.X);
            Canvas.SetTop(textBlock, pt5.Y);
            textBlock.Tag = edge.V1.Number + " " + edge.V2.Number;
            GraphCanvas.Children.Add(textBlock);
        }

        private void RenderNonOrientedEdge(EdgeView edge)
        {
            double theta = Math.Atan2(edge.V1.Y - edge.V2.Y, edge.V1.X - edge.V2.X);
            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);
            double HeadWidth = 50;
            double HeadHeight = 5;

            Point pt1 = new Point(edge.V1.X - VertexView.Radius / 2, edge.V1.Y - VertexView.Radius / 2);
            Point pt2 = new Point(edge.V2.X - VertexView.Radius / 2, edge.V2.Y - VertexView.Radius / 2);

            Point pt3 = new Point(
                pt2.X + (HeadWidth * cost - HeadHeight * sint),
                pt2.Y + (HeadWidth * sint + HeadHeight * cost));

            Point pt4 = new Point(
                pt2.X + (HeadWidth * cost + HeadHeight * sint),
                pt2.Y - (HeadHeight * cost - HeadWidth * sint));

            Line mainLine = new Line();
            mainLine.X1 = pt1.X;
            mainLine.Y1 = pt1.Y;

            mainLine.X2 = pt2.X;
            mainLine.Y2 = pt2.Y;
            mainLine.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            mainLine.StrokeThickness = 2;
            mainLine.Tag = edge.V1.Number + " " + edge.V2.Number;
            GraphCanvas.Children.Add(mainLine);

            Point pt5 = new Point(
                Math.Abs(pt2.X + pt1.X) / 2,
                Math.Abs(pt2.Y + pt1.Y) / 2);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = edge.Distance.ToString();
            textBlock.FontSize = 15;
            textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            Canvas.SetLeft(textBlock, pt5.X);
            Canvas.SetTop(textBlock, pt5.Y);
            textBlock.Tag = edge.V1.Number + " " + edge.V2.Number;
            GraphCanvas.Children.Add(textBlock);
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

