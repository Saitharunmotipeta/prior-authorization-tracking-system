using System.Diagnostics;

namespace PriorAuthorization.Shared.Utilities;

public static class StopwatchUtility
{
    public static Stopwatch Start()
    {
        return Stopwatch.StartNew();
    }

    public static long Stop(
        Stopwatch stopwatch)
    {
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }
}