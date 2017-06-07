using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inward_Tapal.Models
{
    public class sample_User
    {
        public long Id { get; set; }
        public DateTime? Date_in { get; set; }
        public string T_Sno { get; set; }
        [Required(ErrorMessage = "From is required")]
        public string T_From { get; set; }
     
        public string T_Dept { get; set; }
        [Required(ErrorMessage = "Subject is required")]
        public string T_Subject { get; set; }
        [Required(ErrorMessage = "Selection is a MUST")]
        public string T_Subject_Value { get; set; }
        public string T_Section { get; set; }
        public string T_Passout { get; set; }
        public string TPLNO { get; set; }
        public string T_Cheque { get; set; }
        public string T_User { get; set; }
        [Required(ErrorMessage = "WFCPNO is required")]
        public string wfcpno { get; set; }
        public bool Issue { get; set; }
        public List<Subject> sub { get; set; }

        public List<SelectListItem> subList { get; set; }
        public Int64 SubjectId { get; set; }
        public IEnumerator<Inward> enum11 { get; set; }
    }
}