using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DrugBLL : BaseBLL<models.tb_DrugInfo>
    {
        #region Init
        DAL.DrugDAL dao = null;
        protected override void SetDAL()
        {
            dal = new DAL.DrugDAL();
            dao = dal as DAL.DrugDAL;
        }
        #endregion
    }
}
