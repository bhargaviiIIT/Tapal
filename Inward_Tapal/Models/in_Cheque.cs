using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inward_Tapal.Models
{
    public class in_Cheque
    {
        public string TPLNO { get; set; }
        public long CQNO { get; set; }
        public DateTime CQDATE { get; set; }
        public string BANK { get; set; }
        public string AMOUNT { get; set; }
    }
}