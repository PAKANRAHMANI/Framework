using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Kafka
{
    public interface IOffsetManager
    {
        Position CurrentPosition();
        void MoveTo(Position position);
    }
}
