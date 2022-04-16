using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.ChainHandlers
{
    public class EndOfChainHandler<T> : IHandler<T>
    {
        public async Task<object> Handle(T request)
        {
            return await Task.FromResult(new Exception("No other handler has processed the request"));
        }

        public void SetNext(IHandler<T> handler)
        {
            throw new NotSupportedException();
        }
    }
}
