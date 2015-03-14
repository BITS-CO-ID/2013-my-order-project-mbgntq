using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecms.Website.Common
{
    public class PagingModels<T>
    {
        //DataSource Models
        public List<T> Models { get; set; }
        public PagingInfo PagingInfo { get; set; }

        /// <summary>
        /// Khởi tạo phân trang với một DataSource và trang hiện tại
        /// </summary>
        /// <param name="list">DataSource</param>
        /// <param name="pageIndex">Trang hiện tại</param>
        public PagingModels(IQueryable<T> list, int pageIndex,int items)
        {
            PagingInfo = new PagingInfo();
            PagingInfo.CurrentPage = pageIndex;
            PagingInfo.ItemsPerPage = items;
            PagingInfo.LinkPerPage = 11;
            PagingInfo.TotalItems = list.Count();
            PagingInfo.DataItemIndex = pageIndex * PagingInfo.ItemsPerPage - PagingInfo.ItemsPerPage;
            this.Models = list.ToList()
                 .Skip((pageIndex - 1) * items)
                 .Take(items).ToList();
        }
    }
}
