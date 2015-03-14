using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;

namespace Ecms.Website.Common
{
    public static class Extension
    {
        public static string PageLinks(this Page html, PagingInfo pagingInfo,string pageIndex, string url)
        {
            StringBuilder result = new StringBuilder();
            #region //Temp

            if (pagingInfo.CurrentPage > 1)
            {
                result.Append(string.Format("<a href='{0}'><i class='icon-chevron-left'></i></a>", url + "?pageIndex=" + (Convert.ToInt32(pageIndex) - 1)));
            }
            int startPage = 1;
            int endPage = 11;

            if (pagingInfo.TotalPages < pagingInfo.LinkPerPage)
            {
                endPage = pagingInfo.TotalPages;
            }
            else
            {
                if (pagingInfo.CurrentPage > 5 && pagingInfo.CurrentPage <= pagingInfo.TotalPages)
                {
                    startPage = pagingInfo.CurrentPage - 5;
                    endPage = pagingInfo.CurrentPage + 5;
                    if (endPage > pagingInfo.TotalPages)
                    {
                        endPage = pagingInfo.TotalPages;
                    }
                }
            }
            for (int i = startPage; i <= endPage; i++)
            {
                result.Append(string.Format("<a class='"+(i == pagingInfo.CurrentPage ? "selected" : "")+"' href='{0}'>{1}</a>",url + "?pageIndex=" + i,i));
            }

            if (pagingInfo.TotalPages > 1 && pagingInfo.CurrentPage < pagingInfo.TotalPages)
            {
                result.Append(string.Format("<a href='{0}'><i class='icon-chevron-right'></i></a>", url + "?pageIndex=" + (Convert.ToInt32(pageIndex) + 1)));
            }

            #endregion
            return result.ToString();
        }
    }
}