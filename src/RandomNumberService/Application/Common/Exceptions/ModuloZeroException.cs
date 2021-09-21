using System;

namespace RandomNumberService.Application.Common.Exceptions
{
    public class ModuloZeroException : Exception
    {
        public ModuloZeroException(int orginalNumber) : base(
            $"Modulo on number {orginalNumber} retuned zero.")
        {
        }
    }
}
