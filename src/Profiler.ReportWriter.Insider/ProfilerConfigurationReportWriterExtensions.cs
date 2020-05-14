using Insider;

namespace Profiler
{
    public static class ProfilerConfigurationReportWriterExtensions
    {
        public static IProfilerConfiguration UseInsiderReportWriter(this IProfilerConfiguration settings, IInsider insider)
        {
            settings.CreateReportWriter = () => new InsiderReportWriter(insider);
            return settings;
        }
    }
}
