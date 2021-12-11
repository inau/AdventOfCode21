using AdventOfCode21.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode21.assignments
{
    internal class Day06 : Common
    {
        public Day06() : base("Day06") {}

        public IEnumerable<sbyte> GetInitialPopulation(bool simpleInput = false)
        {
            return InitReader<IEnumerable<sbyte>>(simpleInput, (reader) =>
            {
                var lineStream = ParseLines(reader);
                foreach (var line in lineStream)
                {
                    return line.Split(",").Select(x=>sbyte.Parse(x)).ToList();
                }
                return null;
            });
        }

        void PrintDebugLine(int day, in List<int> state)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(day + "\t\t");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var v in state)
            {
                Console.Write($"{v},");
            }
            Console.WriteLine();
        }

        // other idea - cycle
        //                                              [ 0 ][ 1 ][ 2 ]
        // round count translates to entry in cycle            ^
        // at round 0 the initial population is accumulated into their array entry (count 3 means +1 on that bin)
        // increment the round counter
        // each time a field is hit, the cell number represent the fish to add to an 'future bin' ((round + 8)%6 bin)
        //-- The cycle might be too short this to work though (6 and 8)


        // rethink the solution - dont allocate, use math
        // This solution gets the result close to instantly - avoids allocating many objects.
        // Groups items that spawn at the same time and sums up their 'spawnCount'
        // this results in a small list of few entries at the expense of having a multiplier per entry for when it spawns
        class FishMatrix
        {
            class meta { public sbyte count = 0; public ulong multiplier = 1; }

            public ulong RunSimple(List<sbyte> initialPopulation, int cycles)
            {
                ulong totalCount = 0;
                List<meta> fish = new List<meta>();

                foreach(var citizen in initialPopulation)
                {
                    fish.Add(new meta { count = citizen, multiplier = 1});
                }

                totalCount += (uint)fish.Count;

                for(int i = 0; i < cycles; i++)
                {
                    var groupedFish = fish.GroupBy(x => x.count).ToList();
                    var pairedFish = groupedFish
                        .Select(grp => new meta 
                        { 
                            count = grp.Key, 
                            multiplier = grp.Select(x => x.multiplier).Aggregate((a,v)=> a+v) 
                        }).ToList();

                    fish = pairedFish;

                    //fish = fish.GroupBy(x => x.count)
                    //    .Select(g => new meta
                    //    {
                    //        count = g.Key,
                    //        multiplier = g.Select(x => x.multiplier).Aggregate((a,v)=> a+v)
                    //    }).ToList();

                    ulong fishToSpawn = 0;
                    for(int j = 0; j < fish.Count; ++j)
                    {
                        if (fish[j].count == 0)
                        {
                            fish[j].count = 7;
                            fishToSpawn += fish[j].multiplier;
                        }
                        fish[j].count--;
                    }

                    if( fishToSpawn > 0 )
                    {
                        fish.Add(new meta { count = 8, multiplier = fishToSpawn });
                        totalCount += fishToSpawn;
                    }
                }

                return totalCount;
            }
        }

        // This runs for for more than 10m and allocates more than 30gb of memory - still no result for 256 cycles
        class FishTank
        {
            class FishSegment
            {
                public long IndexOffset { get; private set; }
                sbyte[] Fish = new sbyte[1000000];
                public long Current { get; private set; }

                bool Isfull => Current >= Fish.LongLength;
                public long Capacity => Fish.Length;
                public bool AddFish(sbyte value)
                {
                    if(!Isfull)
                    {
                        Fish[Current++] = value;
                        return true;
                    }
                    return false;
                }

                public long RunCycle()
                {
                    long fishTospawn = 0;

                    for (int f = 0; f < Current; f++)
                    {
                        if (Fish[f] == 0)
                        {
                            Fish[f] = 7;
                            fishTospawn++;
                        }
                        Fish[f]--;
                    }

                    return fishTospawn;
                }

                public FishSegment(long offset)
                {
                    IndexOffset = offset;
                    Current = 0;
                }
            }

            List<FishSegment> segment = new List<FishSegment>();

            public void Init(List<sbyte> fish)
            {
                long i = 0;
                foreach (sbyte f in fish)
                {
                    segment.Add(new FishSegment(i++));
                    if( !segment.Last().AddFish(f) )
                    {
                        Console.WriteLine("OH NO");
                    }
                }                
            }

            private void AddFreshFish(long count)
            {
                for(long f = 0; f < count; f++)
                {
                    if (!segment.Last().AddFish(8))
                    {
                        var pr = segment.Last();
                        segment.Add(new FishSegment(pr.IndexOffset + pr.Capacity));
                        if (!segment.Last().AddFish(8))
                        {
                            Console.WriteLine("OH NO");
                        }
                    }
                }
            }

            public long RunSim(int rounds)
            {
                long totalCount = segment.Select(s => s.Current).Sum();

                for(int r = 0; r < rounds; ++r)
                {
                    long FishToAdd = segment.AsParallel().Select(x => x.RunCycle()).Sum();
                    totalCount += FishToAdd;
                    AddFreshFish(FishToAdd);

                    if (r % 50 == 0) Console.WriteLine($"Tank {r}: {totalCount} > ");
                }

                return totalCount;
            }
        }

        List<sbyte> RunFishSim(int rounds, List<sbyte> fish)
        {
            for (int day = 0; day < rounds; day++)
            {
                List<sbyte> toAdd = new List<sbyte>();
                for (int f = 0; f < fish.Count; f++)
                {
                    if (fish[f] == 0)
                    {
                        fish[f] = 7;
                        toAdd.Add(8);
                    }
                    fish[f]--;
                }
                fish = fish.Concat(toAdd).ToList();

                //    PrintDebugLine(day, fish);
            //    if (day % 50 == 0) Console.Write($"Day {day} > ");
            }

            return fish;
        }

        public override string GetResult1()
        {
            List<sbyte> fish = GetInitialPopulation(false).ToList();

            fish = RunFishSim(80, fish);

            return $"{fish.Count}";
        }


        public override string GetResult2()
        {
            List<sbyte> fish = GetInitialPopulation(false).ToList();

            FishMatrix fishTank = new FishMatrix();
            var population = fishTank.RunSimple(fish, 256);
            
            return $"{population}";
        }
    }
}
