using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lokunclient.Models
{
    class APIError
    {
        public int status
        {
            get;
            set;
        }

        public string error
        {
            get;
            set;
        }
    }
}
