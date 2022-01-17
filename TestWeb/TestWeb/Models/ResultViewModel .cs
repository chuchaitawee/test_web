using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWeb.Models
{
    public class ResultViewModel
    {
        // model
        public List<tbl_test> _tbl_test { get; set; }
        public List<tbl_member> _tbl_member { get; set; }
    }
}