using Profiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Insider.Demo.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var insider = new LocalInsiderConfiguration()
                .ConfigureDefault()
                .CreateInsider();

            var profiler = new ProfilerConfiguration()
                .UseInsiderReportWriter(insider)
                .CreateProfiler();

            insider.Run();

            Console.WriteLine("Commands:");
            Console.WriteLine("0 - exit");
            Console.WriteLine("1 - measure-start KEY");
            Console.WriteLine("2 - measure-pause KEY");
            Console.WriteLine("3 - measure-report");
            Console.WriteLine("4 - state-set KEY VALUE");


            var sections = new Dictionary<string, ISection>();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Press '0', '1', '2', '3' or '4':");

                var command = Console.ReadKey().KeyChar;

                if (!new[] { '0', '1', '2', '3', '4' }.Contains(command))
                {
                    break;
                }

                if (command == '0')
                {
                    await insider.StopAsync();
                    break;
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
                else if (command == '4')
                {
                    Console.WriteLine(" - state-set");

                    Console.Write("KEY: ");
                    string key = Console.ReadLine();

                    Console.Write("VALUE: ");
                    string value = Console.ReadLine();

                    insider.SetState(new[] { key }, value);
                }
            }

            Console.ReadKey();
        }
    }
}
