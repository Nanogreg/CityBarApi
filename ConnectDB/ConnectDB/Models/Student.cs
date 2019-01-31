using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectDB.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int YearResult { get; set; }
        public int SectionId { get; set; }
        public bool Active { get; set; }

        public override string ToString()
        {
            return $"{Id, -5}{FirstName, -20} {LastName,-20}{BirthDate.Date}\t{YearResult, -10}{SectionId, -10}{Active}";
        }

    }
}
