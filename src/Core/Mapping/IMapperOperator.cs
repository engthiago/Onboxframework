using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V7.Mapping
{
    public interface IMapperOperator
    {
        object Map(object source, object target);
        object Map(object source);
    }
}
