using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Utilities
{
    public static class MoneyFactory
    {
        public static Money CreateIranCurrency(decimal amount)
        {
            return new Money(amount, CurrencyCode.Rial);
        }
    }
}
