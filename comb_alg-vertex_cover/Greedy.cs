using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace vertex_cover
{
    class Greedy
    {
        public static List<Vertex> Solve(Graph graph)
        {
            var vertices = new List<Vertex>(graph.Vertices);
            var edges = new List<Edge>(graph.Edges);

            //vertices.Sort((x, y) => y.Degree.CompareTo(x.Degree));

            List<Vertex> res = new List<Vertex>();

            while (edges.Count > 0)
            {
                vertices.Sort((x, y) => y.Degree.CompareTo(x.Degree));
                Vertex v = vertices.First();
                res.Add(v);

                var incEdges = edges.Where(x => x.From == v || x.To == v).ToList();
                foreach (Edge e in incEdges)
                {
                    edges.Remove(e);
                    e.From.Degree--;
                    e.To.Degree--;
                }
            }

            return res;
        }
    }
}
