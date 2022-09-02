using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Framework.DataAccess.NH
{
    public class BinaryTimestamp : IUserVersionType
    {
        bool IUserType.Equals(object x, object y)
        {
            return (x == y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            return rs.GetValue(rs.GetOrdinal(names[0]));
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            NHibernateUtil.Binary.NullSafeSet(cmd, value, index, session);
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }

        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }

        public SqlType[] SqlTypes => new[] { new SqlType(DbType.Binary, 8) };
        public Type ReturnedType => typeof(byte[]);
        public bool IsMutable => false;
        public int Compare(object x, object y)
        {
            return CompareValues((byte[])x, (byte[])y);
        }
        private static int CompareValues(byte[] x, byte[] y)
        {
            if (x.Length < y.Length)
            {
                return -1;
            }
            if (x.Length > y.Length)
            {
                return 1;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] < y[i])
                {
                    return -1;
                }
                if (x[i] > y[i])
                {
                    return 1;
                }
            }
            return 0;
        }
        public object Seed(ISessionImplementor session)
        {
            return new byte[8];
        }

        public object Next(object current, ISessionImplementor session)
        {
            return current;
        }
    }
}
