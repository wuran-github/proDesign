using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DamagedDrugBLL : BaseBLL<models.tb_DamagedDrug>
    {
        #region Init
        DAL.DamagedDrugDAL dao = null;
        protected override void SetDAL()
        {
            dal=new DAL.DamagedDrugDAL();
            dao = dal as DAL.DamagedDrugDAL;
        }
        #endregion
    }
}
