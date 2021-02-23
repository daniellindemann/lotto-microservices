using System.Collections.Generic;
using LottoService.Domain.Common;
using LottoService.Domain.Exception;

namespace LottoService.Domain.ValueObjects
{
    public class SuperNumber : ValueObject
    {
        static SuperNumber()
        {
        }

        private SuperNumber()
        {
        }

        private SuperNumber(int number)
        {
            Number = number;
        }

        public int Number { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }

        public static implicit operator int(SuperNumber superNumber)
        {
            return superNumber.Number;
        }

        public static explicit operator SuperNumber(int superNumber)
        {
            return From(superNumber);
        }

        public static SuperNumber From(int number)
        {
            var superNumber = new SuperNumber(number);

            if (superNumber.Number < 1 ||
                superNumber.Number > 9)
            {
                throw new UnsupportedSuperNumberException();
            }

            return superNumber;
        }
    }
}
