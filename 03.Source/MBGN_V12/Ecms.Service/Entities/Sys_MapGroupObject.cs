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
    
    public partial class Sys_MapGroupObject
    {
        public string GroupCode { get; set; }
        public string ObjectCode { get; set; }
        public string ObjectType { get; set; }
        public string Remark { get; set; }
    
        public virtual Sys_Group Sys_Group { get; set; }
        public virtual Sys_Object Sys_Object { get; set; }
    }
}
