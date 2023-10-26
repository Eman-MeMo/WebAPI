using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Day2.Models
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public decimal salary { get; set; }
        public int DepartmentId { get; set; }
        //[JsonIgnore]
        public virtual Department Department { get; set; }
    }
}