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
                        graph.AddEdge(v1,v2,d);
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
