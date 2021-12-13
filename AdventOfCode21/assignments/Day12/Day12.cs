using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class Day12 : Common
    {
        public Day12() : base("Day12") {}

        public class CaveGraph
        {
            public class Node
            {
                public string Name { get; init; }
                public string[] Others { get; init; }
                public bool isLargeCave { get; init; }

                public Node(string name, string[] adjacent) 
                {
                    Name = name;
                    isLargeCave = name.ToUpper() == name;
                    Others = adjacent;
                }
            }

            public Dictionary<string,Node> Nodes { get; init; }
            public Node Start  { get => Nodes["start"]; }
            public Node End    { get => Nodes["end"]; }
        }

        public class Branch
        {
            List<CaveGraph.Node> path { get; init; } = new List<CaveGraph.Node>();
            public Dictionary<string,int> Visited { get; init; } = new Dictionary<string,int>();

            public bool CanVisit(CaveGraph.Node node)
            {
                return (!Visited.ContainsKey(node.Name) || node.isLargeCave);
            }
            public void Visit(CaveGraph.Node node) 
            {
                if( CanVisit(node) )
                {
                    Visited.TryAdd(node.Name,1);
                    path.Add(node);
                }
            }

            bool isSpecial(string s) => s == "start";

            bool VisitedSmallCaveTwice => Visited.FirstOrDefault(x => x.Key == x.Key.ToLower() && x.Value > 1).Value > 1;

            public bool CanVisit2(CaveGraph.Node node)
            {
                if (!isSpecial(node.Name))
                {
                    if (node.isLargeCave || !Visited.ContainsKey(node.Name))
                    {
                        return true;
                    }
                    else 
                        return (!VisitedSmallCaveTwice && Visited[node.Name] <= 1);
                }
                return !Visited.ContainsKey(node.Name);
            }
            public void Visit2(CaveGraph.Node node)
            {
                if (CanVisit2(node))
                {
                    int count = 0;
                    if (!Visited.TryGetValue(node.Name, out count))
                    {
                        Visited.Add(node.Name, 1);
                    }
                    else
                        Visited[node.Name]++;

                    path.Add(node);
                }
            }

            public CaveGraph.Node Current => path.Last();
            public string Debug => new string(path.Select(x=>x.Name).Aggregate((a, x) => a+","+x).ToArray());
            public string GetSequenceString()
            {
                string sequence = "";
                path.ForEach(x => sequence += x.Name);
                return sequence;
            }

            public bool IsEnded => Visited.ContainsKey("end");

            public Branch Clone()
            {
                return new Branch
                {
                    path = new List<CaveGraph.Node>(this.path),
                    Visited = new Dictionary<string,int>(this.Visited),
                };
            }
        }

        public class Graph
        {
            public class Node
            {
                string      Name { get; init; }
                string[]    Others { get; init; }
            }
            public class Edge 
            { 
                public int Node0 { get; init; } 
                public int Node1 { get; init; } 
            }
            public string[] Nodes { get; init; }
            public Edge[]   Edges { get; init; }
            public int      StartNode { get; init; }
            public int      EndNode { get; init; }
            public bool IsLargeCave(string node) => node.ToUpper() == node;
        }

        public CaveGraph GetInputSequence(bool simpleInput = false)
        {
            return InitReader(simpleInput, (reader) =>
            {
                Dictionary<string, HashSet<string>> nodes = new Dictionary<string,HashSet<string>>();
                //List<Graph.Edge> edges = new List<Graph.Edge>();

                var lineStream = ParseLines(reader);
                foreach (var line in lineStream)
                {
                    var edgetokens = line?.Split("-");
                    if( !nodes.ContainsKey(edgetokens[0]) )
                    {
                        nodes.Add(edgetokens[0], new HashSet<string>());
                    }
                    if (!nodes.ContainsKey(edgetokens[1]))
                    {
                        nodes.Add(edgetokens[1], new HashSet<string>());
                    }

                    nodes[edgetokens[0]].Add(edgetokens[1]);
                    nodes[edgetokens[1]].Add(edgetokens[0]);
                }

                return new CaveGraph
                {
                    Nodes = nodes
                        .Select(e => new CaveGraph.Node(e.Key, e.Value.ToArray()))
                        .ToDictionary(x => x.Name),
                };
            });
        }

        int GetUniquePaths(CaveGraph g)
        {
            List<Branch> branches = new List<Branch>();
            HashSet<string> uniquePaths = new HashSet<string>();
            var branch = new Branch();
            branch.Visit(g.Start);
            branches.Add(branch);

            while(branches.Count > 0)
            {
                List<Branch> newBranches = new List<Branch>();
                foreach(var br in branches)
                {
                    if (br.IsEnded)
                    {
                        uniquePaths.Add(br.GetSequenceString());
                    }
                    else
                    {
                        foreach (var a in br.Current.Others)
                        {
                            var otherNode = g.Nodes[a];
                            if ( br.CanVisit(otherNode) )
                            {
                                var newBranch = br.Clone();
                                newBranch.Visit(otherNode);
                                newBranches.Add(newBranch);
                            }
                        }
                    }                    
                }
                branches = newBranches;
            }

            return uniquePaths.Count;
        }

        int GetUniquePaths2(CaveGraph g)
        {
            List<Branch> branches = new List<Branch>();
            HashSet<string> uniquePaths = new HashSet<string>();
            var branch = new Branch();
            branch.Visit2(g.Start);
            branches.Add(branch);

            while (branches.Count > 0)
            {
                List<Branch> newBranches = new List<Branch>();
                foreach (var br in branches)
                {
                    if (br.IsEnded)
                    {
                        uniquePaths.Add(br.GetSequenceString());
                    }
                    else
                    {
                        foreach (var a in br.Current.Others)
                        {
                            var otherNode = g.Nodes[a];
                            if (br.CanVisit2(otherNode))
                            {
                                var newBranch = br.Clone();
                                newBranch.Visit2(otherNode);
                                newBranches.Add(newBranch);
                            }
                        }
                    }
                }
                branches = newBranches;
            }

            return uniquePaths.Count;
        }

        bool UseSimpleInput = false;
        public override string GetResult1()
        {
            var graph = GetInputSequence(UseSimpleInput);

            var result = GetUniquePaths(graph);
            
            return $"{result}";
        }


        public override string GetResult2()
        {
            var graph = GetInputSequence(UseSimpleInput);

            var result = GetUniquePaths2(graph);

            return $"{result}";
        }
    }
}
