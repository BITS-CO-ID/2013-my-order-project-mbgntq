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
    
    public partial class CustomerBalanceHistory
    {
        public int CustomerBalanceHistoryId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<double> Balance { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public string CreatedUserCode { get; set; }
    
        public virtual Invoice Invoice { get; set; }
    }
}