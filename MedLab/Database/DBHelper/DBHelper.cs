using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLab.Database.DBHelper
{
    public class DBHelper
    {
        private static MedLabEntities context = new MedLabEntities();

        public static MedLabEntities GetContext()
        {
            return context;
        }
    }
}
