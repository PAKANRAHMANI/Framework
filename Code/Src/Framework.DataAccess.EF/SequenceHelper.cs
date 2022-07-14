using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Framework.DataAccess.EF
{
    public class SequenceHelper
    {
        private readonly FrameworkDbContext _dbContext;

        public SequenceHelper(FrameworkDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<long> Next(string sequenceName)
        {
            var sequenceSqlParameter = new SqlParameter("@sequence", SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output
            };

            await _dbContext.Database.ExecuteSqlRawAsync($"SELECT NEXT VALUE FOR {sequenceName}", sequenceSqlParameter);

            return (long)sequenceSqlParameter.Value;
        }
    }
}
