using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Training.Core
{
    public class MyGuidGenerator : IGuidGenerator
    {
        private Guid _guid;

        public MyGuidGenerator()
        {
            _guid = Guid.NewGuid();
        }

        public Guid GetGuid()
        {
            return _guid;
        }

        
    }
}
