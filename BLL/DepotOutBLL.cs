﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DepotOutBLL : BaseBLL<models.tb_DepotOut>
    {
        #region Init
        DAL.DepotOutDAL dao = null;
        protected override void SetDAL()
        {
            dal = new DAL.DepotOutDAL();
            dao = dal as DAL.DepotOutDAL;
        }
        #endregion
    }
}
