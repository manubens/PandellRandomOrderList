using System;
using System.Collections.Generic;
using System.Linq;

// Generate a random permutation of numbers in range [1..n], here n=10_000
// Uses Fisher-Yates shuffle for O(n) runtime and minimal memory overhead.
public static class Program
{
    private const int MinValue = 1;
    private const int MaxValue = 10_000;

    public static void Main(string[] args)
    {
        var numbers = GenerateRandomPermutation(MaxValue, new Random());

        // validate and output
        Verify(numbers);

        Console.WriteLine("Random permutation generated. First 20 values:");
        Console.WriteLine(string.Join(", ", numbers.Take(20)));
        Console.WriteLine($"Checksum (sum) = {numbers.Sum()}, min={numbers.Min()}, max={numbers.Max()}");
        Console.WriteLine("Full list length: " + numbers.Length);

        if (args.Length > 0 && args[0].Equals("--dump", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine(string.Join(",", numbers));
        }
    }

    public static int[] GenerateRandomPermutation(int n = MaxValue, Random? random = null)
    {
        if (n < MinValue) throw new ArgumentOutOfRangeException(nameof(n), "N must be positive.");

        random ??= new Random();
        var values = Enumerable.Range(MinValue, n).ToArray();
        FisherYatesShuffle(values, random);
        return values;
    }

    private static void FisherYatesShuffle<T>(T[] array, Random random)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1); // 0 <= j <= i
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    public static void Verify(int[] array)
    {
        if (array.Length != MaxValue)
            throw new InvalidOperationException($"Expected {MaxValue} entries, got {array.Length}.");

        var min = array.Min();
        var max = array.Max();
        if (min != MinValue || max != MaxValue)
            throw new InvalidOperationException($"Range violation: [{min}..{max}] expected [{MinValue}..{MaxValue}].");

        var distinctCount = array.Distinct().Count();
        if (distinctCount != array.Length)
            throw new InvalidOperationException($"Duplicate detected: only {distinctCount} unique values.");
    }

    public static bool IsValidPermutation(int[] array, int expectedSize)
    {
        if (array.Length != expectedSize) return false;

        var min = array.Min();
        var max = array.Max();
        if (min != MinValue || max != expectedSize) return false;

        return array.Distinct().Count() == expectedSize;
    }
}

