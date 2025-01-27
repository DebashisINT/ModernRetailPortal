using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModernRetail.Models
{
    public class FormTypeModel
    {
        public string Is_PageLoad { get; set; }

        public bool IsActive { get; set; }

        public Int64 grp_id { get; set; }
        public List<SECGRPLIST> SECGRPLIST { get; set; }

    }

    public class SECGRPLIST
    {
        public int grp_id { get; set; }
        public string grp_name { get; set; }
    }
}