using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz.Interfaces;
using Ecms.Biz.Entities;
using System.Collections;
using CommonUtils;

namespace Ecms.Biz.Class
{
    public class NewsBiz : INews
    {
        public List<NewsModel> NewsGet(string newsId
                                        , string title
                                        , string parentId
                                        , string published
                                        , string type
                                        , string websiteId
                                        , string dateFrom
                                        , string dateTo
                                        , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "NewsGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "newsId",newsId
						, "title",title
						, "parentId",parentId
                        , "published",published
                        , "websiteId",websiteId
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    //from p in _db.Products
                    //             join c in _db.Categories on p.CategoryId equals c.CategoryId into c_join
                    //             from c in c_join.DefaultIfEmpty()
                    var result = from p in _db.News
                                 join c in _db.WebsiteLinks on p.WebsiteId equals c.WebsiteId into c_join
                                 from c in c_join.DefaultIfEmpty()
                                 select new NewsModel()
                                 {
                                     NewsId = p.NewsId,
                                     Title = p.Title,
                                     ShortContent = p.ShortContent,
                                     NewsContent = p.NewsContent,
                                     ParentId = p.ParentId,
                                     AllowDelete = p.AllowDelete,
                                     Published = p.Published,
                                     NewsImage = p.NewsImage,
                                     Type = p.Type,
                                     WebsiteId = p.WebsiteId,
                                     CreatedDate = p.CreatedDate,
                                     CreateUser = p.CreateUser,
                                     WebsiteName = c.WebsiteName
                                 };


                    if (!string.IsNullOrEmpty(newsId))
                    {
                        int iFilter = Convert.ToInt32(newsId);
                        result = result.Where(p => p.NewsId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(title))
                        result = result.Where(p => p.Title.Contains(title));

                    if (!string.IsNullOrEmpty(type))
                    {
                        int iFilter = Convert.ToInt32(type);
                        result = result.Where(p => p.Type == iFilter);
                    }

                    if (!string.IsNullOrEmpty(dateFrom))
                    {
                        var iFilterFrom = Convert.ToDateTime(dateFrom);
                        result = result.Where(p => p.CreatedDate >= iFilterFrom);
                    }

                    if (!string.IsNullOrEmpty(dateTo))
                    {
                        var iFilterTo = Convert.ToDateTime(dateTo);
                        result = result.Where(p => p.CreatedDate <= iFilterTo);
                    }


                    if (!string.IsNullOrEmpty(parentId))
                    {
                        double iFilter = Convert.ToDouble(parentId);
                        result = result.Where(p => p.ParentId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(websiteId))
                    {
                        int iFilter = Convert.ToInt32(websiteId);
                        result = result.Where(p => p.WebsiteId == iFilter);
                    }


                    if (!string.IsNullOrEmpty(published))
                    {
                        bool iFilter = Convert.ToBoolean(published);
                        result = result.Where(p => p.Published == iFilter);
                    }

                    List<NewsModel> resultNewsModel = new List<NewsModel>();
                    resultNewsModel = result.OrderByDescending(x => x.NewsId).ToList();

                    // xử lí để hiển thị trên gridview
                    foreach (var item in resultNewsModel)
                    {

                        //published
                        if (item.Published==null || item.Published == false)
                        {
                            item.gPublished = "Không hiển thị";
                        }
                        else
                        {
                            item.gPublished = "Hiển thị";
                        }

                        // type
                        if (item.Type == null)
                        {
                            item.gType = "Chưa có dữ liệu";
                        }
                        else
                        {
                            switch (item.Type)
                            {
                                case 0:
                                        item.gType = "Các dịch vụ";
                            	    break;
                                case 1:
                                    item.gType = "Sale nóng";
                                    break;
                                case 2:
                                    item.gType = "Feed back";
                                    break;
                                case 3:
                                    item.gType = "Bài viết";
                                    break;
                            }
                        }

                        //// short content
                        //if (!String.IsNullOrEmpty(item.ShortContent) && item.ShortContent.Length>=50)
                        //{
                        //    item.ShortContent = item.ShortContent.Substring(0,49) + "...";
                        //}
                    }

                    return resultNewsModel;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<NewsModel>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        public bool NewsDelete(string newsId, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "NewsDelete";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "newsId",newsId
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    #region // Check:
                    int iFilter = Convert.ToInt32(newsId);
                    // Check in Department
                    var query = _db.News.Where(p => p.ParentId == iFilter);
                    if (query != null && query.Count() > 0)
                    {
                        throw new Exception("Tin tức đã sử dụng, bạn không thể xóa.");
                    }

					var deleteNews = _db.News.Where(p => p.NewsId == iFilter).SingleOrDefault();
					if (deleteNews != null && deleteNews.AllowDelete==false)
					{
						throw new Exception("Tin tức không cho phép xóa.");
					}
                    #endregion

                    #region // Delete
					_db.News.Remove(deleteNews);
                    _db.SaveChanges();
                    #endregion
                    return true;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return false;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        public News NewsCreate(News news, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "NewsCreate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "news", news
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.News.Add(news);
                    _db.SaveChanges();
                }
                return news;
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new News();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        public News NewsUpdate(News news, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "NewsUpdate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    News ne = _db.News.Where(p => p.NewsId == news.NewsId).SingleOrDefault();
                    ne.NewsId = news.NewsId;
                    ne.Title = news.Title;
                    ne.ShortContent = news.ShortContent;
                    ne.NewsContent = news.NewsContent;
                    ne.ParentId = news.ParentId;
                    ne.Published = news.Published;
                    ne.NewsImage = news.NewsImage;
                    ne.Type = news.Type;
                    ne.WebsiteId = news.WebsiteId;
                    ne.CreatedDate = news.CreatedDate;
                    ne.CreateUser = news.CreateUser;

                    _db.SaveChanges();

                    return ne;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new News();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        /************************************************************************/
        /* NEWS_COMMENT                                                         */
        /************************************************************************/

        #region News_Comment

        public List<News_Comment> News_CommentGet(string newsId, string commentId, string comment, string status, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "NewsGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "newsId",newsId
						, "commentId",commentId
						, "comment",comment
                        , "status",status
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.News_Comment
                                 select p;

                    if (!string.IsNullOrEmpty(newsId))
                    {
                        int iFilter = Convert.ToInt32(newsId);
                        result = result.Where(p => p.NewsId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(commentId))
                    {
                        int iFilter = Convert.ToInt32(commentId);
                        result = result.Where(p => p.CommentId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(comment))
                        result = result.Where(p => p.Comment.Contains(comment));

                    if (!string.IsNullOrEmpty(status))
                    {
                        int iFilter = Convert.ToInt32(status);
                        result = result.Where(p => p.Status == iFilter);
                    }

                    return result.OrderBy(x => x.NewsId).ToList();
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<News_Comment>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        /// <summary>
        /// Xóa comment
        /// </summary>
        /// <param name="commentId">Xóa từng comment riêng</param>
        /// <param name="newsId">Xóa tất cả comment của bài viết</param>
        /// <param name="alParamsOutError"></param>
        /// <returns></returns>
        public bool News_CommentDelete(string commentId, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "DeleteCustomerType";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "commentId",commentId
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    #region // Check:

                    int iFilter = Convert.ToInt32(commentId);
                    News_Comment comm = new News_Comment() { CommentId = iFilter };

                    #endregion

                    #region // Delete

                    _db.News_Comment.Attach(comm);
                    _db.News_Comment.Remove(comm);
                    _db.SaveChanges();
                    #endregion
                    return true;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return false;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        public News_Comment News_CommentCreate(News_Comment news_comment, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "News_CommentCreate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.News_Comment.Add(news_comment);
                    _db.SaveChanges();
                }
                return news_comment;
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new News_Comment();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        public News_Comment News_CommentUpdate(News_Comment news_comment, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "News_CommentUpdateS";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    News_Comment news_comm = _db.News_Comment.Where(p => p.CommentId == news_comment.CommentId).SingleOrDefault();

                    news_comm.CommentId = news_comment.CommentId;
                    news_comm.Comment = news_comment.Comment;
                    news_comm.NewsId = news_comment.NewsId;
                    news_comm.Status = news_comment.Status;
                    
                    _db.SaveChanges();

                    return news_comm;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new News_Comment();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

       
    }

    #region NewsModel
    public class NewsModel
    {
        public Int32? NewsId { set; get; }
        public String Title { set; get; }
        public String ShortContent { set; get; }
        public String NewsContent { get; set; }
        public Int32? ParentId { get; set; }
        public Boolean? Published { set; get; }
        public Boolean? AllowDelete { set; get; }
        public String gPublished { set; get; }
        public String NewsImage { set; get; }
        public Int32? Type { set; get; }
        public String gType { set; get; }
        public Int32? WebsiteId { set; get; }
        public String WebsiteName { set; get; }
        public DateTime? CreatedDate { set; get; }
        public String CreateUser { set; get; }
    }
    #endregion
}
