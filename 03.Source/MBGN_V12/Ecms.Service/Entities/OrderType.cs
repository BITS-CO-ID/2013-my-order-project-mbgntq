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
    
    public partial class OrderType
    {
        public OrderType()
        {
            this.Orders = new HashSet<Order>();
            this.ConfigBusinesses = new HashSet<ConfigBusiness>();
        }
    
        public int OrderTypeId { get; set; }
        public string OrderTypeName { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ConfigBusiness> ConfigBusinesses { get; set; }
    }
}
