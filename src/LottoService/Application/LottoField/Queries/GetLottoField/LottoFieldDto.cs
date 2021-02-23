using System.Collections.Generic;
using LottoService.Domain.ValueObjects;

namespace LottoService.Application.LottoField.Queries.GetLottoField
{
    public class LottoFieldDto
    {
        public IList<int> Numbers { get; set; }
        public int SuperNumber { get; set; }
    }
}
