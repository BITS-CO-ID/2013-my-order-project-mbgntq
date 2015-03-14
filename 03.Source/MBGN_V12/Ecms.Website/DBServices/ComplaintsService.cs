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
    public class ComplaintsService : BaseService
    {
         #region // Constructs
        public ComplaintsService()
            : base()
        {

        }
        #endregion

        #region // NewsGet
        public List<Complaint> ComplaintGet(
            string id
            , string createdUser
            , string title
            , string status
            , Page page)
        {
            try
            {
                string alParamsOutError = "";
                var comp = this._complaintsBiz.ComplaintGet(
                    id
                    , createdUser
                    , title
                    , status
                    , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);

                return comp;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<Complaint>();
            }
        }
        #endregion

        #region //ComplaintDelete
        public bool ComplaintDelete(string id, Page page)
        {
            try
            {
                string alParamsOutError = "";
                bool result;
                result = this._complaintsBiz.ComplaintDelete(id, ref alParamsOutError);
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

         #region //ComplaintCreate
        public Complaint ComplaintCreate(Complaint complaint, Page page)
        {
            try
            {
                string alParamsOutError = "";
                Complaint result;
                result = this._complaintsBiz.ComplaintCreate(complaint, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new Complaint();
            }
        }
        #endregion

        #region //ComplaintUpdate
        public Complaint ComplaintUpdate(Complaint complaint, Page page)
        {
            try
            {
                string alParamsOutError = "";
                Complaint result;
                result = this._complaintsBiz.ComplaintUpdate(complaint, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new Complaint();
            }
        }
        #endregion

        #region //ComplaintDetailGet
        public List<ComplaintDetailModel> ComplaintDetailGet(
            string id
            , string complaintId
            , string userCreatedId
            , Page page)
        {
            try
            {
                string alParamsOutError = "";
                var news_comment = this._complaintsBiz.ComplaintDetailGet(
                    id
                    , complaintId
                    , userCreatedId
                    , ref alParamsOutError);

                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return news_comment;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new List<ComplaintDetailModel>();
            }
        }
        #endregion

        #region // ComplaintDetailDelete
        public bool ComplaintDetailDelete(string id, Page page)
        {
            try
            {
                string alParamsOutError = "";
                bool result;
                result = this._complaintsBiz.ComplaintDetailDelete(id, ref alParamsOutError);
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

        #region //ComplaintDetailUpdate
        public ComplaintDetail ComplaintDetailUpdate(ComplaintDetail complaintDetail, Page page)
        {
            try
            {
                string alParamsOutError = "";
                ComplaintDetail result;
                result = this._complaintsBiz.ComplaintDetailUpdate(complaintDetail, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new ComplaintDetail();
            }
        }
        #endregion

        #region //ComplaintDetailCreate
        public ComplaintDetail ComplaintDetailCreate(ComplaintDetail complaintDetail, Page page)
        {
            try
            {
                string alParamsOutError = "";
                ComplaintDetail result;
                result = this._complaintsBiz.ComplaintDetailCreate(complaintDetail, ref alParamsOutError);
                if (!string.IsNullOrEmpty(alParamsOutError))
                    throw GenServiceException(alParamsOutError);
                return result;
            }
            catch (Exception ex)
            {
                Utils.ShowExceptionBox(ex, page);
                return new ComplaintDetail();
            }
        }
        #endregion




    }
}