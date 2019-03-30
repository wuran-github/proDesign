using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DrugCheckBLL:ViewBLL<models.drugCheck>
    {
        protected override void SetDAL()
        {
            dal = new DAL.DrugCheckDAL();
        }
    }
}
