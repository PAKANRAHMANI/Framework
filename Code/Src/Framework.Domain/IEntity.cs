using System;
using System.Text;

namespace Framework.Domain
{
    public interface IEntity
    {
        bool IsDeleted { get; }

        void MarkAsUpdated();

        void MarkAsDeleted();
        void MarkAsRowVersion();
    }
}
