using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace vertex_cover
{
    class BruteForce
    {

        static int ChangeBit(int x)
        {
            return x == 0 ? 1 : 0;
        }


        static void Print(List<int> S)
        {
            Console.WriteLine(S.Select(x => x.ToString()).Aggregate((x, y) => $"{x} {y}"));
        }

        static bool Covers(List<Vertex> vertices, List<Edge> edges)
        {
            foreach (Vertex v in vertices)
            {
                var incEdges = edges.Where(x => x.From == v || x.To == v).ToList();
                foreach (Edge e in incEdges)
                {
                    edges.Remove(e);
                }
            }

            return edges.Count == 0;
        }
        
        static int VCSize(List<int> S)
        {
            return S.Sum();
        }

        static void PrintSV(List<Vertex> S)
        {
            S.Sort((x, y) => x.Name.CompareTo(y.Name));
            Console.WriteLine(S.Select(x => x.ToString()).Aggregate((x, y) => $"{x} {y}"));
        }

        public static List<Vertex> Solve(Graph graph)
        {
            var vertices = graph.Vertices;
            var edges = graph.Edges;

            List<Vertex> res = new List<Vertex>();
            int min = graph.Vertices.Count + 1;

            List<Vertex> S_vertices = new List<Vertex>();

            List<int> S = new List<int>();
            List<int> B = new List<int>();
            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                S.Add(0);
                B.Add(i + 1);
            }

            B.Add(B.Count + 1);
            B.Add(B.Count + 1);

            int x;

            do
            {
                //Print(S);

                //if (S_vertices.Count > 0)
                //    PrintSV(S_vertices);
                
                int vcSize = VCSize(S);

                if (vcSize < min && Covers(S_vertices, new List<Edge>(edges)))
                {
                    min = vcSize;
                    res = new List<Vertex>(S_vertices);
                }

                x = B[0];
                B[0] = 1;
                B[x - 1] = B[x];
                B[x] = x + 1;
                if (x <= graph.Vertices.Count)
                {
                    S[x - 1] = ChangeBit(S[x - 1]);

                    if (S[x - 1] == 1)
                    {
                        S_vertices.Add(vertices[x - 1]);
                    }
                    else
                    {
                        S_vertices.Remove(vertices[x - 1]);
                    }
                }

                //Console.WriteLine();
            }
            while (x <= graph.Vertices.Count);

            return res;
        }
    }
}
