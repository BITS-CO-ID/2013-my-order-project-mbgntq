using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecms.Biz.Entities;
using Ecms.Biz;

namespace Ecms.Biz
{
    public interface IUserBiz
    {
        #region // User

        Sys_User GetUserSigin(string userCode, string userPassword, string flagAdmin, ref string alParamsOutError);

        List<UserModel> GetUser(string userCode, string userName, string departmentId, string flagActive, string isFUApproved);

        List<UserModel> GetUserForMap(string userCode
            , string userName
            , string groupCode
            , string objectCode
            , string objectType
            , string flagActive
            );

		void DeleteUser(string _GWUserCode, string userCode, out string alParamsOutError);

        void AddUser(Sys_User user);

        void UpdateUser(Sys_User user);

        void UserChangedPassword(string usercode, string userPassword, string userPasswordNew, ref string alParamsOutError);

        Sys_User UserResetPassword(string usercodeOrEmail, string type, ref string alParamsOutError);

        #endregion

        #region // Group
        List<Sys_Group> GetGroup(string groupCode, string groupName);

        void DeleteGroup(string groupCode);

        void AddGroup(Sys_Group group);

        void UpdateGroup(Sys_Group group);
        #endregion

        #region // Object
        List<Sys_Object> GetObject(string objectCode, string objectName, string objectType, string flagActive);

        void Remove(string obj);

        void Add(Sys_Object obj);

        void UpdateObject(Sys_Object obj);
        #endregion

        #region // Sys_MapGroupObject
        List<MapGroupObjectModel> GetSysMapGroupObject(string groupCode, string objectCode, string objectType, string flagActive);

        void AddMapGroupObject(List<Sys_MapGroupObject> lstGrpObj);

        void UpdateMapGroupObject(string groupCode, List<Sys_MapGroupObject> lstGrpObj);

        #endregion

        #region // Sys_MapGroupUsers
        List<MapUserGroupModel> GetSysMapGroupUsers(string groupCode, string userCode, string flagActive);

        void AddMapGroupUsers(List<Sys_MapUserGroup> lstUserObj);

        void UpdateMapGroupUsers(string groupCode, List<Sys_MapUserGroup> lstGrpUsers);
        #endregion
       
    }
}
