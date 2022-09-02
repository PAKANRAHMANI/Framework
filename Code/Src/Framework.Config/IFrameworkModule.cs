using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Config
{
    public interface IFrameworkModule
    {
        void Register(IDependencyRegister dependencyRegister);
    }
}
