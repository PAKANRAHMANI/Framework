using System.Threading.Tasks;
using NHibernate;

namespace Framework.DataAccess.NH
{
    public class SequenceHelper
    {
        private readonly ISession _session;
        public SequenceHelper(ISession session)
        {
            this._session = session;
        }
        public async Task<long> Next(string sequenceName)
        {
            return await _session.CreateSQLQuery("SELECT NEXT VALUE FOR " + sequenceName).UniqueResultAsync<long>();
        }
    }
}
