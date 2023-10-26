using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Day2.Models
{
    public class Department
    {
        public int id { get; set; }
        public string name { get; set; }
        //[JsonIgnore]
        public ICollection<Employee> Employees { get; set; }=new HashSet<Employee>();
}
}