using System;
using System.Collections.Generic;
using System.Text;

namespace Collabed.JobPortal.Settings
{
    public class BlobStorageOptions
    {
        public string ConnectionString { get; set; }
        public string DefaultContainerName { get; set; }
    }
}
