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
    
    public partial class Rpt_DeliverlyConfirmPrint
    {
        public Rpt_DeliverlyConfirmPrint()
        {
            this.Rpt_DeliverlyCPDetail = new HashSet<Rpt_DeliverlyCPDetail>();
        }
    
        public int RptDeliverlyId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public string DeliverlyFullName { get; set; }
        public string DeliverlyPosition { get; set; }
        public string DeliverlyMobile { get; set; }
        public string Remark { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Rpt_DeliverlyCPDetail> Rpt_DeliverlyCPDetail { get; set; }
    }
}