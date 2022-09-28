namespace LottoService.Models.Responses;

public class LottoFieldResponse
{
    public IList<int> Numbers { get; set; } = new List<int>(6);
    public int SuperNumber { get; set; }
}
