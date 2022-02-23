using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Framework.DataAccess.Mongo
{
    public interface IMongoContext
    {
        IClientSessionHandle GetSession();
    }
}
