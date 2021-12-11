using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal abstract class Common : IAoCEntity
    {
        public string Name { get; private set; }
        public Common(string DayName)
        {
            Name = DayName;
        }

        public Stream GetSimpleInput()
        {
            return File.OpenRead(CreateRelativePath($"{Name}simple"));
        }

        protected string CreateRelativePath(string name)
        {
            return $"assignments//{Name}//{name}.txt";
        }

        public Stream GetInput()
        {
            return File.OpenRead( CreateRelativePath(Name) );
        }

        protected T InitReader<T>(bool simpleInput, Func<StreamReader, T> Handler)
        {
            using var input = new StreamReader((!simpleInput ? GetInput() : GetSimpleInput()));
            return Handler(input);
        }
        

        protected IEnumerable<string> ParseLines(StreamReader reader)
        {
            for(string? line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                yield return line;
            }
        }

        public abstract string GetResult1();
        public abstract string GetResult2();
    }
}
