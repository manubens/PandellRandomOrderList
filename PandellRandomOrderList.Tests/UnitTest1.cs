using System;
using System.Linq;
using Xunit;

namespace PandellRandomOrderList.Tests;

public class ProgramTests
{
    [Fact]
    public void GenerateRandomPermutation_IsSize10000AndValid()
    {
        var result = Program.GenerateRandomPermutation();

        Assert.Equal(10000, result.Length);
        Assert.True(Program.IsValidPermutation(result, 10000));
        Assert.Equal(50005000, result.Sum());
        Assert.Equal(1, result.Min());
        Assert.Equal(10000, result.Max());
        Assert.Equal(result.Length, result.Distinct().Count());
    }

    [Fact]
    public void GenerateRandomPermutation_IsDifferentOnSubsequentCalls()
    {
        var first = Program.GenerateRandomPermutation(1000);
        var second = Program.GenerateRandomPermutation(1000);

        Assert.NotEqual(first, second); // extremely unlikely to be equal
        Assert.True(Program.IsValidPermutation(first, 1000));
        Assert.True(Program.IsValidPermutation(second, 1000));
    }

    [Fact]
    public void IsValidPermutation_RejectsInvalidInput()
    {
        var invalid = new[] { 1, 2, 2, 4 };

        Assert.False(Program.IsValidPermutation(invalid, 4));
        Assert.False(Program.IsValidPermutation(new[] { 2, 3, 4, 5 }, 4));
        Assert.False(Program.IsValidPermutation(new[] { 1, 2, 3 }, 4));
    }

    [Fact]
    public void GenerateRandomPermutation_WithSeedProducesRepeatableSequence()
    {
        var seed = 12345;
        var rng1 = new Random(seed);
        var rng2 = new Random(seed);

        var first = Program.GenerateRandomPermutation(1000, rng1);
        var second = Program.GenerateRandomPermutation(1000, rng2);

        Assert.Equal(first, second);
    }
}
