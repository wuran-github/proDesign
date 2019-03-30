using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PurchaseBLL:BaseBLL<models.tb_Purchase>
    {
        protected override void SetDAL()
        {
            dal = new DAL.PurchaseDAL();
        }
    }
}
