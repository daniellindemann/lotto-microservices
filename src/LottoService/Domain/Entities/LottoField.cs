namespace LottoService.Domain.Entities;

public class LottoField
{
    public IList<int> Numbers { get; set; } = new List<int>(6);
    public int SuperNumber { get; set; }

    public void SetNumbers(IEnumerable<int> lottoNumbers)
    {
        Numbers = lottoNumbers.OrderBy(n => n).ToList();
    }

    public void SetSuperNumber(int superNumber)
    {
        SuperNumber = superNumber;
    }
}
