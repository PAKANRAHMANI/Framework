using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Framework.Core.Extensions
{
    public static class DbCommandExtensions
    {
        public static void ApplyYeke(this IDbCommand command)
        {
            command.CommandText = command.CommandText.ApplyPersianYeKe();

            foreach (DbParameter parameter in command.Parameters)
            {
                switch (parameter.DbType)
                {
                    case DbType.AnsiString:
                    case DbType.AnsiStringFixedLength:
                    case DbType.String:
                    case DbType.StringFixedLength:
                    case DbType.Xml:
                        parameter.Value = Apply(parameter.Value);
                        break;
                }
            }
        }
        private static object Apply(object value)
        {
            return value?.ToString().ApplyPersianYeKe();
        }
    }
}
