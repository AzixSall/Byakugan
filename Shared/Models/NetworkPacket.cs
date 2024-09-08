using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class NetworkPacket
    {
        [Key]
        public string? DestinationIp { get; set; }

        public string? SourceIp { get; set; }

        public int DestinationPort { get; set; }

        public int SourcePort { get; set; }

        public DateTime? LastUpdate { get; set; }

        public string? Location { get; set; }
    }
}
