using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecms.Biz.Entities;
using System.Web.UI;
using Ecms.Website.Common;
using Ecms.Biz.Class;

namespace Ecms.Website.DBServices
{
    public class NewsService : BaseService
    {
        #region // Constructs

        public NewsService()
            : base()
        {

        }
        #endregion

        #region // NewsGet

        public List<NewsModel> NewsGet(string newsId
                                        , string title
                                        , string parentId
                                        , string published
                                        , string type
                                        , string websiteId
                                        , string dateFrom
                                        , string dateTo
                                        , Page page)
        {
            try
            {
                string alParamsOutError = "";
                var news = this._newsBiz.NewsGet( newsId
                                        , title
                                        , parentId
                                        , published
                                        , type
                                        , websiteId
                                        , dateFrom
                                        , dateTo
                                        , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);

                return news;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<NewsModel>();
            }
        }


        #endregion

        #region // NewsDelete
        public bool NewsDelete(string newsId, Page page)
        {
            try
            {
                string alParamsOutError = "";
                bool result;
                result = this._newsBiz.NewsDelete(newsId, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return false;
            }
        }
        #endregion

        #region // NewsCreate
        public News NewsCreate(News news, Page page)
        {
            try
            {
                string alParamsOutError = "";
                News result;
                result = this._newsBiz.NewsCreate(news, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new News();
            }
        }
        #endregion

        #region // NewsUpdate
        public News NewsUpdate(News news, Page page)
        {
            try
            {
                string alParamsOutError = "";
                News result;
                result = this._newsBiz.NewsUpdate(news, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new News();
            }
        }
        #endregion

        #region // News_CommentGet

        public List<News_Comment> News_CommentGet(string newsId, string commentId, string comment, string status, string websiteId, Page page)
        {
            try
            {
                string alParamsOutError = "";
                var news_comment = this._newsBiz.News_CommentGet(
                                    newsId
                                    , commentId
                                    , comment
                                    , status
                                    , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return news_comment;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<News_Comment>();
            }
        }

        #endregion

        #region // News_CommentDelete
        /// <summary>
        /// Xóa comment
        /// </summary>
        /// <param name="commentId"> để "" nếu xóa theo bài viết</param>
        /// <param name="newsId"> </param>
        /// <param name="page"></param>
        /// <returns></returns>
        public bool News_CommentDelete(string commentId, string newsId, Page page)
        {
            try
            {
                string alParamsOutError = "";
                bool result;
                result = this._newsBiz.News_CommentDelete(commentId,ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return false;
            }
        }
        #endregion

        #region // News_CommentCreate
        public News_Comment News_CommentCreate(News_Comment news_comment, Page page)
        {
            try
            {
                string alParamsOutError = "";
                News_Comment result;
                result = this._newsBiz.News_CommentCreate(news_comment , ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new News_Comment();
            }
        }
        #endregion

        #region // News_CommentUpdate
        public News_Comment News_CommentUpdate(News_Comment news_comment, Page page)
        {
            try
            {
                string alParamsOutError = "";
                News_Comment result;
                result = this._newsBiz.News_CommentUpdate(news_comment, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new News_Comment();
            }
        }
        #endregion
    }
}