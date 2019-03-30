using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DepotDrugBLL : BaseBLL<models.tb_DepotDrug>
    {
        #region Init
        DAL.DepotDrugDAL dao = null;
        protected override void SetDAL()
        {
            dal = new DAL.DepotDrugDAL();
            dao = dal as DAL.DepotDrugDAL;
        }
        #endregion
    }
}
