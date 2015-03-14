using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Ecms.Biz;
using Ecms.Biz.Entities;
using CommonUtils;
using System.Collections;
using System.Transactions;



namespace Ecms.Biz
{
    public class UserBiz : IUserBiz
    {
        #region // Sys_User

        #region // GetUserSigin
        public Sys_User GetUserSigin(string userCode, string userPassword, string flagAdmin, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "GetUserSigin";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "userCode",userCode
						, "userPassword",userPassword
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    if (flagAdmin.Equals("0"))
                    {
                        var user = (from u in _db.Sys_User
                                    join c in _db.Customers on u.UserCode equals c.UserCode
                                    where (c.UserCode == userCode || c.Email == userCode || c.CustomerCode == userCode) && u.UserPassword == userPassword
                                    select u).FirstOrDefault();
                        return user;
                    }

                    if (flagAdmin.Equals("1"))
                    {
                        var user = (from u in _db.Sys_User
                                    where (u.UserCode == userCode || u.Email == userCode) && u.UserPassword == userPassword
                                    select u).FirstOrDefault();

                        return user;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return null;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // GetUser
        public List<UserModel> GetUser(string userCode
            , string userName
            , string departmentId
            , string flagActive
            , string isFUApproved
            )
        {
            #region // Temp
            string strFunctionName = "GetUser";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "userCode",userCode
						, "userName",userName
						, "departmentId",departmentId
						, "flagActive",flagActive
						, "isFUApproved", isFUApproved
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.Sys_User
                                 select new UserModel
                                 {
                                     FlagActive = p.FlagActive,
                                     FlagAdmin = p.FlagAdmin,
                                     Position = p.Position,
                                     Remark = p.Remark,
                                     UserCode = p.UserCode,
                                     UserName = p.UserName,
                                     Email = p.Email,
                                     UserPassword = p.UserPassword,
									 SupperAdmin=p.SupperAdmin,
                                 };

                    if (!string.IsNullOrEmpty(userCode))
                        result = result.Where(p => p.UserCode == userCode);

                    if (!string.IsNullOrEmpty(userName))
                        result = result.Where(p => p.UserName == userName);

                    if (!string.IsNullOrEmpty(flagActive))
                        result = result.Where(p => p.FlagActive == flagActive);

                    if (!string.IsNullOrEmpty(isFUApproved))
                        result = result.Where(p => p.IsFUApproved == isFUApproved);

                    if (!string.IsNullOrEmpty(departmentId))
                    {
                        int iFilter = Convert.ToInt32(departmentId);
                        result = result.Where(p => p.DepartmentId == iFilter);
                    }

                    return result.OrderBy(p => p.UserName).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<UserModel>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // GetUserForMap
        public List<UserModel> GetUserForMap(string userCode
            , string userName
            , string groupCode
            , string objectCode
            , string objectType
            , string flagActive
            )
        {
            #region // Temp
            string strFunctionName = "GetUserForMap";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "userCode",userCode
						, "userName",userName
						, "groupCode",groupCode
						, "objectCode",objectCode
						, "objectType",objectType
						, "flagActive",flagActive
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.Sys_User
                                 join ug in _db.Sys_MapUserGroup on p.UserCode equals ug.UserCode into ug_join
                                 from ug in ug_join.DefaultIfEmpty()
                                 join og in _db.Sys_MapGroupObject on ug.GroupCode equals og.GroupCode into og_join
                                 from og in og_join.DefaultIfEmpty()
                                 join o in _db.Sys_Object on og.ObjectCode equals o.ObjectCode into o_join
                                 from o in o_join.DefaultIfEmpty()
                                 join g in _db.Sys_Group on ug.GroupCode equals g.GroupCode into g_join
                                 from g in g_join.DefaultIfEmpty()
                                 select new UserModel
                                 {
                                     FlagActive = p.FlagActive,
                                     FlagAdmin = p.FlagAdmin,
                                     Position = p.Position,
                                     Remark = og.Remark,
                                     UserCode = p.UserCode,
                                     UserName = p.UserName,
                                     Email = p.Email,
                                     UserPassword = p.UserPassword,
                                     GroupCode = ug.GroupCode,
                                     GroupName = g.GroupName,
                                     ObjectCode = og.ObjectCode,
                                     ObjectName = o.ObjectName,
                                     ParentObjectCode = o.ParentObjectCode,
                                     ObjectType = o.ObjectType,
                                     ObjectTemplate = o.ObjectTemplate,
									 OPriority=o.OPriority,
									 SupperAdmin=p.SupperAdmin
                                 };

                    if (!string.IsNullOrEmpty(userCode))
                        result = result.Where(p => p.UserCode == userCode);

                    if (!string.IsNullOrEmpty(userName))
                        result = result.Where(p => p.UserName == userName);

                    if (!string.IsNullOrEmpty(flagActive))
                        result = result.Where(p => p.FlagActive == flagActive);

                    if (!string.IsNullOrEmpty(groupCode))
                        result = result.Where(p => p.GroupCode == groupCode);

                    if (!string.IsNullOrEmpty(objectCode))
                        result = result.Where(p => p.ObjectCode == objectCode);

                    if (!string.IsNullOrEmpty(objectType))
                        result = result.Where(p => p.ObjectType == objectType);

                    return result.OrderBy(p=>p.OPriority).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<UserModel>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // DeleteUser
        public void DeleteUser(
                string _GWUserCode
                , string userCode
                , out string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "DeleteUser";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "_GWUserCode",_GWUserCode
                        , "userCode",userCode
			            });
            #endregion

            #region // Init:
            alParamsOutError = "";
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    #region // Check:
                    //var query = from p in _db.Transactions
                    //            where (p.ApprovedUser == userCode
                    //            || p.CreatedUser == userCode
                    //            || p.CreatedRequest == userCode
                    //            || p.StatusChangedUser == userCode
                    //            || p.FUChangedUser == userCode
                    //            || p.BUChangedUser == userCode
                    //            || p.VOChangedUser == userCode
                    //            || p.PAChangedUser == userCode)
                    //            select p;

                    //if (query.Count() > 0)
                    //{
                    //    throw new Exception("Người dùng đã dc sử dụng, không thể xóa!");
                    //}
                    #endregion

                    #region // Build:
                    Sys_User user = new Sys_User() { UserCode = userCode };
                    _db.Sys_User.Attach(user);
                    _db.Sys_User.Remove(user);
                    _db.SaveChanges();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // AddUser
        public void AddUser(Sys_User user)
        {
            #region // Temp
            string strFunctionName = "AddUser";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.Sys_User.Add(user);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // UpdateUser
        public void UpdateUser(Sys_User user)
        {
            #region // Temp
            string strFunctionName = "UpdateUser";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    Sys_User com = _db.Sys_User.Where(p => p.UserCode == user.UserCode).SingleOrDefault();

                    com.UserName = user.UserName;
                    //com.UserPassword = user.UserPassword;
                    com.Remark = user.Remark;
                    com.Position = user.Position;
                    com.FlagActive = user.FlagActive;
					if (!string.IsNullOrEmpty(user.FlagAdmin))
					{
						com.FlagAdmin = user.FlagAdmin;
					}

					if (!string.IsNullOrEmpty(user.SupperAdmin))
					{
						com.SupperAdmin = user.SupperAdmin;
					}
                    com.Email = user.Email;
                    com.Email = user.Email;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // UserChangedPassword
        public void UserChangedPassword(string usercode, string userPassword, string userPasswordNew, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "UserChangedPassword";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "usercode", usercode
						, "userPassword", userPassword
						, "userPasswordNew", userPasswordNew
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    Sys_User user = _db.Sys_User.Where(p => p.UserCode == usercode).SingleOrDefault();
                    if (user != null)
                    {
                        if (user.UserPassword != Utilities.Encrypt(userPassword))
                        {
                            throw new Exception("Current Password Invaid");
                        }
                        user.UserPassword = Utilities.Encrypt(userPasswordNew);
                        _db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // UserResetPassword
        public Sys_User UserResetPassword(string usercodeOrEmail, string type, ref string alParamsOutError)
        {
            #region // Temp
            string strFunctionName = "UserResetPassword";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
						, "usercode", usercodeOrEmail
                        , "type", type
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    Random d = new Random();
                    var numberNewPassword = d.Next(100000, 999999);
                    string strPasswordGenerate = string.Empty;
                    if (type.Equals("0"))
                    {
                        var user = (from u in _db.Sys_User
                                    join c in _db.Customers on u.UserCode equals c.UserCode
                                    where c.UserCode == usercodeOrEmail || c.Email == usercodeOrEmail
                                    select u
                            ).FirstOrDefault();

                        if (user != null)
                        {
                            user.UserPassword = Utilities.Encrypt(numberNewPassword+"");
                            _db.SaveChanges();
                            return user;
                        }
                        else
                        {
                            throw new Exception("Không có tài khoản tồn tại với email này.");
                        }
                    }

                    if (type.Equals("1"))
                    {
                        Sys_User user = _db.Sys_User.Where(p => p.UserCode == usercodeOrEmail).SingleOrDefault();
                        if (user != null)
                        {
                            user.UserPassword = Utilities.Encrypt(numberNewPassword+"");
                            _db.SaveChanges();
                            return user;
                        }
                        else
                        {
                            throw new Exception("Không có tài khoản tồn tại với email này.");
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                alParamsOutError = string.Format("Inner Exception : {0}\r\nErrorMsg: {1}\r\nErrorDetail: {2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return null;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #endregion

        #region // Sys_Group

        #region // GetGroup
        public List<Sys_Group> GetGroup(string groupCode, string groupName)
        {
            #region // Temp
            string strFunctionName = "GetGroup";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "GroupCode",groupCode
						, "GroupName",groupName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.Sys_Group
                                 select p;

                    if (!string.IsNullOrEmpty(groupCode))
                        result = result.Where(p => p.GroupCode == groupCode);

                    if (!string.IsNullOrEmpty(groupName))
                        result = result.Where(p => p.GroupName == groupName);

                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<Sys_Group>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // DeleteGroup
        public void DeleteGroup(string groupCode)
        {
            #region // Temp
            string strFunctionName = "DeleteGroup";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "groupCode",groupCode
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    Sys_Group Group = new Sys_Group() { GroupCode = groupCode };
                    _db.Sys_Group.Attach(Group);
                    _db.Sys_Group.Remove(Group);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // AddGroup
        public void AddGroup(Sys_Group group)
        {
            #region // Temp
            string strFunctionName = "AddGroup";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.Sys_Group.Add(group);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // UpdateGroup
        public void UpdateGroup(Sys_Group group)
        {
            #region // Temp
            string strFunctionName = "UpdateGroup";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    Sys_Group com = _db.Sys_Group.Where(p => p.GroupCode == group.GroupCode).SingleOrDefault();

                    com.GroupName = group.GroupName;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion
        #endregion

        #region // Sys_Object

        #region // GetObject
        public List<Sys_Object> GetObject(string objectCode, string objectName, string objectType, string flagActive)
        {
            #region // Temp
            string strFunctionName = "GetObject";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "objectCode",objectCode
						, "objectName",objectName
						, "objectType",objectType
						, "flagActive",flagActive
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.Sys_Object
                                 select p;

                    if (!string.IsNullOrEmpty(objectCode))
                        result = result.Where(p => p.ObjectCode == objectCode);

                    if (!string.IsNullOrEmpty(objectName))
                        result = result.Where(p => p.ObjectName == objectName);

                    if (!string.IsNullOrEmpty(objectType))
                        result = result.Where(p => p.ObjectType == objectType);

                    if (!string.IsNullOrEmpty(flagActive))
                        result = result.Where(p => p.FlagActive == flagActive);

                    return result.OrderBy(p=>p.ParentObjectCode).OrderBy(p=>p.OPriority).ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<Sys_Object>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // Remove
        public void Remove(string objectCode)
        {
            #region // Temp
            string strFunctionName = "Remove";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "objectCode",objectCode
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    Sys_Object Object = new Sys_Object() { ObjectCode = objectCode };
                    _db.Sys_Object.Attach(Object);
                    _db.Sys_Object.Remove(Object);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // Add
        public void Add(Sys_Object obj)
        {
            #region // Temp
            string strFunctionName = "Add";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var _db = new EcmsEntities())
                {
                    _db.Sys_Object.Add(obj);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // UpdateObject
        public void UpdateObject(Sys_Object obj)
        {
            #region // Temp
            string strFunctionName = "UpdateObject";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    Sys_Object com = _db.Sys_Object.Where(p => p.ObjectCode == obj.ObjectCode).SingleOrDefault();

                    com.ObjectName = obj.ObjectName;
                    com.FlagActive = obj.FlagActive;
                    com.ObjectType = obj.ObjectType;
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion
        #endregion

        #region // Sys_MapGroupObject

        #region // GetSysMapGroupObject
        public List<MapGroupObjectModel> GetSysMapGroupObject(string groupCode, string objectCode, string objectType, string flagActive)
        {
            #region // Temp
            string strFunctionName = "GetSysMapGroupObject";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "groupCode",groupCode
						, "objectCode",objectCode
						, "objectType", objectType
						, "flagActive", flagActive
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.Sys_MapGroupObject
                                 join g in _db.Sys_Group on p.GroupCode equals g.GroupCode into g_join
                                 from g in g_join.DefaultIfEmpty()
                                 join o in _db.Sys_Object on p.ObjectCode equals o.ObjectCode into o_join
                                 from o in o_join.DefaultIfEmpty()
                                 select new MapGroupObjectModel
                                 {
                                     GroupCode = p.GroupCode,
                                     GroupName = g.GroupName,
                                     ObjectCode = p.ObjectCode,
                                     ObjectName = o.ObjectName,
                                     ObjectType = p.ObjectType,
                                     FlagActive = o.FlagActive,
                                     Remark = p.Remark
                                 };

                    if (!string.IsNullOrEmpty(objectCode))
                        result = result.Where(p => p.GroupCode == groupCode);

                    if (!string.IsNullOrEmpty(objectCode))
                        result = result.Where(p => p.ObjectCode == objectCode);

                    if (!string.IsNullOrEmpty(objectType))
                        result = result.Where(p => p.ObjectType == objectType);

                    if (!string.IsNullOrEmpty(flagActive))
                        result = result.Where(p => p.FlagActive == flagActive);

                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<MapGroupObjectModel>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // AddMapGroupObject
        public void AddMapGroupObject(List<Sys_MapGroupObject> lstGrpObj)
        {
            #region // Temp
            string strFunctionName = "AddMapGroupObject";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var ts = new TransactionScope())
                {
                    using (var _db = new EcmsEntities())
                    {
                        foreach (var item in lstGrpObj)
                        {
                            _db.Sys_MapGroupObject.Add(item);
                        }
                        _db.SaveChanges();
                    }
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // UpdateMapGroupObject
        public void UpdateMapGroupObject(string groupCode, List<Sys_MapGroupObject> lstGrpObj)
        {
            #region // Temp
            string strFunctionName = "UpdateMapGroupObject";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
						"groupCode", groupCode
			            });
            #endregion

            try
            {
                using (var ts = new TransactionScope())
                {
                    using (var _db = new EcmsEntities())
                    {
                        #region // Delete current mapObject
                        var mapGObj = _db.Sys_MapGroupObject.Where(p => p.GroupCode == groupCode);
                        foreach (var item in mapGObj)
                        {
                            _db.Sys_MapGroupObject.Remove(item);
                        }
                        #endregion

                        #region // insert new
                        foreach (var item in lstGrpObj)
                        {
                            _db.Sys_MapGroupObject.Add(item);
                        }
                        #endregion

                        _db.SaveChanges();
                    }
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion
        #endregion

        #region // Sys_MapGroupUsers

        #region // GetSysMapGroupUsers
        public List<MapUserGroupModel> GetSysMapGroupUsers(string groupCode, string userCode, string flagActive)
        {
            #region // Temp
            string strFunctionName = "GetSysMapGroupObject";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
                        , "groupCode",groupCode
						, "userCode",userCode
						, "flagActive",flagActive
			            });
            #endregion

            try
            {
                using (var _db = new EcmsEntities())
                {
                    var result = from p in _db.Sys_MapUserGroup
                                 join g in _db.Sys_Group on p.GroupCode equals g.GroupCode into g_join
                                 from g in g_join.DefaultIfEmpty()
                                 join u in _db.Sys_User on p.UserCode equals u.UserCode into u_join
                                 from u in u_join.DefaultIfEmpty()
                                 select new MapUserGroupModel
                                 {
                                     GroupCode = p.GroupCode,
                                     GroupName = g.GroupName,
                                     UserCode = p.UserCode,
                                     UserName = u.UserName,
                                     FlagActive = u.FlagActive,
                                     Remark = p.Remark
                                 };

                    if (!string.IsNullOrEmpty(groupCode))
                        result = result.Where(p => p.GroupCode == groupCode);

                    if (!string.IsNullOrEmpty(userCode))
                        result = result.Where(p => p.UserCode == userCode);

                    if (!string.IsNullOrEmpty(flagActive))
                        result = result.Where(p => p.FlagActive == flagActive);

                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return new List<MapUserGroupModel>();
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion

        #region // AddMapGroupUsers
        public void AddMapGroupUsers(List<Sys_MapUserGroup> lstUserObj)
        {
            #region // Temp
            string strFunctionName = "AddMapGroupObject";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName
			            });
            #endregion
            try
            {
                using (var ts = new TransactionScope())
                {
                    using (var _db = new EcmsEntities())
                    {
                        foreach (var item in lstUserObj)
                        {
                            _db.Sys_MapUserGroup.Add(item);
                        }
                        _db.SaveChanges();
                    }
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        #endregion

        #region // UpdateMapGroupUsers
        public void UpdateMapGroupUsers(string groupCode, List<Sys_MapUserGroup> lstGrpUsers)
        {
            #region // Temp
            string strFunctionName = "UpdateMapGroupUsers";
            ArrayList alParamsCoupleError = new ArrayList(new object[]{
			            "strFunctionName", strFunctionName,
						"groupCode", groupCode
			            });
            #endregion

            try
            {
                using (var ts = new TransactionScope())
                {
                    using (var _db = new EcmsEntities())
                    {
                        #region // Delete current map
                        var mapGObj = _db.Sys_MapUserGroup.Where(p => p.GroupCode == groupCode);
                        foreach (var item in mapGObj)
                        {
                            _db.Sys_MapUserGroup.Remove(item);
                        }
                        #endregion

                        #region // insert new
                        foreach (var item in lstGrpUsers)
                        {
                            _db.Sys_MapUserGroup.Add(item);
                        }
                        #endregion

                        _db.SaveChanges();
                    }
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(alParamsCoupleError.ToArray(), ex, false);
                return;
            }
            finally
            {
                NLogLogger.Info(string.Format("finally: {0}, Time: {1}", strFunctionName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }
        #endregion
        #endregion

        #region // sendmail

        #endregion

    }

    #region // Model
    public class DepartmentModel
    {
        public Int32? DepartmentId { set; get; }
        public Int32? CompanyId { set; get; }
        public String DepartmentCode { set; get; }
        public String DepartmentName { set; get; }
        public String Description { set; get; }
        public String CompanyName { set; get; }
    }

    public class ProjectModel
    {
        public Int32? ProjectId { set; get; }
        public String ProjectName { set; get; }
        public String ProjectOwner { set; get; }
        public Int32? CompanyId { set; get; }
        public String CompanyName { set; get; }
        public String Description { set; get; }
    }

    public class FeeModel
    {
        public Int32? FeeId { set; get; }
        public Int32? ParentFeeId { set; get; }
        public String ParentFeeName { set; get; }
        public String FeeName { set; get; }
        public String FeeCode { set; get; }
        public String Description { set; get; }

    }

    public class UserModel
    {
        public String UserCode { set; get; }
        public String UserName { set; get; }
        public String UserPassword { set; get; }
        public String Email { get; set; }
        public String Remark { set; get; }
        public String FlagActive { set; get; }
        public String FlagAdmin { set; get; }
        public String DepartmentName { set; get; }
        public Int32? CompanyId { set; get; }
        public String CompanyName { set; get; }
        public String Position { set; get; }
        public Int32? DepartmentId { set; get; }
        public String GroupCode { set; get; }
        public String GroupName { set; get; }
        public String ObjectCode { set; get; }
        public String ObjectName { set; get; }
        public String ParentObjectCode { set; get; }
        public String ObjectType { set; get; }
        public String ObjectTemplate { set; get; }
        public String IsFUApproved { set; get; }
		public String SupperAdmin { set; get; }
		public int? OPriority { set; get; }
    }

    public class MapGroupObjectModel
    {
        public String ObjectCode { set; get; }
        public String ObjectName { set; get; }
        public String GroupCode { set; get; }
        public String GroupName { set; get; }
        public String ObjectType { set; get; }
        public String FlagActive { set; get; }
        public String Remark { set; get; }
    }

    public class MapUserGroupModel
    {
        public String UserCode { set; get; }
        public String UserName { set; get; }
        public String GroupCode { set; get; }
        public String GroupName { set; get; }
        public String FlagActive { set; get; }
        public String Remark { set; get; }
    }

    public class MailWarrning
    {
        public String UserCode { set; get; }
        public String UserName { set; get; }
        public String Email { set; get; }
        public Int32 TranNoCheck { set; get; }
        public Int32 CompanyId { get; set; }
    }

    public class EnterBanlanceModel
    {
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public double Money { get; set; }
        public string Remark { get; set; }
    }

    #endregion
}
