using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vertex_cover
{
    class Ind
    {
        public List<int> Genes { get; set; } = new List<int>();

        public Ind(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Random r = new Random();
                Genes.Add(r.Next(0, 2));
            }
        }

        public Ind(List<int> genes)
        {
            Genes = genes;
        }

        public int Fitness { get; set; }
    }
    class Genetic
    {
        static Random random = new Random();

        static int ChangeBit(int x)
        {
            return x == 0 ? 1 : 0;
        }

        static (Ind, Ind) Crossover(List<int> p1, List<int> p2)
        {
            int c_point = random.Next(0, p1.Count); //or int i = random.Next(1, p1.Count - 1);

            List<int> c1 = new List<int>();
            List<int> c2 = new List<int>();

            for (int i = 0; i < c_point; i++)
            {
                c1.Add(p1[i]);
                c2.Add(p2[i]);
            }

            for (int i = c_point; i < p1.Count; i++)
            {
                c1.Add(p2[i]);
                c2.Add(p1[i]);
            }

            return (new Ind(c1), new Ind(c2));
        }

        static List<int> Mutate(List<int> x)
        {
            int i = random.Next(0, x.Count);
            x[i] = ChangeBit(x[i]);
            return x;
        }

        static int Fitness(List<int> x, List<Vertex> v, List<Edge> edges)
        {
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] == 1)
                {
                    var incEdges = edges.Where(y => y.From == v[i] || y.To == v[i]).ToList();
                    foreach (Edge e in incEdges)
                    {
                        edges.Remove(e);
                    }
                }
            }

            return edges.Count == 0 ? x.Count - x.Sum() : 0;
        }

        static List<Vertex> GetResult(List<int> genes, List<Vertex> vertices)
        {
            List<Vertex> res = new List<Vertex>();
            for (int i = 0; i < genes.Count; i++)
                if (genes[i] == 1)
                    res.Add(vertices[i]);

            return res;
        }

        public static List<Vertex> Solve(Graph graph, int p_size, int epochs, int elite_count)
        {
            //создаем начальную популяцию
            List<Ind> population = new List<Ind>();
            for (int i = 0; i < p_size; i++)
                population.Add(new Ind(graph.Vertices.Count));

            foreach (Ind ind in population)
            {
                ind.Fitness = Fitness(ind.Genes, graph.Vertices, new List<Edge>(graph.Edges));
            }

            for (int i = 0; i < epochs; i++)
            {
                List<Ind> new_population = new List<Ind>();
                population.Sort((x, y) => y.Fitness.CompareTo(x.Fitness));

                //отбираем лучших для след поколения
                for (int j = 0; j < elite_count; j++)
                    new_population.Add(population[j]);

                //выбираем скрещиваемых особей по турнирному принципу
                List<Ind> parents = new List<Ind>();
                for (int j = 0; j < p_size; j++)
                {
                    Ind ind1 = population[random.Next(0, p_size)];
                    Ind ind2 = population[random.Next(0, p_size)];

                    parents.Add(ind1.Fitness > ind2.Fitness ? ind1 : ind2);
                }


                List<Ind> children = new List<Ind>();
                //скрещиваем 
                for (int j = 0; j < p_size; j++)
                {
                    Ind p1 = parents[random.Next(0, p_size)];

                    Ind p2 = null;

                    do
                        p2 = parents[random.Next(0, p_size)];
                    while (p2 == p1);

                    Ind c1, c2;
                    (c1, c2) = Crossover(p1.Genes, p2.Genes);

                    c1.Fitness = Fitness(c1.Genes, graph.Vertices, new List<Edge>(graph.Edges));
                    c2.Fitness = Fitness(c2.Genes, graph.Vertices, new List<Edge>(graph.Edges));

                    children.Add(c1);
                    children.Add(c2);
                }

                //мутация
                double mutationRate = 1 / (double)p_size;
                for (int j = 0; j < children.Count; j++)
                {
                    double r = random.NextDouble();
                    if (r < mutationRate)
                    {
                        children[j].Genes = Mutate(children[j].Genes);
                        children[j].Fitness = Fitness(children[j].Genes, graph.Vertices, new List<Edge>(graph.Edges));
                    }
                }

                //собираем новую популяцию
                children.Sort((x, y) => y.Fitness.CompareTo(x.Fitness));

                for (int j = 0; j < population.Count; j++)
                {
                    if (new_population.Count == p_size)
                        break;

                    new_population.Add(children[j]);

                    //if (!new_population.Contains(population[j]))
                    //    new_population.Add(population[j]);
                }

                population = new_population;
            }

            population.Sort((x, y) => y.Fitness.CompareTo(x.Fitness));
            return GetResult(population.First().Genes, graph.Vertices);
        }
    }
}
