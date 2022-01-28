using Framework.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Domain.CommonValueObjects.Factories
{
    public static class MoneyFactory
    {
        public static Money CreateIranCurrency(decimal amount)
        {
            return new Money(amount, CurrencyCode.Rial);
        }
    }
}
