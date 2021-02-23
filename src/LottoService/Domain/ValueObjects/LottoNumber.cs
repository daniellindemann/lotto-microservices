using System.Collections.Generic;
using LottoService.Domain.Common;
using LottoService.Domain.Exception;

namespace LottoService.Domain.ValueObjects
{
    /// <summary>
    /// Lotto number for 6 of 49
    /// </summary>
    public class LottoNumber : ValueObject
    {
        static LottoNumber()
        {
        }

        private LottoNumber()
        {
        }

        private LottoNumber(int number)
        {
            Number = number;
        }

        public int Number { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }

        public static implicit operator int(LottoNumber superNumber)
        {
            return superNumber.Number;
        }

        public static explicit operator LottoNumber(int superNumber)
        {
            return From(superNumber);
        }

        public static LottoNumber From(int number)
        {
            var superNumber = new LottoNumber(number);

            if (superNumber.Number < 1 ||
                superNumber.Number > 49)
            {
                throw new UnsupportedLottoNumberException();
            }

            return superNumber;
        }
    }
}
