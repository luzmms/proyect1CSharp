using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ProyectCSharp
{
    public class TeamProyect
    {
        private int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        private decimal _salaryPercent;

        public decimal salaryPercent
        {
            get { return _salaryPercent; }
            set { _salaryPercent = value; }
        }

        public List<Programmer> programmers;
    }
}
