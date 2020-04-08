using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Models
{
    public class AdvAndEth
    {
        public EthinicalGroup GetEthinicalGroup { get; set; }
        public int Value { get; set; }

    }
}
