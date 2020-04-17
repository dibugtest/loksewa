using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data
{
    public class Data
    {
        public enum Exam
        {
            [Description("Written")]
            Written, 

            [Description("interview")]
            interview,
            [Description("experimental")]

            experimental
        }
        
    }
}
