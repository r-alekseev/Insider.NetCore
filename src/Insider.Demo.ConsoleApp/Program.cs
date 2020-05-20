using Profiler;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Console.WriteLine("e - [E]xit");
            Console.WriteLine("s - measure-[S]tart KEY");
            Console.WriteLine("p - measure-[P]ause KEY");
            Console.WriteLine("r - measure-[R]eport");
            Console.WriteLine("v - state-set KEY [V]ALUE");
            Console.WriteLine("b - open [B]rowser with `ui-web` page");

            var sections = new Dictionary<string, ISection>();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Press one key of: 'e', 's', 'p', 'r', 'v', 'b':");

                var command = Console.ReadKey().KeyChar;

                if (!new[] { 'e', 's', 'p', 'r', 'v', 'b' }.Contains(command))
                {
                    continue;
                }

                if (command == 'e')
                {
                    break;
                }
                else if (command == 's')
                {
                    Console.WriteLine(" - measure-start");

                    Console.Write("KEY: ");
                    string key = Console.ReadLine();

                    if (!sections.ContainsKey(key))
                    {
                        sections[key] = profiler.Section(key);
                    }
                }
                else if (command == 'p')
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
                else if (command == 'r')
                {
                    Console.WriteLine(" - measure-report");

                    profiler.WriteReport();
                }
                else if (command == 'v')
                {
                    Console.WriteLine(" - state-set");

                    Console.Write("KEY: ");
                    string key = Console.ReadLine();

                    Console.Write("VALUE: ");
                    string value = Console.ReadLine();

                    insider.SetState(new[] { key }, value);
                }
                else if (command == 'b')
                {
                    Browser.Open("http://localhost:8080/insider/ui-web");
                }
            }

            await insider.StopAsync();

            Console.ReadKey();
        }
    }
}
