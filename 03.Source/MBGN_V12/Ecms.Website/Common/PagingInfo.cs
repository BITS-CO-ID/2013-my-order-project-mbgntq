using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecms.Website.Common
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int LinkPerPage { get; set; }
        public bool HasFirstPage { get { return CurrentPage > 1; } }
        public bool HasLastPage { get { return (CurrentPage < TotalPages); } }
        public bool HasNextPage { get { return (TotalPages > LinkPerPage); } }
        public bool HasPreviousPage { get { return (TotalPages > 1 && CurrentPage < TotalPages); } }
        public int DataItemIndex { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}