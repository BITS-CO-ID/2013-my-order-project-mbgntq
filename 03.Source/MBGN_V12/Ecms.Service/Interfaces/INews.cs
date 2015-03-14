using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz.Entities;
using Ecms.Biz.Class;

namespace Ecms.Biz.Interfaces
{
    public interface INews
    {
        List<NewsModel> NewsGet(string newsId, string title, string parentId, string published, string type, string websiteId, string dateFrom, string dateTo, ref string alParamsOutError);

        bool NewsDelete(string newsId, ref string alParamsOutError);

        News NewsCreate(News news, ref string alParamsOutError);

        News NewsUpdate(News news, ref string alParamsOutError);

        List<News_Comment> News_CommentGet(string newsId, string commentId, string comment, string status, ref string alParamsOutError);

        bool News_CommentDelete(string commentId, ref string alParamsOutError);

        News_Comment News_CommentCreate(News_Comment news_comment, ref string alParamsOutError);

        News_Comment News_CommentUpdate(News_Comment news_comment, ref string alParamsOutError);
    }
}
