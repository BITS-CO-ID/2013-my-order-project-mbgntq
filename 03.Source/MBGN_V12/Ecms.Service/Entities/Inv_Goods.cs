//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecms.Biz.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inv_Goods
    {
        public int GoodsId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string Serial { get; set; }
        public Nullable<int> StockInId { get; set; }
        public Nullable<int> StockOutId { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> DateIn { get; set; }
        public Nullable<System.DateTime> DateOut { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Inv_StockInOut Inv_StockInOut { get; set; }
        public virtual Inv_StockInOut Inv_StockInOut1 { get; set; }
    }
}
