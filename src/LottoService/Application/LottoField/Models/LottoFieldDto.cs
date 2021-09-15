using System.Collections.Generic;

namespace LottoService.Application.LottoField.Models
{
    public class LottoFieldDto
    {
        public IList<int> Numbers { get; set; }
        public int SuperNumber { get; set; }
    }
}
