// See https://aka.ms/new-console-template for more information
using AdventOfCode21.assignments;
using AdventOfCode21.common;

List<IAoCEntity> days = new List<IAoCEntity>
{
    new Day01(), // **
    new Day02(), // **
    new Day03(), // **
    new Day04(), // **
    new Day05(), // **
    new Day06(), // **
    new Day07(), // **     
    new Day08(), //       !!
    new Day09(), // **      
    new Day10(), // **
    new Day11(), // **
    new Day12(), // 

};

foreach (var day in days.TakeLast(1))
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(day.Name);
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(day.GetResult1());
    Console.WriteLine(day.GetResult2());
//    Console.WriteLine(" - - - - - - - - - - - - - - - - - -\n");
}