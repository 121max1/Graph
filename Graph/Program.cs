using System;


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
7.Выход");
        }
        static void Main(string[] args)
        {
            Graph graph = new Graph("input.txt");
            bool flag = true;
            Menu();
            while (flag)
            {
                Console.WriteLine("Ваш выбор: ");
                int n = int.Parse(Console.ReadLine());
                switch(n)
                {
                    case 1:
                        Console.WriteLine("Вершина добавлена с номером {0}", graph.AddVertex());
                        break;
                    case 2:
                        Console.WriteLine("Введите первую вершину: ");
                        int v1 = int.Parse(Console.ReadLine());
                        Console.WriteLine("Введите вторую вершину: ");
                        int v2 = int.Parse(Console.ReadLine());
                        Console.WriteLine("Введите расстояние: ");
                        int d = int.Parse(Console.ReadLine());
                        graph.AddEdge(v1,v2,d);
                        break;
                    case 3:
                        Console.WriteLine("Введите номер вершины: ");
                        int vert = int.Parse(Console.ReadLine());
                        graph.DeleteVertex(vert);
                        break;
                    case 4:
                        Console.WriteLine("Введите первую вершину: ");
                        int v1_ = int.Parse(Console.ReadLine());
                        Console.WriteLine("Введите вторую вершину: ");
                        int v2_ = int.Parse(Console.ReadLine());
                        graph.DeleteEdge(v1_,v2_);
                        break;
                    case 5:
                        graph.WriteMatrix("input.txt");
                        break;
                    case 6:
                        graph.Print();
                        break;
                    case 7:
                        flag=false;
                        break;

                }
            }

        }
    }
}
