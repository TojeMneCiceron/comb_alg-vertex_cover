using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace vertex_cover
{
    class Vertex
    {
        public int Name { get; set; }
        public int Degree { get; set; }

        public Vertex(int name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }

    class Edge
    {
        public Edge(Vertex from, Vertex to)
        {
            From = from;
            To = to;
        }

        public Vertex From { get; set; }
        public Vertex To { get; set; }

        public override string ToString()
        {
            return $"{From}-{To}";
        }
    }

    class Graph
    {
        int N;
        public List<Vertex> Vertices { get; set; } = new List<Vertex>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public int[,] Matrix { get; set; }

        public Graph(int n, int[,] matrix)
        {
            N = n;
            Matrix = matrix;
            //Print();
            FillGraph();
        }

        public Graph(int n)
        {
            N = n;
            GenerateMatrix();
            FillGraph();
        }

        private void GenerateMatrix()
        {
            Matrix = new int[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = i; j < N; j++)
                {
                    if (i == j)
                    {
                        Matrix[i, j] = 0;
                        continue;
                    }

                    Random random = new Random();
                    //Matrix[i, j] = random.Next(0, 2);
                    Matrix[i, j] = (int)Math.Round(0.7 - random.NextDouble());
                    Matrix[j, i] = Matrix[i, j];
                }
            }
        }

        //public void Print()
        //{
        //    for (int i = 0; i < N; i++)
        //    {
        //        for (int j = 0; j < N; j++)
        //            Console.Write(Matrix[i, j]);
        //        Console.WriteLine();
        //    }
        //}

        private void FillGraph()
        {
            for (int i = 1; i <= N; i++)
            {
                Vertices.Add(new Vertex(i));
            }

            for (int i = 0; i < N; i++)
                for (int j = i + 1; j < N; j++)
                    if (Matrix[i, j] == 1)
                    {
                        Edges.Add(new Edge(Vertices[i], Vertices[j]));
                        Vertices[i].Degree++;
                        Vertices[j].Degree++;
                    }
        }

        public override string ToString()
        {
            string res = "";

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                    res += Matrix[i, j] + ",";

                res += "\n";
            }
            return res;
        }
    }
}
