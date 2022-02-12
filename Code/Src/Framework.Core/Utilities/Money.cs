using Framework.Domain.CommonValueObjects.Factories;
using System;
using System.Collections.Generic;

namespace Framework.Domain.CommonValueObjects
{
    public class Money : ValueObject
    {
        protected Money()
        {
        }

        public Money(decimal amount, string currency)
        {
            this.Amount = amount;
            this.Currency = currency;
        }

        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        protected bool Equals(Money other)
        {
            return base.Equals(other) && Amount == other.Amount && Equals(Currency, other.Currency);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Money)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Amount, Currency);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return new { Amount, Currency };
        }

        public static bool operator ==(Money firstMoney, Money secondMoney)
        {
            var amount = secondMoney?.Amount;
            if (amount == null) return false;
            return firstMoney != null && (firstMoney.Amount == secondMoney.Amount &&
                                          Equals(firstMoney.Currency, secondMoney.Currency));
        }

        public static bool operator !=(Money firstMoney, Money secondMoney)
        {
            return !(firstMoney == secondMoney);
        }

        public static bool operator <=(Money firstMoney, Money secondMoney)
        {
            return firstMoney != null && firstMoney.Amount <= secondMoney.Amount;
        }

        public static bool operator >=(Money firstMoney, Money secondMoney)
        {
            return firstMoney != null && firstMoney.Amount >= secondMoney.Amount;
        }

        public static bool operator >(Money firstMoney, Money secondMoney)
        {
            return firstMoney.Amount > secondMoney.Amount;
        }

        public static bool operator <(Money firstMoney, Money secondMoney)
        {
            return firstMoney.Amount < secondMoney.Amount;
        }

        public static Money operator +(Money firstMoney, Money secondMoney)
        {
            return MoneyFactory.CreateIranCurrency(firstMoney.Amount + secondMoney.Amount);
        }

        public static Money operator -(Money firstMoney, Money secondMoney)
        {
            return MoneyFactory.CreateIranCurrency(firstMoney.Amount - secondMoney.Amount);
        }
    }
}
