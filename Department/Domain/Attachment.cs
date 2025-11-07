using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Attachment
    {
        public int Id { get; set; }
        public int LetterId { get; set; }
        public string FileName { get; set; } 
        public string ContentType { get; set; } 
        public string? RedisKey { get; set; }
        public string? FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; } 

        public Letter Letter { get; set; } = null!;
    }
}
