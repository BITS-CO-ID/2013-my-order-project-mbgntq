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
    
    public partial class Inv_StockInOut
    {
        public Inv_StockInOut()
        {
            this.Inv_Goods = new HashSet<Inv_Goods>();
            this.Inv_Goods1 = new HashSet<Inv_Goods>();
            this.Inv_StockInOutDetail = new HashSet<Inv_StockInOutDetail>();
        }
    
        public int StockInOutId { get; set; }
        public string StockOutNo { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> InOutDate { get; set; }
        public string Remark { get; set; }
        public Nullable<byte> Type { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> status { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> LastDateModify { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Inv_Goods> Inv_Goods { get; set; }
        public virtual ICollection<Inv_Goods> Inv_Goods1 { get; set; }
        public virtual ICollection<Inv_StockInOutDetail> Inv_StockInOutDetail { get; set; }
    }
}
