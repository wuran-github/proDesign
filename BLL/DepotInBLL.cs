using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DepotInBLL : BaseBLL<models.tb_DepotIn>
    {
        #region Init
        DAL.DepotInDAL dao = null;
        protected override void SetDAL()
        {
            dal = new DAL.DepotInDAL();
            dao = dal as DAL.DepotInDAL;
        }
        #endregion
    }
}
