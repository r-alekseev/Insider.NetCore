using Profiler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Insider.Demo.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var insider = new LocalInsiderConfiguration()
                .ConfigureDefault()
                .CreateInsider();

            var profiler = new ProfilerConfiguration()
                .UseInsiderReportWriter(insider)
                .CreateProfiler();

            insider.Run();

            Console.WriteLine("Commands:");
            Console.WriteLine("0 - state-set KEY VALUE");
            Console.WriteLine("1 - measure-start KEY");
            Console.WriteLine("2 - measure-pause KEY");
            Console.WriteLine("3 - measure-report");
            

            var sections = new Dictionary<string, ISection>();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Press '0', '1', '2' or '3':");

                var command = Console.ReadKey().KeyChar;

                if (!new[] { '0', '1', '2', '3' }.Contains(command))
                {
                    break;
                }

                if (command == '0')
                {
                    Console.WriteLine(" - state-set");

                    Console.Write("KEY: ");
                    string key = Console.ReadLine();

                    Console.Write("VALUE: ");
                    string value = Console.ReadLine();

                    insider.SetStates(new[] { (Key: new[] { key }, value) });
                }
                else if (command == '1')
                {
                    Console.WriteLine(" - measure-start");

                    Console.Write("KEY: ");
                    string key = Console.ReadLine();

                    if (!sections.ContainsKey(key))
                    {
                        sections[key] = profiler.Section(key);
                    }
                }
                else if (command == '2')
                {
                    Console.WriteLine(" - measure-pause");

                    Console.Write("KEY: ");
                    string key = Console.ReadLine();

                    if (sections.TryGetValue(key, out ISection section))
                    {
                        section.Dispose();

                        Console.WriteLine($"measure: {(section as ISectionMetrics).Elapsed}");

                        sections.Remove(key);
                    }
                }
                else if (command == '3')
                {
                    Console.WriteLine(" - measure-report");

                    profiler.WriteReport();
                }
            }

        }
    }
}
