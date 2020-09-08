using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLab.Database.DBHelper;

namespace MedLab.Database.DBHelper
{
    public partial class User
    {
        public string Login { get { return $"{LastName} {Name} {MiddleName} ({Password})"; } }
        public string FullName { get { return $"{LastName} {Name} {MiddleName}"; } }
    }
}
