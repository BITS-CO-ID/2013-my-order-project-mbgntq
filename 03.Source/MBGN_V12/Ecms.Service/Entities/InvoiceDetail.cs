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
    
    public partial class InvoiceDetail
    {
        public int InvoiceDetailId { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<int> OverDueConfigId { get; set; }
        public Nullable<int> Type { get; set; }
    
        public virtual Invoice Invoice { get; set; }
        public virtual InvoiceDetail InvoiceDetail1 { get; set; }
        public virtual InvoiceDetail InvoiceDetail2 { get; set; }
    }
}
