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

        public class Graph
        {
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

        public Graph GetInputSequence(bool simpleInput = false)
        {
            return InitReader(simpleInput, (reader) =>
            {
                Dictionary<string,int> nodes = new Dictionary<string,int>();
                List<Graph.Edge> edges = new List<Graph.Edge>();

                var lineStream = ParseLines(reader);
                foreach (var line in lineStream)
                {
                    var edgetokens = line?.Split("-");
                    if( !nodes.ContainsKey(edgetokens[0]) )
                    {
                        nodes.Add(edgetokens[0], nodes.Count);
                    }
                    if (!nodes.ContainsKey(edgetokens[1]))
                    {
                        nodes.Add(edgetokens[1], nodes.Count);
                    }
                    edges.Add(new Graph.Edge
                    {
                        Node0 = nodes[edgetokens[0]],
                        Node1 = nodes[edgetokens[1]],
                    });
                }

                return new Graph
                {
                    Nodes = nodes.OrderBy(x => x.Value).Select(x => x.Key).ToArray(),
                    Edges = edges.ToArray(),
                    StartNode = nodes["start"],
                    EndNode = nodes["end"],
                };
            });
        }

        bool UseSimpleInput = false;
        public override string GetResult1()
        {
            var graph = GetInputSequence(UseSimpleInput);

            var result = "";
            
            return $"{result}";
        }


        public override string GetResult2()
        {
            var sequence = GetInputSequence(UseSimpleInput);

            var result = "";

            return $"{result}";
        }
    }
}
