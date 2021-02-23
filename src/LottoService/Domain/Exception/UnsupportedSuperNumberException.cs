namespace LottoService.Domain.Exception
{
    public class UnsupportedSuperNumberException : System.Exception
    {
        public UnsupportedSuperNumberException() : base("Super number must be between 1 and 9")
        {
        }
    }
}
