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
    
    public partial class Sys_Group
    {
        public Sys_Group()
        {
            this.Sys_MapGroupObject = new HashSet<Sys_MapGroupObject>();
            this.Sys_MapUserGroup = new HashSet<Sys_MapUserGroup>();
        }
    
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string ParentGroupCode { get; set; }
        public string Remark { get; set; }
    
        public virtual ICollection<Sys_MapGroupObject> Sys_MapGroupObject { get; set; }
        public virtual ICollection<Sys_MapUserGroup> Sys_MapUserGroup { get; set; }
    }
}
