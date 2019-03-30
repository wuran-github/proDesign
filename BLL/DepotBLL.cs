using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DepotBLL :BaseBLL<models.tb_DepotInfo>
    {
        #region Init
        DAL.DepotDAL dao = null;
        protected override void SetDAL()
        {
            dal = new DAL.DepotDAL();
            dao = dal as DAL.DepotDAL;
        }
        #endregion
    }
}
