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
    
    public partial class tb_Purchase
    {
        public tb_Purchase()
        {
            this.tb_PurchaseDrugs = new HashSet<tb_PurchaseDrugs>();
        }
    
        public int Id { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Finish { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
    
        public virtual ICollection<tb_PurchaseDrugs> tb_PurchaseDrugs { get; set; }
    }
}