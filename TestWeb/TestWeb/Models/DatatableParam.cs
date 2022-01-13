using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWeb.Models
{
    public class DatatableParam
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public Dictionary<string, string> search { get; set; }
        //public List<string> search { get; set; } //
    }
}