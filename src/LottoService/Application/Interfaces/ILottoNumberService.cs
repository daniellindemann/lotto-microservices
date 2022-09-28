using LottoService.Domain.Entities;

namespace LottoService.Application.Interfaces;

public interface ILottoNumberService
{
    Task<LottoField> Draw();
}
