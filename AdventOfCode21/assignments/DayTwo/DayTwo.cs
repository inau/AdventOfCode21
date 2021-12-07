using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class DayTwo : Common
    {
        public DayTwo() : base("DayTwo") {}

        struct Command
        {
            public Tuple<int,int> Values { get; private set; }
            
            public Command(string direction, string distance)
            {
                int value = int.Parse(distance);
                switch (direction.ToLower())
                {
                    case "forward":
                        Values = new Tuple<int, int>(value, 0);
                        break;
                    case "down": 
                        Values = new Tuple<int, int>(0, 1*value);
                        break;
                    case "up":
                        Values = new Tuple<int, int>(0, -1*value);
                        break;
                   default: 
                        Values = new Tuple<int, int>(0, 0);
                        break;
                }
            }
        }

        IEnumerable<Command> GetCommands()
        {
            List<Command> commands = new List<Command>();
            using (var input = new StreamReader(GetInput()))
            {
                for (string? s = input.ReadLine(); s != null; s = input.ReadLine())
                {
                    var items = s.Split(" ");
                    commands.Add(new Command(items[0], items[1]));
                }
            }
            return commands;
        }

        public override string GetResult1()
        {
            int x = 0;
            int y = 0;
            foreach (var cmd in GetCommands())
            {
                x += cmd.Values.Item1;
                y += cmd.Values.Item2;
            }

            return $"{x * y}";
        }

        public override string GetResult2()
        {
            int x = 0;
            int y = 0;
            int z = 0;

            foreach (var cmd in GetCommands())
            {
                z += cmd.Values.Item2;

                x += cmd.Values.Item1;
                y += z * cmd.Values.Item1;
            }

            return $"{x*y}";
        }
    }
}
