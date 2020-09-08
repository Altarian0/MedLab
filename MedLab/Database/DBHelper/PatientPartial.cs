using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Database.DBHelper
{
    partial class Patient
    {
        public string FullName 
        {
            get
            {
                return $"{LastName} {Name} {MiddleName}";
            }
        }
    }
}
