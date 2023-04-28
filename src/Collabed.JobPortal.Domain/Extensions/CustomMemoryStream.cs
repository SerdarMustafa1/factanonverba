using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Extensions
{
    public class CustomMemoryStream : MemoryStream
    {
        public override int ReadTimeout
        {
            get { return -1; } // No timeout
            set { throw new NotSupportedException("ReadTimeout is not supported on this stream."); }
        }

        public override int WriteTimeout
        {
            get { return -1; } // No timeout
            set { throw new NotSupportedException("WriteTimeout is not supported on this stream."); }
        }
    }

}
