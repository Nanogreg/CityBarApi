using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectDB
{
    public class Contact
    {
        public int ID { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime? DateNaiss { get; set; }

        public override string ToString()
        {
            return $"{ID} - {Nom} {Prenom}";
        }
    }
}
