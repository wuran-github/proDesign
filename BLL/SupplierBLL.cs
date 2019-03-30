using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SupplierBLL : BaseBLL<models.tb_SupplierInfo>
    {
        #region Init
        DAL.SupplierDAL dao = null;
        protected override void SetDAL()
        {
            dal = new DAL.SupplierDAL();
            dao = dal as DAL.SupplierDAL;
        }
        #endregion
    }
}
