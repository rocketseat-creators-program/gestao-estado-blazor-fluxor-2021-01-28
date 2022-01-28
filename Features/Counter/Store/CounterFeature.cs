using Fluxor;

namespace GestaoEstadoFluxor.Features.Counter.Store;

public record CounterState(int CurrentCounte);

public class CounterFeature: Feature<CounterState>
{
    public override string GetName() => "Counter";

    protected override CounterState GetInitialState() => new CounterState(0);
}

public record IncrementCounter {}

public static class CounterReducers
{
    [ReducerMethod(typeof(IncrementCounter))]
    public static CounterState IncrementCurrentCounte(CounterState state) => state with { CurrentCounte = state.CurrentCounte + 1 };
}