using System.Text.Json.Serialization;

namespace LottoService.Models.Requests;

public class StateStoreHttpData<T> where T : class, new()
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("value")]
    public T? Value { get; set; }
}