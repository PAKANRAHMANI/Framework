using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.ChainHandlers
{
    public interface IHandler<T>
    {
        object Handle(T request);
        void SetNext(IHandler<T> handler);
    }
}
