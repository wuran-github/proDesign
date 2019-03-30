using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    static public class SingleContext
    {
        public static models.DBDepotEntities db = new models.DBDepotEntities();
    }
}
