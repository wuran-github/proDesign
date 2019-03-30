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
    
    public partial class tb_SupplierInfo
    {
        public tb_SupplierInfo()
        {
            this.tb_DepotIn = new HashSet<tb_DepotIn>();
            this.tb_DepotOut = new HashSet<tb_DepotOut>();
            this.tb_PurchaseDrugs = new HashSet<tb_PurchaseDrugs>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string LegalPersons { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string CompanyPhone { get; set; }
        public string Email { get; set; }
        public string OrganizationCode { get; set; }
        public string PostalCode { get; set; }
        public string Fax { get; set; }
    
        public virtual ICollection<tb_DepotIn> tb_DepotIn { get; set; }
        public virtual ICollection<tb_DepotOut> tb_DepotOut { get; set; }
        public virtual ICollection<tb_PurchaseDrugs> tb_PurchaseDrugs { get; set; }
    }
}