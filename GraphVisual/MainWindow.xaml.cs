﻿using Graph.AlgDjekstra;
using GraphVisual.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        private bool _alreadyDeleted = false;
        private bool _learningMode = false;
        private Ellipse _selectedVertex;
        private List<Ellipse> _selectedVertexsForAlg = new List<Ellipse>();
        private List<Ellipse> _selectedVertexs = new List<Ellipse>();
        private List<Line> _linesOnCanvas = new List<Line>();
        private List<UIElement> graphVisualChildren = new List<UIElement>();
        private Graph _graph = new Graph();
        private List<Ellipse> _selectedVertexsToColorEdge = new List<Ellipse>();
        private int _algmode = 1;
        private int _buttonStartClickCount = 0;
        private SortedDictionary<VertexView, int> _userDjekstraAnswer = new SortedDictionary<VertexView, int>();
        private bool _isBoruvkaProcessed = false;
        enum AlgMode
        {
            DFS,
            BFS,
            Djekstra,
            Boruvka
        }

        public MainWindow()
        {
            InitializeComponent();
        }

       
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _learningMode = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _learningMode = false;
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
                RenderVertex(vertexView,Color.FromRgb(140,0,0));
            }
            else if (_deleteButtonIsPressed)
            {
                foreach (UIElement child in GraphCanvas.Children)
                {
                    if (child is Ellipse ellipse)
                    {
                        child.MouseLeftButtonDown += ChildDeleteElipse_MouseLeftButtonDown;
                    }
                    if(child is Line edge)
                    {
                        child.MouseRightButtonDown += ChildDeleteEdge_MouseRightButtonDown;
                    }    

                }
                _alreadyDeleted = false;
            }
            else if(_addEdgeButtonIsPressed)
            {
                foreach (UIElement child in GraphCanvas.Children)
                {
                    if (child is Ellipse ellipse)
                    {
                        child.MouseLeftButtonDown += AddEdgeLine_MouseLeftButtonDown;
                    }
                }
                if (_selectedVertexs.Count == 2)
                {
                    AddNewEdgeWindow addNewEdgeWindow = null;
                    if (_isBoruvkaProcessed == false)
                    {
                        addNewEdgeWindow = new AddNewEdgeWindow();
                    }
                    if (addNewEdgeWindow != null)
                    {
                        if (addNewEdgeWindow.ShowDialog() == true)
                        {
                            var V1 = _graph.GetVertexByNumber((int)_selectedVertexs[0].Tag);
                            var V2 = _graph.GetVertexByNumber((int)_selectedVertexs[1].Tag);
                            EdgeView edge = new EdgeView()
                            {

                                V1 = new VertexView() { Name = V1.Name, Number = V1.Number, X = V1.X, Y = V1.Y },
                                V2 = new VertexView() { Name = V2.Name, Number = V2.Number, X = V2.X, Y = V2.Y },
                                Distance = addNewEdgeWindow.Distance,
                                IsOriented = addNewEdgeWindow.IsOriented
                            };

                            try
                            {
                                if (_isBoruvkaProcessed == false)
                                {
                                    _graph.AddEdge(edge);
                                }
                            }
                            catch (Exception excp)
                            {
                                if (excp.Message == "Edge is already exists")
                                {
                                    foreach (var elps in _selectedVertexs)
                                    {
                                        elps.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                                    }
                                    _selectedVertexs.Clear();
                                    return;
                                }
                                else if (excp.Message == "Add new nonOriented edge")
                                {
                                    List<Line> linesToDelete = new List<Line>();
                                    foreach (UIElement child in GraphCanvas.Children)
                                    {
                                        if (child is Line line)
                                        {
                                            int firstVertexNumber = int.Parse(line.Tag.ToString().Split()[0]);
                                            int secondVertexNumber = int.Parse(line.Tag.ToString().Split()[1]);

                                            if (firstVertexNumber == edge.V2.Number && secondVertexNumber == edge.V1.Number)
                                            {
                                                linesToDelete.Add(line);

                                            }
                                        }
                                    }
                                    foreach (var line in linesToDelete)
                                    {
                                        GraphCanvas.Children.Remove(line);
                                    }
                                    foreach (var elps in _selectedVertexs)
                                    {
                                        elps.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                                    }
                                    _selectedVertexs.Clear();
                                    RenderEdge(false, edge);
                                    return;
                                }
                            }
                            RenderEdge(addNewEdgeWindow.IsOriented, edge);
                            foreach (var elps in _selectedVertexs)
                            {
                                elps.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                            }
                            _selectedVertexs.Clear();
                        }
                    }
                    else
                    {
                        var V1 = _graph.GetVertexByNumber((int)_selectedVertexs[0].Tag);
                        var V2 = _graph.GetVertexByNumber((int)_selectedVertexs[1].Tag);
                        EdgeView edge = new EdgeView()
                        {

                            V1 = new VertexView() { Name = V1.Name, Number = V1.Number, X = V1.X, Y = V1.Y },
                            V2 = new VertexView() { Name = V2.Name, Number = V2.Number, X = V2.X, Y = V2.Y },

                        };
                        RenderEdge(false, edge);
                        foreach (var elps in _selectedVertexs)
                        {
                            elps.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                        }
                        _selectedVertexs.Clear();
                    }
                    {

                    }
                }
            }
            if(_selectVertexButtonIsPressed)
            {
                foreach (UIElement child in GraphCanvas.Children)
                {
                    if (child is Ellipse ellipse)
                    {
                        child.MouseLeftButtonDown += ChildSelectVertexButton_MouseLeftButtonDown; ;
                    }
                }
            }
        }

        private void ChildSelectVertexButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_learningMode == false)
            {
                Ellipse vert = sender as Ellipse;
                if (_selectVertexButtonIsPressed)
                {
                    if (_selectedVertex != null)
                    {
                        _selectedVertex.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                    }

                    _selectedVertex = vert;
                    vert.Stroke = new SolidColorBrush(Color.FromRgb(0, 140, 0));

                }
            }
            else
            {
                Ellipse vert = sender as Ellipse;
                if (_selectVertexButtonIsPressed)
                {
                    if (vert != null)
                    {
                        if (!_selectedVertexsForAlg.Contains(vert) && (_algmode == 0 || _algmode == 1))
                        {
                            _selectedVertexsForAlg.Add(vert);
                            if(_selectedVertexsToColorEdge.Count<2)
                            {
                                _selectedVertexsToColorEdge.Add(vert);
                            }
                            if (_algmode == 0)
                            {
                                if (_selectedVertexsToColorEdge.Count == 2)
                                {
                                    foreach (var element in GraphCanvas.Children)
                                    {
                                        if (element is Line line)
                                        {
                                            int firstVertexNumber = int.Parse(line.Tag.ToString().Split()[0]);
                                            int secondVertexNumber = int.Parse(line.Tag.ToString().Split()[1]);
                                            if (firstVertexNumber == (int)_selectedVertexsToColorEdge[0].Tag
                                                && secondVertexNumber == (int)_selectedVertexsToColorEdge[1].Tag)
                                            {
                                                line.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 140));
                                            }
                                        }
                                    }
                                    _selectedVertexsToColorEdge.RemoveAt(0);
                                }
                            }
                            if(_algmode == 1)
                            {
                                foreach (var element in GraphCanvas.Children)
                                {
                                    if (element is Line line)
                                    {
                                        int firstVertexNumber = int.Parse(line.Tag.ToString().Split()[0]);
                                        if (firstVertexNumber == (int)vert.Tag)
                                        {
                                            line.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 140));
                                        }
                                    }
                                }
                            }
                            vert.Stroke = new SolidColorBrush(Color.FromRgb(0, 140, 0));
                        }
                    }
                }
            }
        }

        private void AddEdgeLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            if (_addEdgeButtonIsPressed)
            {
                bool addToSelectedVertexFlag = true;
                foreach (var elps in _selectedVertexs)
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
                        foreach (var elps in _selectedVertexs)
                        {
                            elps.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                        }
                        _selectedVertexs.Clear();
                    }
                }

            }
        }
        private void ChildDeleteEdge_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Line edge = sender as Line;
            if (edge != null)
            {
                List<Line> linesToDelete = new List<Line>();
                TextBlock _textBlockToDelete = new TextBlock();
                foreach (UIElement element in GraphCanvas.Children)
                {
                    if (element is Line line)
                    {
                        if (line.Tag.ToString() == edge.Tag.ToString())
                        {
                            linesToDelete.Add(line);
                        }
                    }
                    else if(element is TextBlock textBlock)
                    {
                        if(textBlock.Tag.ToString() == edge.Tag.ToString())
                        {
                            _textBlockToDelete = textBlock;
                        }
                    }
                }
                foreach (var line in linesToDelete)
                {
                    GraphCanvas.Children.Remove(line);
                }
                GraphCanvas.Children.Remove(_textBlockToDelete);
                int v1 = int.Parse(edge.Tag.ToString().Split()[0]);
                int v2 = int.Parse(edge.Tag.ToString().Split()[1]);

                _graph.DeleteEdge(v1, v2);
            }
        }
        private void ChildDeleteElipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_deleteButtonIsPressed)
            {
                Ellipse ellipse = sender as Ellipse;
                if (ellipse != null)
                {
                    GraphCanvas.Children.Remove(ellipse);
                    _selectedVertexs.Remove(ellipse);
                    List<TextBlock> textBlocksToDelete = new List<TextBlock>();
                    List<Line> linesToDelete = new List<Line>();
                    foreach (UIElement child in GraphCanvas.Children)
                    {
                        if (child is TextBlock textBlock)
                        {
                            if (textBlock.Tag == ellipse.Tag)
                            {
                                textBlocksToDelete.Add(textBlock);
                            }

                        }
                    }
                    foreach (var child in GraphCanvas.Children)
                    {
                        if (child is Line line)
                        {
                            int firstVertexNumber = int.Parse(line.Tag.ToString().Split()[0]);
                            int secondVertexNumber = int.Parse(line.Tag.ToString().Split()[1]);
                            if ((int)ellipse.Tag == firstVertexNumber || (int)ellipse.Tag == secondVertexNumber)
                            {
                                linesToDelete.Add(line);
                            }
                        }
                        else if(child is TextBlock text)
                        {
                            if(text.Tag.ToString().Split().Length == 2)
                            {
                                int firstVertexNumber = int.Parse(text.Tag.ToString().Split()[0]);
                                int secondVertexNumber = int.Parse(text.Tag.ToString().Split()[1]);
                                if ((int)ellipse.Tag == firstVertexNumber || (int)ellipse.Tag == secondVertexNumber)
                                {
                                    textBlocksToDelete.Add(text);
                                }
                            }
                        }
                    }
                    foreach (TextBlock text in textBlocksToDelete)
                    {
                        GraphCanvas.Children.Remove(text);
                    }
                    foreach (Line line in linesToDelete)
                    {
                        GraphCanvas.Children.Remove(line);
                    }

                }
                if (!_alreadyDeleted)
                {
                    _graph.DeleteVertex(_graph.GetVertexByNumber((int)ellipse.Tag));
                    _alreadyDeleted = true;
                }
            }
        }

    

    

        private Ellipse RenderVertex(VertexView vertex, Color color)
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
            mainLine.StrokeThickness = 4;
            mainLine.Tag = edge.V1.Number + " " + edge.V2.Number;
            GraphCanvas.Children.Add(mainLine);
            _linesOnCanvas.Add(mainLine);

            Line arrowLine1 = new Line();
            arrowLine1.X1 = pt2.X;
            arrowLine1.Y1 = pt2.Y;

            arrowLine1.X2 = pt3.X;
            arrowLine1.Y2 = pt3.Y;
            arrowLine1.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            arrowLine1.StrokeThickness = 4;
            arrowLine1.Tag = edge.V1.Number + " " + edge.V2.Number;
            GraphCanvas.Children.Add(arrowLine1);

            Line arrowLine2 = new Line();
            arrowLine2.X1 = pt2.X;
            arrowLine2.Y1 = pt2.Y;

            arrowLine2.X2 = pt4.X;
            arrowLine2.Y2 = pt4.Y;
            arrowLine2.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            arrowLine2.StrokeThickness = 4;
            arrowLine2.Tag = edge.V1.Number + " " + edge.V2.Number;
            GraphCanvas.Children.Add(arrowLine2);

            Point pt5 = new Point(
                Math.Abs(pt2.X + pt1.X) / 2,
                Math.Abs(pt2.Y + pt1.Y) / 2);

            if (_isBoruvkaProcessed == false)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = edge.Distance.ToString();
                textBlock.FontSize = 15;
                textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                Canvas.SetLeft(textBlock, pt5.X);
                Canvas.SetTop(textBlock, pt5.Y);
                textBlock.Tag = edge.V1.Number + " " + edge.V2.Number;
                GraphCanvas.Children.Add(textBlock);
            }
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
            mainLine.StrokeThickness = 4;
            mainLine.Tag = edge.V1.Number + " " + edge.V2.Number;
            GraphCanvas.Children.Add(mainLine);
            _linesOnCanvas.Add(mainLine);

            Point pt5 = new Point(
                Math.Abs(pt2.X + pt1.X) / 2,
                Math.Abs(pt2.Y + pt1.Y) / 2);

            if (_isBoruvkaProcessed == false)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = edge.Distance.ToString();
                textBlock.FontSize = 15;
                textBlock.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                Canvas.SetLeft(textBlock, pt5.X);
                Canvas.SetTop(textBlock, pt5.Y);
                textBlock.Tag = edge.V1.Number + " " + edge.V2.Number;
                GraphCanvas.Children.Add(textBlock);
            }
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
                //_selectedVertex.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                _selectedVertex = null;
            }
        }

        private void AddTextBlocksForDjekstra()
        {
            List<TextBlock> toAdd = new List<TextBlock>();
            foreach(VertexView vertex in _graph.V)
            {
                TextBlock text = new TextBlock();
                text.Tag = vertex.Number + " Dj";
                text.Text = "INF";
                text.FontSize = 18;
                text.Foreground = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                Canvas.SetLeft(text, vertex.X - 20);
                Canvas.SetTop(text, vertex.Y - 35);
                GraphCanvas.Children.Add(text);
            }
            
        }

        private async void StartAlgorithmButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (_learningMode == true && (_algmode == 2 || _algmode == 3))
            {
                _buttonStartClickCount+=1;
            }
            if (chooseAlgComboBox.SelectedIndex == 0)
            {
                if (_learningMode == true)
                {
                    _selectedVertex = _selectedVertexsForAlg.FirstOrDefault();
                }

                if (_selectedVertex != null)
                {
                    if (_learningMode == false)
                    {
                        foreach (var vertex in await _graph.DFS((int)_selectedVertex.Tag, GraphCanvas))
                        {
                            textBoxAnswer.Text += vertex.Name + "->";
                        }
                    }
                    else
                    {
                        string answer_comp = " ";
                        foreach (var vertex in _graph.DFSLearningMode((int)_selectedVertex.Tag))
                        {
                            answer_comp += vertex.Name + " ";
                        }
                        string answer_user = " ";
                        foreach (var elp in _selectedVertexsForAlg)
                        {
                            answer_user += elp.Tag.ToString() + " ";
                        }
                        if (answer_comp == answer_user)
                        {

                            MessageBox.Show(
                            "Задание решено верно",
                            "Сообщение",
                             MessageBoxButton.OK

                            );

                        }
                        else
                        {
                            MessageBox.Show(
                                "Задание решено неверно",
                                "Сообщение",
                                 MessageBoxButton.OK
                                 );
                        }
                        foreach (UIElement element in GraphCanvas.Children)
                        {
                            if (element is Line line)
                            {

                                line.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                            }
                            if (element is Ellipse ellipse)
                            {
                                ellipse.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                            }
                        }
                        _selectedVertex = null;
                        _selectedVertexsForAlg.Clear();
                        _selectedVertexsToColorEdge.Clear();
                    }
                }
            }
            else if (chooseAlgComboBox.SelectedIndex == 1)
            {
                if (_learningMode == true)
                {
                    _selectedVertex = _selectedVertexsForAlg.FirstOrDefault();
                }

                if (_selectedVertex != null)
                {
                    if (_learningMode == false)
                    {
                        foreach (var vertex in await _graph.BFS((int)_selectedVertex.Tag, GraphCanvas))
                        {
                            textBoxAnswer.Text += vertex.Name + "->";
                        }
                    }
                    else
                    {
                        string answer_comp = " ";
                        foreach (var vertex in _graph.BFSLearningMode((int)_selectedVertex.Tag))
                        {
                            answer_comp += vertex.Name + " ";
                        }
                        string answer_user = " ";
                        foreach (var elp in _selectedVertexsForAlg)
                        {
                            answer_user += elp.Tag.ToString() + " ";
                        }
                        if (answer_comp == answer_user)
                        {

                            MessageBox.Show(
                            "Задание решено верно",
                            "Сообщение",
                             MessageBoxButton.OK

                            );

                        }
                        else
                        {
                            MessageBox.Show(
                                "Задание решено неверно",
                                "Сообщение",
                                 MessageBoxButton.OK
                                 );
                        }
                        foreach (UIElement element in GraphCanvas.Children)
                        {
                            if (element is Line line)
                            {

                                line.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                            }
                            if (element is Ellipse ellipse)
                            {
                                ellipse.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                            }
                        }
                        _selectedVertex = null;
                        _selectedVertexsForAlg.Clear();
                        _selectedVertexsToColorEdge.Clear();

                    }
                }
            }
            else if (chooseAlgComboBox.SelectedIndex == 2)
            {
                if (_learningMode == true)
                {
                    if (_buttonStartClickCount == 1)
                    {
                        List<UIElement> toRemove = new List<UIElement>();
                        foreach (UIElement element in GraphCanvas.Children)
                        {
                            graphVisualChildren.Add(element);
                            if (element is Line line)
                            {
                                toRemove.Add(line);
                            }
                        }
                        foreach (var line in toRemove)
                        {
                            GraphCanvas.Children.Remove(line);
                        }
                        _isBoruvkaProcessed = true;
                    }
                    else
                    {
                        bool rightAnswer = true;
                        var answerGraphEdges = _graph.AlgBoruvka(GraphCanvas, true).Result.E;
                        List<EdgeView> userAnswer = new List<EdgeView>();
                        foreach (var element in GraphCanvas.Children)
                        {
                            if (element is Line line)
                            {
                                int firstVertexNumber = int.Parse(line.Tag.ToString().Split()[0]);
                                int secondVertexNumber = int.Parse(line.Tag.ToString().Split()[1]);
                                userAnswer.Add(new EdgeView()
                                {
                                    V1 = new VertexView() { Number = firstVertexNumber },
                                    V2 = new VertexView() { Number = secondVertexNumber }
                                });

                            }
                        }
                        List<EdgeView> toAdd = new List<EdgeView>();
                        foreach(var edge in userAnswer)
                        {
                            toAdd.Add(
                                new EdgeView()
                                { V1 = new VertexView() { Number = edge.V2.Number },
                                  V2 = new VertexView() { Number = edge.V1.Number }
                                }
                                );
                        }
                        foreach(var edge in toAdd)
                        {
                            userAnswer.Add(edge);
                        }
                        if (userAnswer.Count == answerGraphEdges.Count)
                        {
                            foreach (var edge in userAnswer)
                            {
                                bool IsThere = false;
                                foreach(var answerEdge in answerGraphEdges)
                                if (edge.V1.Number == answerEdge.V1.Number && edge.V2.Number == answerEdge.V2.Number)
                                {
                                        IsThere = true;
                                }
                                if(IsThere == false)
                                {
                                    rightAnswer = false;
                                }
                            }
                        }
                        else
                        {
                            rightAnswer = false;
                        }

                        if (rightAnswer == true)
                        {

                            MessageBox.Show(
                            "Задание решено верно",
                            "Сообщение",
                             MessageBoxButton.OK

                            );
                        }
                        else
                        {
                            MessageBox.Show(
                                   "Задание решено неверно",
                                   "Сообщение",
                                    MessageBoxButton.OK
                                    );
                        }
                        _isBoruvkaProcessed = false;
                        _buttonStartClickCount = 0;
                        List<UIElement> toDelete = new List<UIElement>();
                        foreach (UIElement element in GraphCanvas.Children)
                        {
                            toDelete.Add(element);
                        }
                        foreach (var element in toDelete)
                        {
                            GraphCanvas.Children.Remove(element);
                        }
                        foreach (var line in graphVisualChildren)
                        {
                            GraphCanvas.Children.Add(line);
                        }

                    }
                }
                else
                {
                    await _graph.AlgBoruvka(GraphCanvas, false);
                }
            }
            else if (chooseAlgComboBox.SelectedIndex == 3)
            {
                
                if (_selectedVertex != null) 
                {
                    if (_learningMode == true)
                    {
                        bool rightAnswer = true;
                        if (_buttonStartClickCount == 1)
                        {
                            AddTextBlocksForDjekstra();
                            foreach (var child in GraphCanvas.Children)
                            {
                                if (child is TextBlock text)
                                {
                                    if (text.Tag.ToString().Split().Length == 2)
                                    {
                                        text.MouseLeftButtonDown += Text_MouseLeftButtonDown;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var compAnswer = await AlgDjekstr.AlgDjekstra((int)_selectedVertex.Tag, _graph, GraphCanvas, true);
                            if (compAnswer.Count == _userDjekstraAnswer.Count)
                            {
                                foreach (var vertUser in _userDjekstraAnswer)
                                {
                                    bool IsThere = true;
                                    foreach (var answerVert in compAnswer)
                                        if (vertUser.Key == answerVert.Key && vertUser.Value != answerVert.Value)
                                        {
                                            rightAnswer = false;
                                        }
                                    if (IsThere == false)
                                    {
                                        rightAnswer = false;
                                    }
                                }
                                if (rightAnswer == true)
                                {

                                    MessageBox.Show(
                                    "Задание решено верно",
                                    "Сообщение",
                                     MessageBoxButton.OK

                                    );
                                }
                                else
                                {
                                    MessageBox.Show(
                                           "Задание решено неверно",
                                           "Сообщение",
                                            MessageBoxButton.OK
                                            );
                                }
                                

                            }
                        }
                    }
                    else
                    {
                        AddTextBlocksForDjekstra();
                        foreach (var pair in await AlgDjekstr.AlgDjekstra((int)_selectedVertex.Tag, _graph, GraphCanvas, false))
                        {
                            textBoxAnswer.Text += pair.Key.Name + "-" + pair.Value.ToString() + "\n";
                        }
                    }                  
                }
            }


        }

        private void Text_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock text = sender as TextBlock;
            ChangeMarkWindow changeMarkWindow = new ChangeMarkWindow();
            changeMarkWindow.markValueTextBlock.Text = text.Text;
            if (changeMarkWindow.ShowDialog() == true)
            {
                if (changeMarkWindow.isToSubmitMark == false)
                {
                    text.Text = changeMarkWindow.markValueTextBlock.Text;
                }
                else
                {
                    text.Text = changeMarkWindow.markValueTextBlock.Text;
                    text.Foreground = new SolidColorBrush(Color.FromRgb(0, 140, 0));
                    _userDjekstraAnswer.Add(new VertexView(int.Parse(text.Tag.ToString().Split()[0]), 
                        text.Tag.ToString().Split()[0]), int.Parse(text.Text));

                }
            }
        }
        
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            List<UIElement> toRemove = new List<UIElement>();
            foreach (UIElement element in GraphCanvas.Children)
            {
                if (element.Visibility == Visibility.Hidden)
                {
                    element.Visibility = Visibility.Visible;
                    element.IsEnabled = true;
                }
                if(element is Line line)
                {

                    line.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    
                }
                if(element is Ellipse ellipse)
                {
                    ellipse.Stroke = new SolidColorBrush(Color.FromRgb(140, 0, 0));
                }
                if(element is TextBlock textBlock)
                {
                    if(textBlock.Tag.ToString().Split().Length == 2)
                    {
                        if (textBlock.Tag.ToString().Split()[1] == "Dj")
                        {
                            toRemove.Add(textBlock);
                        }
                    }
                }
            }
            foreach(var element in toRemove)
            {
                GraphCanvas.Children.Remove(element);
            }
            textBoxAnswer.Text = "";
            _selectedVertex = null;
            _selectedVertexsForAlg.Clear();
            _selectedVertexsToColorEdge.Clear();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            _learningMode = true;
        }

        private void chooseAlgComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if(comboBox!=null)
            {
                _algmode = comboBox.SelectedIndex;
            } 
            
        }

        private void ReadAndSetGrapg(string filePath)
        {
            List<VertexView> vertices = new List<VertexView>();
            List<EdgeView> edges = new List<EdgeView>();
            int n;
            using (StreamReader reader = new StreamReader(filePath))
            {
                n = int.Parse(reader.ReadLine());
                for (int i = 0; i < n; i++)
                {
                    string[] vertexString = reader.ReadLine().Split();
                    vertices.Add(new VertexView()
                    {
                        IsVisited = false,
                        Name = vertexString[0],
                        Number = int.Parse(vertexString[1]),
                        X = double.Parse(vertexString[2]),
                        Y = double.Parse(vertexString[3])
                    });
                }
                n = int.Parse(reader.ReadLine());
                for (int i = 0; i < n; i++)
                {
                    string[] edgesString = reader.ReadLine().Split();
                    VertexView v1 = vertices.Where(item => item.Number == int.Parse(edgesString[0])).FirstOrDefault();
                    VertexView v2 = vertices.Where(item => item.Number == int.Parse(edgesString[1])).FirstOrDefault();
                    bool? isOriented;
                    if (edgesString[3] == "0")
                    {
                        isOriented = false;
                    }
                    else
                    {
                        isOriented = true;
                    }
                    edges.Add(new EdgeView()
                    {
                        V1 = v1,
                        V2 = v2,
                        Distance = int.Parse(edgesString[2]),
                        IsOriented = isOriented
                    });
                }
            }
            
            foreach (var vertex in vertices)
            {
                _graph.AddVertex(vertex);
                RenderVertex(vertex, Color.FromRgb(140, 0, 0));
            }
            foreach(var edge in edges)
            {
                _graph.AddEdge(edge);
                RenderEdge(edge.IsOriented, edge);
            }
            
            
        }
        private void GraphExample1Button_Click(object sender, RoutedEventArgs e)
        {
            GraphCanvas.Children.Clear();
            _graph = null;
            _graph = new Graph();
            ReadAndSetGrapg(@"..\..\Resources\GraphExample1.txt");
            
        }

        private void GraphExample2Button_Click(object sender, RoutedEventArgs e)
        {
            GraphCanvas.Children.Clear();
            _graph = null;
            _graph = new Graph();
            ReadAndSetGrapg(@"..\..\Resources\GraphExample2.txt");
        }

        private void GraphExample3Button_Click(object sender, RoutedEventArgs e)
        {
            GraphCanvas.Children.Clear();
            _graph = null;
            _graph = new Graph();
            ReadAndSetGrapg(@"..\..\Resources\GraphExample3.txt");
        }

        private void SaveGraphButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog= new SaveFileDialog();
            if(saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                SaveGraphInFile(filePath);
            }

        }
        private void SaveGraphInFile(string fileName)
        {
            using(StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(_graph.V.Count);
                foreach(var vertex in _graph.V)
                {
                    writer.WriteLine(vertex.Name + " " + vertex.Number + " " + vertex.X + " " + vertex.Y);
                }
                List<EdgeView> toPrint = new List<EdgeView>();
                //foreach(var edge in _graph.E)
                //{
                //    toPrint.Add(edge);
                //}
                List<EdgeView> passedEdges = new List<EdgeView>();
                foreach(var edge in _graph.E)
                {
                    var reversedEdge = toPrint.Where(item => item.V1.Number == edge.V2.Number
                    && item.V2.Number == edge.V1.Number).FirstOrDefault();
                    if (!passedEdges.Contains(edge) && !passedEdges.Contains(reversedEdge))
                    {
                        edge.IsOriented = false;
                        if(reversedEdge == null)
                        {
                            edge.IsOriented = true;
                        }
                        toPrint.Add(edge);
                        passedEdges.Add(edge);
                        if(reversedEdge!=null)
                        {
                            passedEdges.Add(reversedEdge);
                        }
                    }
                }
                writer.WriteLine(toPrint.Count);
                foreach(var edge in toPrint)
                {
                    writer.WriteLine(edge.V1.Name + " " + edge.V2.Name + " " + edge.Distance + " " + (edge.IsOriented == true ? "0" : "1"));
                } 
            }
        }
        private void LoadGraphButton_Click(object sender, RoutedEventArgs e)
        {
            GraphCanvas.Children.Clear();
            _graph = null;
            _graph = new Graph();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                ReadAndSetGrapg(filePath);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InformationButton_Click(object sender, RoutedEventArgs e)
        {
            string message = "Формат входных данных:\n" +
                             "<Колличество вершин>\n" +
                             "<Название вершины> <Номер вершины> <X координата> <Y координата> \n" +
                             "... \n\n" +
                             "Количество ребер\n" +
                             "<Первая вершина> <Вторая вершина> <Расстояние> <Ориентрированность(0/1)> \n" +
                             "... \n\n" +
                             "Цвета:\n" +
                             "красный - стандартный;\n" +
                             "зеленый - выбранная вершина для алгоритма;\n" +
                             "синий - выбранная вершна при соединении ребер;\n" +
                             "серо-зеленый - пройденная вершина в алгоритмах обхода.\n\n" +
                             "Алгоритм обхода в глубину и ширину:\n"+
                             "Обычный режим:\n"+
                             "1. Выберите вершину\n" +
                             "2. Выберите алгоритм\n" +
                             "3. Нажмите кнопку выполнить\n"+
                             "Обучаюший режим:\n" +
                             "1. Выберите алгоритм\n" +
                             "2. Выберите вершины\n" +
                             "3. Нажмите кнопку выполнить\n" +
                             "... \n\n" +
                             "Приложение сделал студент 341 группы Шкодин Максим (2020).\n";
                              
            MessageBox.Show(message, "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

