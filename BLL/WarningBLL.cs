using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class WarningBLL : BaseBLL<models.tb_Warning>
    {
        #region Init
        DAL.WarningDAL dao = null;
        protected override void SetDAL()
        {
            dal = new DAL.WarningDAL();
            dao = dal as DAL.WarningDAL;
        }
        #endregion
    }
}
