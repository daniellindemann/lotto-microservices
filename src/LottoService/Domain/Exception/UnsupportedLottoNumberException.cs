namespace LottoService.Domain.Exception
{
    public class UnsupportedLottoNumberException : System.Exception
    {
        public UnsupportedLottoNumberException() : base("Super number must be between 1 and 49")
        {
        }
    }
}
