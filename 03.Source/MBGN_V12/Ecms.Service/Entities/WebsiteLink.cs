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
    
    public partial class WebsiteLink
    {
        public WebsiteLink()
        {
            this.WebsiteLink1 = new HashSet<WebsiteLink>();
            this.WebsiteAccounts = new HashSet<WebsiteAccount>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int WebsiteId { get; set; }
        public string WebsiteName { get; set; }
        public string WebLink { get; set; }
        public string Description { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string WebsiteImage { get; set; }
    
        public virtual ICollection<WebsiteLink> WebsiteLink1 { get; set; }
        public virtual WebsiteLink WebsiteLink2 { get; set; }
        public virtual ICollection<WebsiteAccount> WebsiteAccounts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
