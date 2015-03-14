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
    public class ComplaintsBiz : IComplaints
    {
        #region //Complaint

        #region //ComplaintGet

        public List<Complaint> ComplaintGet(
            string id
            , string createdUser
            , string title
            , string status
            , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ComplaintGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "id",id
                        , "createdUser",createdUser
						, "title",title
                        , "status",status
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.Complaints
                                 select p;

                    if (!string.IsNullOrEmpty(id))
                    {
                        int iFilter = Convert.ToInt32(id);
                        result = result.Where(p => p.Id == iFilter);
                    }

                    if (!string.IsNullOrEmpty(createdUser))
                    {
                        int iFilter = Convert.ToInt32(createdUser);
                        result = result.Where(p => p.CreatedUser == iFilter);
                    }

                    if (!string.IsNullOrEmpty(title))
                        result = result.Where(p => p.Title.Contains(title));

                    if (!string.IsNullOrEmpty(status))
                    {
                        int iFilter = Convert.ToInt32(status);
                        result = result.Where(p => p.Status == iFilter);
                    }

                    return result.OrderByDescending(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<Complaint>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region //ComplaintDelete
        public bool ComplaintDelete(string id, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ComplaintDelete";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "commentId",id
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    #region // Check:

                    int iFilter = Convert.ToInt32(id);
                    Complaint comp = new Complaint() { Id = iFilter };

                    #endregion

                    #region // Delete

                    _db.Complaints.Attach(comp);
                    _db.Complaints.Remove(comp);
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
        #endregion

        #region //ComplaintCreate
        public Complaint ComplaintCreate(Complaint complaint, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ComplaintCreate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.Complaints.Add(complaint);
                    _db.SaveChanges();
                }
                return complaint;
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new Complaint();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region //ComplaintUpdate
        public Complaint ComplaintUpdate(Complaint complaint, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ComplaintUpdate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    Complaint comp = _db.Complaints.Where(p => p.Id == complaint.Id).SingleOrDefault();

                    comp.Id = complaint.Id;
                    comp.ComplaintsNo = complaint.ComplaintsNo;
                    comp.CreatedUser = complaint.CreatedUser;
                    comp.CreatedDate = complaint.CreatedDate;
                    comp.CloseUser = complaint.CloseUser;
                    comp.ComplaintsDate = complaint.ComplaintsDate;
                    comp.LastModifyDate = complaint.LastModifyDate;
                    comp.Title = complaint.Title;
                    comp.ContentComplaints = complaint.ContentComplaints;
                    comp.ReceiverComplaintsId = complaint.ReceiverComplaintsId;
                    comp.Status = complaint.Status;
                    comp.Status = complaint.Status;
                    comp.CloseDate = complaint.CloseDate;
                    comp.Remark = complaint.Remark;

                    _db.SaveChanges();

                    return comp;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new Complaint();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #endregion
        #region //ComplaintDetail

        #region //ComplaintDetailGet
        public List<ComplaintDetailModel> ComplaintDetailGet(
            string id
            , string complaintId
            , string userCreatedId
            , ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ComplaintDetailGet";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "id",id
						, "complaintId",complaintId
						, "userCreatedId",userCreatedId
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    //from p in _db.Products
                    //             join c in _db.Categories on p.CategoryId equals c.CategoryId into c_join
                    //             from c in c_join.DefaultIfEmpty()
                    var result = from p in _db.ComplaintDetails
                                 join c in _db.Complaints on p.ComplaintId equals c.Id into c_join
                                 from c in c_join.DefaultIfEmpty()
                                 join u in _db.Customers on p.UserCreatedId equals u.CustomerId into u_join
                                 from u in u_join.DefaultIfEmpty()
                                 select new ComplaintDetailModel()
                                 {
                                     Id = p.Id,
                                     ComplaintId = p.ComplaintId,
                                     CreatedDate = p.CreatedDate,
                                     Content = p.Content,
                                     UserCreatedId = p.UserCreatedId,
                                     Title = c.Title,
                                     UserName = u.UserCode
                                 };


                    if (!string.IsNullOrEmpty(id))
                    {
                        int iFilter = Convert.ToInt32(id);
                        result = result.Where(p => p.Id == iFilter);
                    }

                    if (!string.IsNullOrEmpty(complaintId))
                    {
                        int iFilter = Convert.ToInt32(complaintId);
                        result = result.Where(p => p.ComplaintId == iFilter);
                    }

                    if (!string.IsNullOrEmpty(userCreatedId))
                    {
                        int iFilter = Convert.ToInt32(userCreatedId);
                        result = result.Where(p => p.UserCreatedId == iFilter);
                    }

                    return result.OrderBy(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<ComplaintDetailModel>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region //ComplaintDetailDelete
        public bool ComplaintDetailDelete(string id, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ComplaintDetailDelete";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "id",id
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    #region // Check:
                    int iFilter = Convert.ToInt32(id);

                    var deleteComplaintDetail = _db.ComplaintDetails.Where(p => p.Id == iFilter).SingleOrDefault();
                    #endregion

                    #region // Delete
                    _db.ComplaintDetails.Remove(deleteComplaintDetail);
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
        #endregion

        #region //ComplaintDetailUpdate
        public ComplaintDetail ComplaintDetailUpdate(ComplaintDetail complaintDetail, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ComplaintDetailUpdate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    ComplaintDetail complaintDe = _db.ComplaintDetails.Where(p => p.Id == complaintDetail.Id).SingleOrDefault();
                    complaintDe.Id = complaintDetail.Id;
                    complaintDe.ComplaintId = complaintDetail.ComplaintId;
                    complaintDe.CreatedDate = complaintDetail.CreatedDate;
                    complaintDe.Content = complaintDetail.Content;
                    complaintDe.UserCreatedId = complaintDetail.UserCreatedId;
                   
                    _db.SaveChanges();

                    return complaintDe;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new ComplaintDetail();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region //ComplaintDetailCreate
        public ComplaintDetail ComplaintDetailCreate(ComplaintDetail complaintDetail, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "ComplaintDetailCreate";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.ComplaintDetails.Add(complaintDetail);
                    _db.SaveChanges();
                }
                return complaintDetail;
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new ComplaintDetail();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #endregion
    }

    public class ComplaintDetailModel
    {
        public Int32? Id { set; get; }
        public Int32? ComplaintId { set; get; }
        public DateTime? CreatedDate { set; get; }
        public String Content { get; set; }
        public Int32? UserCreatedId { get; set; }
        public String Title { set; get; }
        public String UserName { set; get; }
        public Int32? CountPost { set; get; } 
    }
}
