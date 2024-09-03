using System;
using UnityEngine;

public class TimeInterval
{
    private const double TIMESTAMP_CLOSE_THRESHOLD = 5d;

    public double FromTimestamp { get; }
    public double ToTimestamp { get; private set; }

    public TimeInterval()
    {
        FromTimestamp = Time.timeAsDouble;
        ToTimestamp = FromTimestamp;
    }

    public bool UpdateInterval(TimeInterval otherInterval)
    {
        bool close = AreTimestampsClose(ToTimestamp, otherInterval.ToTimestamp);
        if (close)
            ToTimestamp = otherInterval.ToTimestamp;

        return close;
    }

    public bool CloseTo(TimeInterval otherInterval)
    {
        bool fromFrom = AreTimestampsClose(FromTimestamp, otherInterval.FromTimestamp);
        bool fromTo = AreTimestampsClose(FromTimestamp, otherInterval.ToTimestamp);
        bool toFrom = AreTimestampsClose(ToTimestamp, otherInterval.FromTimestamp);
        bool toTo = AreTimestampsClose(ToTimestamp, otherInterval.ToTimestamp);
        return fromFrom || fromTo || toFrom || toTo;
    }

    private bool AreTimestampsClose(double t1, double t2)
    {
        return Math.Abs(t2 - t1) < TIMESTAMP_CLOSE_THRESHOLD;
    }

    public override string ToString()
    {
        return ((FromTimestamp == ToTimestamp) ? "at " : ("from " + FromTimestamp.ToString("N1") + " s to ")) +
            ToTimestamp.ToString("N1") + " s";
    }
}
