using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A minimal demonstration app for generating and validating random permutations.
/// </summary>
public static class Program
{
    private const int MinValue = 1;
    private const int MaxValue = 10_000;

    /// <summary>
    /// Entry point. Creates a random permutation, verifies it, and prints summary output.
    /// </summary>
    /// <param name="args">Command line arguments (supports <c>--dump</c> to print all values).</param>
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

    /// <summary>
    /// Generates a random permutation of integers in the range [<see cref="MinValue"/>..n].
    /// </summary>
    /// <param name="n">Length of permutation and max value that must be included.</param>
    /// <param name="random">Random number generator; if null a new instance is created.</param>
    /// <returns>Random permutation array length <c>n</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <c>n</c> is below 
    /// <see cref="MinValue"/>.</exception>
    public static int[] GenerateRandomPermutation(int n = MaxValue, Random? random = null)
    {
        if (n < MinValue) throw new ArgumentOutOfRangeException(nameof(n), "N must be positive.");

        random ??= new Random();
        var values = Enumerable.Range(MinValue, n).ToArray();
        FisherYatesShuffle(values, random);
        return values;
    }

    /// <summary>
    /// Shuffles an array in-place using Fisher–Yates algorithm.
    /// </summary>
    /// <typeparam name="T">Array element type.</typeparam>
    /// <param name="array">Array to shuffle.</param>
    /// <param name="random">RNG used to choose swap indices.</param>
    private static void FisherYatesShuffle<T>(T[] array, Random random)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1); // 0 <= j <= i
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    /// <summary>
    /// Performs invariant checks on a finished permutation with expected size <see cref="MaxValue"/>.
    /// </summary>
    /// <param name="array">Permutation array returned from <see cref="GenerateRandomPermutation(int, Random?)"/>.</param>
    /// <exception cref="InvalidOperationException">If length, range, or uniqueness constraints are violated.</exception>
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

    /// <summary>
    /// Validates whether the array is a permutation of integers from <see cref="MinValue"/> to <paramref name="expectedSize"/>.
    /// </summary>
    /// <param name="array">Array to validate.</param>
    /// <param name="expectedSize">Expected permutation length and max value.</param>
    /// <returns><c>true</c> if <c>array</c> is a valid permutation; otherwise <c>false</c>.</returns>
    public static bool IsValidPermutation(int[] array, int expectedSize)
    {
        if (array.Length != expectedSize) return false;

        var min = array.Min();
        var max = array.Max();
        if (min != MinValue || max != expectedSize) return false;

        return array.Distinct().Count() == expectedSize;
    }
}

