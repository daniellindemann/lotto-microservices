using Web.Models.Responses;

namespace Web.Application.Interfaces;

public interface ILottoNumberService
{
    Task<LottoFieldResponse> GetLottoNumber();
}