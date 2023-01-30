using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Collabed.JobPortal.BlobStorage
{
    public class GetBlobRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}
