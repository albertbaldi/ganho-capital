using GanhoCapital.Models;

namespace GanhoCapital.Services;

public class TaxCalculator
{
    private const decimal TaxRate = 0.20m;
    private const decimal ExemptionThreshold = 20000m;

    public List<TaxResult> CalculateTaxes(List<Operation> operations)
    {
        var results = new List<TaxResult>();
        var state = new CalculatorState();

        foreach (var op in operations)
        {
            var (tax, newState) = ProcessOperation(op, state);
            results.Add(new TaxResult { Tax = tax });
            state = newState;
        }

        return results;
    }

    private (decimal tax, CalculatorState state) ProcessOperation(Operation op, CalculatorState state)
    {
        if (op.Type.ToLower() == "buy")
            return ProcessBuy(op, state);
        if (op.Type.ToLower() == "sell")
            return ProcessSell(op, state);
        return (0, state);
    }

    private (decimal, CalculatorState) ProcessBuy(Operation op, CalculatorState state)
    {
        var total = (state.WeightedAverage * state.Quantity) + (op.UnitCost * op.Quantity);
        var newQuantity = state.Quantity + op.Quantity;
        var newAverage = newQuantity > 0 ? total / newQuantity : 0;

        return (0, new CalculatorState(newAverage, newQuantity, state.Loss));
    }

    private (decimal, CalculatorState) ProcessSell(Operation op, CalculatorState state)
    {
        var saleValue = op.UnitCost * op.Quantity;
        var cost = state.WeightedAverage * op.Quantity;
        var profit = saleValue - cost;
        var newQuantity = state.Quantity - op.Quantity;

        if (profit < 0)
        {
            var newLoss = state.Loss + Math.Abs(profit);
            return (0, new CalculatorState(state.WeightedAverage, newQuantity, newLoss));
        }

        if (saleValue <= ExemptionThreshold)
            return (0, new CalculatorState(state.WeightedAverage, newQuantity, state.Loss));

        var taxable = profit;
        var remainingLoss = state.Loss;

        if (state.Loss > 0)
        {
            if (state.Loss >= taxable)
            {
                remainingLoss = state.Loss - taxable;
                return (0, new CalculatorState(state.WeightedAverage, newQuantity, remainingLoss));
            }
            taxable -= state.Loss;
            remainingLoss = 0;
        }

        var tax = Math.Round(taxable * TaxRate, 2);
        return (tax, new CalculatorState(state.WeightedAverage, newQuantity, remainingLoss));
    }

    private record CalculatorState(decimal WeightedAverage = 0, int Quantity = 0, decimal Loss = 0);
}
