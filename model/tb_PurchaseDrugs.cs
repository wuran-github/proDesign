//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_PurchaseDrugs
    {
        public int PId { get; set; }
        public int DrugId { get; set; }
        public int Num { get; set; }
        public int SupId { get; set; }
        public int DepotId { get; set; }
    
        public virtual tb_DrugInfo tb_DrugInfo { get; set; }
        public virtual tb_Purchase tb_Purchase { get; set; }
        public virtual tb_DepotInfo tb_DepotInfo { get; set; }
        public virtual tb_SupplierInfo tb_SupplierInfo { get; set; }
    }
}