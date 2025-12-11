using GanhoCapital.Models;
using GanhoCapital.Services;
using Xunit;

namespace GanhoCapital.Tests;

public class TaxCalculatorTests
{
    [Fact]
    public void Case1_SellOperationsBelowExemptionLimit_ShouldNotPayTax()
    {
        // Arrange
        var calculator = new TaxCalculator();
        var operations = new List<Operation>
        {
            new Operation { Type = "buy", UnitCost = 10.00m, Quantity = 100 },
            new Operation { Type = "sell", UnitCost = 15.00m, Quantity = 50 },
            new Operation { Type = "sell", UnitCost = 15.00m, Quantity = 50 }
        };
        var expectedTaxes = new[] { 0.00m, 0.00m, 0.00m };

        // Act
        var results = calculator.CalculateTaxes(operations);

        // Assert
        Assert.Equal(3, results.Count);
        for (int i = 0; i < expectedTaxes.Length; i++)
        {
            Assert.Equal(expectedTaxes[i], results[i].Tax);
        }
    }

    [Fact]
    public void Case2_ProfitAboveExemption_ThenLoss()
    {
        // Arrange
        var calculator = new TaxCalculator();
        var operations = new List<Operation>
        {
            new Operation { Type = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new Operation { Type = "sell", UnitCost = 20.00m, Quantity = 5000 },
            new Operation { Type = "sell", UnitCost = 5.00m, Quantity = 5000 }
        };
        var expectedTaxes = new[] { 0.00m, 10000.00m, 0.00m };

        // Act
        var results = calculator.CalculateTaxes(operations);

        // Assert
        Assert.Equal(3, results.Count);
        for (int i = 0; i < expectedTaxes.Length; i++)
        {
            Assert.Equal(expectedTaxes[i], results[i].Tax);
        }
    }

    [Fact]
    public void Case3_LossThenProfitWithDeduction()
    {
        // Arrange
        var calculator = new TaxCalculator();
        var operations = new List<Operation>
        {
            new Operation { Type = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new Operation { Type = "sell", UnitCost = 5.00m, Quantity = 5000 },
            new Operation { Type = "sell", UnitCost = 20.00m, Quantity = 3000 }
        };
        var expectedTaxes = new[] { 0.00m, 0.00m, 1000.00m };

        // Act
        var results = calculator.CalculateTaxes(operations);

        // Assert
        for (int i = 0; i < expectedTaxes.Length; i++)
        {
            Assert.Equal(expectedTaxes[i], results[i].Tax);
        }
    }

    [Fact]
    public void Case4_WeightedAverageThenSellBelowExemption()
    {
        // Arrange
        var calculator = new TaxCalculator();
        var operations = new List<Operation>
        {
            new Operation { Type = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new Operation { Type = "buy", UnitCost = 25.00m, Quantity = 5000 },
            new Operation { Type = "sell", UnitCost = 15.00m, Quantity = 10000 }
        };
        var expectedTaxes = new[] { 0.00m, 0.00m, 0.00m };

        // Act
        var results = calculator.CalculateTaxes(operations);

        // Assert
        for (int i = 0; i < expectedTaxes.Length; i++)
        {
            Assert.Equal(expectedTaxes[i], results[i].Tax);
        }
    }

    [Fact]
    public void Case5_WeightedAverageLossThenProfit()
    {
        // Arrange
        var calculator = new TaxCalculator();
        var operations = new List<Operation>
        {
            new Operation { Type = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new Operation { Type = "buy", UnitCost = 25.00m, Quantity = 5000 },
            new Operation { Type = "sell", UnitCost = 15.00m, Quantity = 10000 },
            new Operation { Type = "sell", UnitCost = 25.00m, Quantity = 5000 }
        };
        var expectedTaxes = new[] { 0.00m, 0.00m, 0.00m, 10000.00m };

        // Act
        var results = calculator.CalculateTaxes(operations);

        // Assert
        for (int i = 0; i < expectedTaxes.Length; i++)
        {
            Assert.Equal(expectedTaxes[i], results[i].Tax);
        }
    }

    [Fact]
    public void Case6_AccumulatedLossWithMultipleSales()
    {
        // Arrange
        var calculator = new TaxCalculator();
        var operations = new List<Operation>
        {
            new Operation { Type = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new Operation { Type = "sell", UnitCost = 2.00m, Quantity = 5000 },
            new Operation { Type = "sell", UnitCost = 20.00m, Quantity = 2000 },
            new Operation { Type = "sell", UnitCost = 20.00m, Quantity = 2000 },
            new Operation { Type = "sell", UnitCost = 25.00m, Quantity = 1000 }
        };
        var expectedTaxes = new[] { 0.00m, 0.00m, 0.00m, 0.00m, 3000.00m };

        // Act
        var results = calculator.CalculateTaxes(operations);

        // Assert
        for (int i = 0; i < expectedTaxes.Length; i++)
        {
            Assert.Equal(expectedTaxes[i], results[i].Tax);
        }
    }

    [Fact]
    public void Case7_ComplexOperationsWithZeroStock()
    {
        // Arrange
        var calculator = new TaxCalculator();
        var operations = new List<Operation>
        {
            new Operation { Type = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new Operation { Type = "sell", UnitCost = 2.00m, Quantity = 5000 },
            new Operation { Type = "sell", UnitCost = 20.00m, Quantity = 2000 },
            new Operation { Type = "sell", UnitCost = 20.00m, Quantity = 2000 },
            new Operation { Type = "sell", UnitCost = 25.00m, Quantity = 1000 },
            new Operation { Type = "buy", UnitCost = 20.00m, Quantity = 10000 },
            new Operation { Type = "sell", UnitCost = 15.00m, Quantity = 5000 },
            new Operation { Type = "sell", UnitCost = 30.00m, Quantity = 4350 },
            new Operation { Type = "sell", UnitCost = 30.00m, Quantity = 650 }
        };
        var expectedTaxes = new[] { 0.00m, 0.00m, 0.00m, 0.00m, 3000.00m, 0.00m, 0.00m, 3700.00m, 0.00m };

        // Act
        var results = calculator.CalculateTaxes(operations);

        // Assert
        for (int i = 0; i < expectedTaxes.Length; i++)
        {
            Assert.Equal(expectedTaxes[i], results[i].Tax);
        }
    }

    [Fact]
    public void Case8_IndependentSimulations()
    {
        // Arrange
        var calculator = new TaxCalculator();
        var operations = new List<Operation>
        {
            new Operation { Type = "buy", UnitCost = 10.00m, Quantity = 10000 },
            new Operation { Type = "sell", UnitCost = 50.00m, Quantity = 10000 },
            new Operation { Type = "buy", UnitCost = 20.00m, Quantity = 10000 },
            new Operation { Type = "sell", UnitCost = 50.00m, Quantity = 10000 }
        };
        var expectedTaxes = new[] { 0.00m, 80000.00m, 0.00m, 60000.00m };

        // Act
        var results = calculator.CalculateTaxes(operations);

        // Assert
        for (int i = 0; i < expectedTaxes.Length; i++)
        {
            Assert.Equal(expectedTaxes[i], results[i].Tax);
        }
    }
}
