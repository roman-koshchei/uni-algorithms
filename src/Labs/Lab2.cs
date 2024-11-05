using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labs;

public static class Lab2
{
    public static void Run()
    {
        // Problem 1: Minimize Maximum Delay
        Console.WriteLine("Problem 1: Minimize Maximum Delay");
        List<Job> jobs =
        [
            new Job(2, 5),
            new Job(3, 3),
            new Job(1, 2),
            new Job(4, 7)
        ];

        // Print job diagram
        PrintJobDiagram(jobs);
        var (maxDelay, maxDelayJobs) = MinimizeMaxDelay(jobs);
        Console.WriteLine("Maximum Delay: " + maxDelay);
        PrintJobDiagram(maxDelayJobs);

        // Problem 2: Minimize Total Delay
        Console.WriteLine("\nProblem 2: Minimize Total Delay");
        PrintJobDiagram(jobs);
        var (totalDelay, totalDelayJobs) = MinimizeTotalDelay(jobs);
        Console.WriteLine("Total Delay: " + totalDelay);
        PrintJobDiagram(totalDelayJobs);

        // Problem 3: Maximum Size Subset of Non-Conflicting Jobs
        Console.WriteLine("\nProblem 3: Maximum Size Subset of Non-Conflicting Jobs");
        List<IntervalJob> intervalJobs =
        [
            new IntervalJob(0, 6),
            new IntervalJob(1, 4),
            new IntervalJob(3, 5),
            new IntervalJob(5, 7),
            new IntervalJob(3, 9),
            new IntervalJob(6, 8)
        ];

        // Print interval job diagram
        PrintIntervalJobDiagram(intervalJobs);
        var (selectedJobs, orderJobs) = MaxNonConflictingJobs(intervalJobs);
        PrintIntervalJobDiagram(orderJobs);

        Console.WriteLine("Maximum number of non-conflicting jobs: " + selectedJobs.Count);
        foreach (var job in selectedJobs)
        {
            Console.WriteLine($"Job starts at {job.Start} and ends at {job.End}");
        }
    }

    private static (int, List<Job>) MinimizeMaxDelay(List<Job> jobs)
    {
        jobs = jobs.OrderBy(j => j.Deadline).ToList();

        int currentTime = 0;
        int maxDelay = 0;

        foreach (var job in jobs)
        {
            currentTime += job.Length;
            int delay = Math.Max(0, currentTime - job.Deadline);
            maxDelay = Math.Max(maxDelay, delay);
        }

        return (maxDelay, jobs);
    }

    private static (int, List<Job>) MinimizeTotalDelay(List<Job> jobs)
    {
        jobs = jobs.OrderBy(j => j.Length).ToList();

        int currentTime = 0;
        int totalDelay = 0;

        foreach (var job in jobs)
        {
            currentTime += job.Length;
            int delay = Math.Max(0, currentTime - job.Deadline);
            totalDelay += delay;
        }

        return (totalDelay, jobs);
    }

    private static (List<IntervalJob> res, List<IntervalJob> order) MaxNonConflictingJobs(List<IntervalJob> jobs)
    {
        jobs = jobs.OrderBy(j => j.End).ToList();

        List<IntervalJob> result = [];
        int lastEndTime = -1;

        foreach (var job in jobs)
        {
            if (job.Start >= lastEndTime)
            {
                result.Add(job);
                lastEndTime = job.End;
            }
        }

        return (result, jobs);
    }

    private static void PrintJobDiagram(List<Job> jobs)
    {
        int currentTime = 0;
        Console.WriteLine("Jobs Diagram:");
        foreach (var job in jobs)
        {
            Console.Write($"Job (Length: {job.Length}, Deadline: {job.Deadline}): ");
            Console.Write(new string(' ', job.Deadline - job.Length));
            Console.WriteLine(new string('-', job.Length));
            currentTime += job.Length;
        }
        Console.WriteLine();
    }

    private static void PrintIntervalJobDiagram(List<IntervalJob> jobs)
    {
        Console.WriteLine("Interval Jobs Diagram:");
        foreach (var job in jobs)
        {
            Console.Write($"Job (Start: {job.Start}, End: {job.End}): ");
            Console.Write(new string(' ', job.Start));
            Console.WriteLine(new string('-', job.End - job.Start));
        }
        Console.WriteLine();
    }
}

internal class Job(int length, int deadline)
{
    public int Length { get; set; } = length;
    public int Deadline { get; set; } = deadline;
}

internal class IntervalJob(int start, int end)
{
    public int Start { get; set; } = start;
    public int End { get; set; } = end;
}