using EGG_Haunolding_Magement_System.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EGG_Haunolding_Management_System.Class
{
    public  class JsonDataItem
    {
        [JsonPropertyName("zeit")]
        public double zeit { get; set; }
        [JsonPropertyName("zeittext")]
        public string zeittext { get; set; }
        [JsonPropertyName("180")]
        public int d1 { get; set; }
        [JsonPropertyName("280")]
        public int d2 { get; set; }
        [JsonPropertyName("170")]
        public int d3 { get; set; }
        [JsonPropertyName("270")]
        public int d4 { get; set; }
        [JsonPropertyName("saldo")]
        public int saldo { get; set; }
        [JsonPropertyName("saldoavg")]
        public int saldoavg { get; set;}
       
    }
}
