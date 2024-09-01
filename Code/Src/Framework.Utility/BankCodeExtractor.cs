using System;
using System.Collections.Generic;

namespace Framework.Utility;

public class BankCodeExtractor
{
    private static readonly Dictionary<string, string> BankCodes = new()
    {
        {"010", "بانک مرکزی جمهوری اسلامی ایران"},
        {"011", "بانک صنعت و معدن"},
        {"012", "بانک ملت"},
        {"013", "بانک رفاه"},
        {"014", "بانک مسکن"},
        {"015", "بانک سپه"},
        {"016", "بانک کشاورزی"},
        {"017", "بانک ملی ایران"},
        {"018", "بانک تجارت"},
        {"019", "بانک صادرات ایران"},
        {"020", "بانک توسعه صادرات"},
        {"021", "پست بانک ایران"},
        {"022", "بانک توسعه تعاون"},
        {"051", "موسسه اعتباری توسعه"},
        {"052", "بانک قوامین"},
        {"053", "بانک کارآفرین"},
        {"054", "بانک پارسیان"},
        {"055", "بانک اقتصاد نوین"},
        {"056", "بانک سامان"},
        {"057", "بانک پاسارگاد"},
        {"058", "بانک سرمایه"},
        {"059", "بانک سینا"},
        {"060", "قرض الحسنه مهر"},
        {"061", "بانک شهر"},
        {"062", "بانک آینده"},
        {"063", "بانک انصار"},
        {"064", "بانک گردشگری"},
        {"065", "بانک حکمت ایرانیان"},
        {"066", "بانک دی"},
        {"069", "بانک ایران زمین"},
        {"070", "بانک قرض‌الحسنه رسالت"},
        {"078", "خاورمیانه"},
        {"079", "بانک مهر اقتصاد"}
    };



    public static string GetBankCodeFromIban(string iban)
    {
        const string defaultCode = "E_914";

        if (IbanIsInValid(iban))
        {
            return defaultCode;
        }

        // استخراج کد بانک مستقیم (سه رقم بعد از چک دیجیت‌ها)
        var directBankCode = iban.Substring(4, 3);

        // اگر کد بانک مستقیم در دیکشنری باشد، آن را برمی‌گرداند
        return BankCodes.ContainsKey(directBankCode) ? directBankCode : defaultCode;
    }

    private static bool IbanIsInValid(string iban)
    {
        return iban.StartsWith("IR") == false
               || iban.Length != 26;
    }
}