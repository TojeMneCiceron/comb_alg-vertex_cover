using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace vertex_cover
{
    class Program
    {
        static Graph ReadGraph()
        {
            StreamReader sr = new StreamReader(@"C:\Users\Пользователь\source\repos\vertex_cover\vertex_cover\input.txt");
            int n = int.Parse(sr.ReadLine());

            int[,] matrix = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                var temp = sr.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(temp[j]);
                }
            }

            sr.Close();
            return new Graph(n, matrix);
        }

        static void Print(List<Vertex> vertices)
        {
            //Console.WriteLine();

            //vertices.Sort((x, y) => x.Name.CompareTo(y.Name));

            Console.WriteLine();
            Console.WriteLine("\t" + vertices.Count);
            Console.WriteLine();

            if (vertices.Count == 0)
                Console.WriteLine("пусто...");
            else
                Console.WriteLine(vertices.Select(x => x.ToString()).Aggregate((x, y) => $"{x} {y}"));
        }

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            List<Vertex> res = null;

            Console.WriteLine("Способ ввода\n1 - Файл\n2 - Генерация");
            bool file = Console.ReadLine() == "1";

            Console.WriteLine("Размер популяции?");
            int p_size = int.Parse(Console.ReadLine());
            Console.WriteLine("Количество эпох?");
            int epochs = int.Parse(Console.ReadLine());

            while (true)
            {
                Graph graph;

                if (file)
                    graph = ReadGraph();
                else
                    graph = new Graph(12);
                Console.WriteLine(graph);

                Console.WriteLine("Полный перебор:");
                stopwatch.Start();
                res = BruteForce.Solve(graph);
                stopwatch.Stop();
                res.Sort((x, y) => x.Name.CompareTo(y.Name));
                Print(res);
                Console.WriteLine($"Время: {(double)stopwatch.ElapsedMilliseconds / 1000} с");

                Console.WriteLine("\nЖадный алгоритм:");
                stopwatch = new Stopwatch();
                stopwatch.Start();
                res = Greedy.Solve(graph);
                stopwatch.Stop();
                Print(res);
                Console.WriteLine($"Время: {(double)stopwatch.ElapsedMilliseconds / 1000} с");

                Console.WriteLine("\nГенетический алгоритм:");
                stopwatch = new Stopwatch();
                stopwatch.Start();
                res = Genetic.Solve(graph, p_size, epochs, p_size / 10);
                stopwatch.Stop();
                Print(res);
                Console.WriteLine($"Время: {(double)stopwatch.ElapsedMilliseconds / 1000} с");

                if (file)
                    break;

                Console.WriteLine("\nЕще? (y/n)");
                if (Console.ReadLine() == "n")
                    break;

                Console.WriteLine();
            }
        }
    }
}
