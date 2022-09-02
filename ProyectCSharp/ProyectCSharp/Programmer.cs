using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ProyectCSharp
{
    public class Programmer : IEmployee
    {
        public int id { get;set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        private string _activity;

        public string activity
        {
            get { return _activity; }
            set { _activity = value; }
        }

        private DateTime _dateStart;

        public DateTime dateStart
        {
            get { return _dateStart; }
            set { _dateStart = value; }
        }

        private DateTime _dateEnd;

        public DateTime dateEnd
        {
            get { return _dateEnd; }
            set { _dateEnd = value; }
        }
    }
}
