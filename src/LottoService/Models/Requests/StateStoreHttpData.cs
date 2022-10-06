namespace LottoService.Models.Requests;

public class StateStoreHttpData<T> where T : class, new()
{
    public string? Key { get; set; }
    public T? Value { get; set; }
}