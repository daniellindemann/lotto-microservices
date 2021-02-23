using System.Threading.Tasks;

namespace LottoService.Application.Common.Interfaces
{
    public interface IRandomNumberService
    {
        Task<int> Generate(int min, int max);
    }
}
