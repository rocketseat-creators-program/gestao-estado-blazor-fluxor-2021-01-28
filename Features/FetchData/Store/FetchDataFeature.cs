using Fluxor;

using System.Net.Http.Json;

namespace GestaoEstadoFluxor.Features.FetchData.Store;

public class WeatherForecast
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record FetchDataState(bool Initialized, bool Loading, WeatherForecast[] Forecasts);

public class FetchDataFeature : Feature<FetchDataState>
{
    public override string GetName() => "Fetch Data";

    protected override FetchDataState GetInitialState() => new FetchDataState(false, false, Array.Empty<WeatherForecast>());
}

public record class SetInitialized { }
public record class SetLoading(bool Loading) { }
public record class SetData(WeatherForecast[] Data) { }
public record class LoadData { }

public static class FetchDataReducers
{
    [ReducerMethod(typeof(SetInitialized))]
    public static FetchDataState Initialize(FetchDataState state) => state with { Initialized = true };

    [ReducerMethod]
    public static FetchDataState ToggleLoading(FetchDataState state, SetLoading action) => state with { Loading = action.Loading };

    [ReducerMethod]
    public static FetchDataState SetForecasts(FetchDataState state, SetData action) => state with { Forecasts = action.Data };
}

public class FetchDataEffects
{
    private readonly HttpClient _client;    

    public FetchDataEffects(HttpClient client)
    {
        _client = client;        
    }

    [EffectMethod(typeof(LoadData))]
    public async Task LoadForecasts(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SetLoading(true));

        var forecasts = await _client.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");

        dispatcher.Dispatch(new SetData(forecasts));

        dispatcher.Dispatch(new SetLoading(false));
    }
}