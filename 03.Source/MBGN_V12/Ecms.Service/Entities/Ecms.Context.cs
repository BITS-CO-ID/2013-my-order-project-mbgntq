﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EcmsEntities : DbContext
    {
        public EcmsEntities()
            : base("name=EcmsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ComplaintDetail> ComplaintDetails { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Sys_Group> Sys_Group { get; set; }
        public DbSet<Sys_MapGroupObject> Sys_MapGroupObject { get; set; }
        public DbSet<Sys_MapUserGroup> Sys_MapUserGroup { get; set; }
        public DbSet<Sys_Object> Sys_Object { get; set; }
        public DbSet<Sys_User> Sys_User { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductSalePrice> ProductSalePrices { get; set; }
        public DbSet<Inv_Goods> Inv_Goods { get; set; }
        public DbSet<Mst_Bank> Mst_Bank { get; set; }
        public DbSet<Mst_Business> Mst_Business { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<CustomerTemp> CustomerTemps { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<News_Comment> News_Comment { get; set; }
        public DbSet<WebsiteLink> WebsiteLinks { get; set; }
        public DbSet<OrderOutboundDetail> OrderOutboundDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Inv_StockInOut> Inv_StockInOut { get; set; }
        public DbSet<OrderOutbound> OrderOutbounds { get; set; }
        public DbSet<CustomerBalanceHistory> CustomerBalanceHistories { get; set; }
        public DbSet<WebsiteAccountVisa> WebsiteAccountVisas { get; set; }
        public DbSet<OrderOutboundHistory> OrderOutboundHistories { get; set; }
        public DbSet<Mst_VisaAccount> Mst_VisaAccount { get; set; }
        public DbSet<WebsiteAccount> WebsiteAccounts { get; set; }
        public DbSet<Mst_SysAuto> Mst_SysAuto { get; set; }
        public DbSet<Rpt_DeliverlyCPDetail> Rpt_DeliverlyCPDetail { get; set; }
        public DbSet<Rpt_DeliverlyConfirmPrint> Rpt_DeliverlyConfirmPrint { get; set; }
        public DbSet<Inv_StockBalance> Inv_StockBalance { get; set; }
        public DbSet<Inv_StockInOutDetail> Inv_StockInOutDetail { get; set; }
        public DbSet<ConfigBusiness> ConfigBusinesses { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
