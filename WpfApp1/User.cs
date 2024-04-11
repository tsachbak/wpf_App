using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class User
    {
        public ObjectId Id { get; set; }

        public string Mail { get; set; }

        public string PWD { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }
        public int Age { get; set; }
    }
}
