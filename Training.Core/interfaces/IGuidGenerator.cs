using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core
{
    public interface IGuidGenerator
    {
        Guid GetGuid();
    }
}
