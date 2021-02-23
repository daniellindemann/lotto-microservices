using System.Collections.Generic;
using System.Linq;
using LottoService.Domain.ValueObjects;

namespace LottoService.Domain.Entities
{
    public class LottoField
    {
        public LottoField()
        {
            Numbers = new List<LottoNumber>(6);
        }

        public IList<LottoNumber> Numbers { get; private set; }
        public SuperNumber SuperNumber { get; private set; }

        public void SetNumbers(IEnumerable<int> lottoNumbers)
        {
            Numbers = lottoNumbers.OrderBy(n => n).Select(n => LottoNumber.From((n))).ToList();
        }

        public void SetSuperNumber(int superNumber)
        {
            SuperNumber = SuperNumber.From(superNumber);
        }
    }
}
