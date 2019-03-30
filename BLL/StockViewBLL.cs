using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class StockViewBLL:ViewBLL<models.StockView>
    {
        protected override void SetDAL()
        {
            dal = new DAL.StockViewDAL();
        }
    }
}
