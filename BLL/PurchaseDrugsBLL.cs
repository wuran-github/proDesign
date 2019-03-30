using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PurchaseDrugsBLL :BaseBLL<models.tb_PurchaseDrugs>
    {
        protected override void SetDAL()
        {
            dal = new DAL.PurchaseDrugsDAL();
        }
    }
}
