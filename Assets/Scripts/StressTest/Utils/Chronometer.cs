using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Time measurement
/// </summary>
public enum ChronoMetrics
{
    SECONDS = 1,
    MILLISECONDS =2
}

/// <summary>
/// A simple chrnometer class that broadcast datas when chronometer start, end or reset.
/// Also, offers events for getting elapsedTime since previous start of the chrono 
/// and an average of every elapsedTime.
/// </summary>
public class Chronometer
{
    const double CONVERSION_MILLISECONDS = 1000.0;

    public event Action<double> OnChronoStarted = null,
                                OnChronoReset = null,
                                OnChronoEnded = null;

    public event Action<string> OnChronoElapsed = null,
                                OnChronoAverageElapsed = null;

    ChronoMetrics metrics = ChronoMetrics.SECONDS;

    /// <summary>
    /// List of every elapsed time during each turn.
    /// </summary>
    List<double> turnResults = new List<double>();

    string Metrics
    {
        get
        {
            switch(metrics)
            {
                case ChronoMetrics.MILLISECONDS:
                default:
                    return "ms";
                case ChronoMetrics.SECONDS:
                    return "s";
            }
        }
    }

    string ElapsedTimeToString => ElapsedTimeBetween(startTime, endTime).ToString("N2") + Metrics;

    string AverageTimeToString => AverageTime.ToString("N2") + Metrics;


    double startTime = 0f, endTime = 0f, turnStartTime = 0f;

    double ElapsedTimeBetween(double _startTime, double _endTime) => ConvertTime(_endTime - _startTime);

    double AverageTime
    {
        get
        {
            double _average = 0f;

            for(int i = 0; i < turnResults.Count; ++i)
                _average += turnResults[i];

            _average /= turnResults.Count;

            return _average;

        }
    }

    /// <summary>
    /// Convert time with the metrics passed in parameters.
    /// </summary>
    /// <param name="_time">Time to set</param>
    /// <returns>Converted time</returns>
    double ConvertTime(double _time)
    {
        switch (metrics)
        {
            case ChronoMetrics.SECONDS:
            default:
                return _time;
            case ChronoMetrics.MILLISECONDS:
                return _time * CONVERSION_MILLISECONDS;
        }
    }

    public Chronometer(ChronoMetrics _metrics)
    {
        metrics = _metrics;
        OnChronoReset += AddTurnValue;
    }

    public void StartChrono()
    {
        turnResults.Clear();
        startTime = turnStartTime = Time.realtimeSinceStartupAsDouble;
        OnChronoStarted?.Invoke(startTime);
    }

    /// <summary>
    /// Get end turn time and copute elapsed time to add it in turnResults.
    /// Set turnStartTime equals to new end turn time to start another turn.
    /// </summary>
    public void NextTurnChrono()
    {
        double _endTurnTime = Time.realtimeSinceStartupAsDouble;
        OnChronoReset?.Invoke(ElapsedTimeBetween(turnStartTime, _endTurnTime));
        turnStartTime = _endTurnTime;
    }

    void AddTurnValue(double _value) => turnResults.Add(_value);

    /// <summary>
    /// End chronometer and broadcast separately elapsed time and average time.
    /// </summary>
    public void EndChrono()
    {
        endTime = Time.realtimeSinceStartupAsDouble;
        OnChronoElapsed?.Invoke(ElapsedTimeToString);
        OnChronoAverageElapsed?.Invoke(AverageTimeToString); 
        OnChronoEnded?.Invoke(endTime);
    }
}
