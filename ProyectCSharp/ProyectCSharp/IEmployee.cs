using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace ProyectCSharp
{
    public interface IEmployee
    {
        int id { get; set; }
        string firstName { get; set; }
        string lastName { get; set; }
    }
}
