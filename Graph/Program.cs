using System;
using System.Runtime.ExceptionServices;

namespace Graph
{
    class Program
    {
        public static void Menu()
        {
            Console.WriteLine(@"1.Добавить вершину
2.Добавить ребро
3.Удалить вершину
4.Удалить ребро
5.Вывести список смежности в файл
6.Вывести вершины и ребра
7.Вывести смежные вершины
8.Вывести не смежные вершины
9.Получить неориентрированный граф
10.Поиск в глубину
11.Поиск компонент связанности
12.Поиск компонент сильной связанности
13.Алгоритм Буровки
14.Вершина с кратчайшей суммой расстояний.
15.Найти центр графа.
16.Минимальные расстояния между любыми парами вершин.
17.Поиск максимального потока в сети
18.Выход");
        }
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            bool flag = true;
            Menu();
            while (flag)
            {
                Console.WriteLine("Ваш выбор: ");
                int n = int.Parse(Console.ReadLine());
                switch (n)
                {
                    case 1:
                        Console.WriteLine("Введите название вершины: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Вершина добавлена с номером {0}", graph.AddVertex(name));
                        break;
                    case 2:
                        Console.WriteLine("Введите первую вершину: ");
                        string v1 = Console.ReadLine();
                        Console.WriteLine("Введите вторую вершину: ");
                        string v2 = Console.ReadLine();
                        Console.WriteLine("Введите расстояние: ");
                        int d = int.Parse(Console.ReadLine());
                        graph.AddEdge(v1, v2, d);
                        break;
                    case 3:
                        Console.WriteLine("Введите название вершины: ");
                        string vert = Console.ReadLine();
                        graph.DeleteVertex(vert);
                        break;
                    case 4:
                        Console.WriteLine("Введите первую вершину: ");
                        string v1_ = Console.ReadLine();
                        Console.WriteLine("Введите вторую вершину: ");
                        string v2_ = Console.ReadLine();
                        graph.DeleteEdge(v1_, v2_);
                        break;
                    case 5:
                        graph.WriteMatrix("input.txt");
                        break;
                    case 6:
                        graph.Print();
                        break;
                    case 7:
                        Console.WriteLine("Введите вершину: ");
                        string vertex = Console.ReadLine();
                        foreach (var v in graph.FindАdjacentVertexs(vertex))
                        {
                            Console.Write(v.Name + " ");
                        }
                        Console.WriteLine();
                        break;
                    case 8:
                        Console.WriteLine("Введите вершину: ");
                        vertex = Console.ReadLine();
                        foreach (var v in graph.FindNonАdjacentVertexs(vertex))
                        {
                            Console.Write(v.Name + " ");
                        }
                        Console.WriteLine();
                        break;
                    case 9:
                        Graph graph1 = graph.GetdisorientedGraph();
                        graph1.Print();
                        break;
                    case 10:
                        Console.WriteLine("Введите вершину:");
                        var ve = Console.ReadLine();
                        foreach (var ver in graph.DFS(ve))
                        {
                            Console.WriteLine(ver.Name);
                        }
                        break;
                    case 11:
                        int cnt = 0;
                        if (graph.IsDisorientedGraph())
                        {
                            foreach (var comp in graph.FindRelatedComponents())
                            {
                                cnt++;
                                Console.Write("{0} компонента связанности: ", cnt);
                                foreach (var _vertex in comp)
                                {
                                    Console.Write(_vertex.Name + " ");
                                }
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            foreach (var comp in graph.FindStrongRelatedComponents())
                            {
                                cnt++;
                                Console.Write("{0} компонента сильной связанности: ", cnt);
                                foreach (var _vertex in comp)
                                {
                                    Console.Write(_vertex + " ");
                                }
                                Console.WriteLine();
                            }
                        }
                        break;
                    case 12:
                        cnt = 0;
                        foreach (var comp in graph.FindStrongRelatedComponents())
                        {
                            cnt++;
                            Console.Write("{0} компонента сильной связанности: ", cnt);
                            foreach (var _vertex in comp)
                            {
                                Console.Write(_vertex + " ");
                            }
                            Console.WriteLine();
                        }
                        break;
                    case 13:
                        Graph copy = new Graph(graph);
                        graph = graph.GetdisorientedGraph();
                        Graph minGraph = graph.AlgBoruvka();
                        graph = copy;
                        minGraph.Print();
                        break;
                    case 14:
                        Console.WriteLine(graph.FindVertexWithMinDistancees().Name);
                        break;
                    case 15:
                        Console.Write("Центр графа: ");
                        foreach (var verte in graph.FindCenter())
                        {
                            Console.Write(verte.Name + " ");
                        }
                        Console.WriteLine();
                        break;
                    case 16:
                        Console.WriteLine("Минимальные расстояния: ");
                        foreach(var edge in graph.GetMinDistancesForEachPair())
                        {
                            Console.WriteLine(edge.V1.Name + " " + edge.V2.Name + " " + edge.Distance);
                        }
                        break;
                    case 17:
                        Console.WriteLine("Введите первую вершину: ");
                        v1 = Console.ReadLine();
                        Console.WriteLine("Введите вторую вершину: ");
                        v2 = Console.ReadLine();
                        Console.Write("Максимальный поток: {0} ",graph.maxFlow(v1,v2));
                        Console.WriteLine();
                        break;
                    case 18:
                        flag = false;
                        break;

                }
            }

        }
    }
}
