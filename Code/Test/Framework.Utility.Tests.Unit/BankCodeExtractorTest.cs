using FluentAssertions;
using Xunit;

namespace Framework.Utility.Tests.Unit
{
    public class BankCodeExtractorTest
    {
        [Theory, MemberData(nameof(GetBankTestData))]
        public void bankCodeExtractor_should_extract_bank_code_correctly(string iban, string expectedBankCode)
        {
            string bankCode = BankCodeExtractor.GetBankCodeFromIban(iban);
            bankCode.Should().Be(expectedBankCode);

        }
        [Theory, InlineData("IR341560000000555559896562", "E_914")]
        public void bankCodeExtractor_should_return_defaultBankCode_when_iban_not_exist_in_dictionary(string iban, string expectedBankCode)
        {
            var bankCode = BankCodeExtractor.GetBankCodeFromIban(iban);
            bankCode.Should().Be(expectedBankCode);
        }

        public static IEnumerable<object[]> GetBankTestData()
        {
            var bankCodes = new List<string>()
            {
                "010", "011", "012", "013", "014", "015", "016",
                "017", "018", "019", "020", "021", "022", "051",
                "052", "053", "054", "055", "056", "057", "058",
                "059", "060", "061", "062", "063", "064", "065",
                "066", "069", "070", "078", "079"
            };
            return bankCodes.Select(bankCode => new object[]
            {
                $"IR34{bankCode}0000000555559896562", // شماره شبا شبیه‌سازی شده
                bankCode // کد بانک
            }).ToList();
        }
    }
}