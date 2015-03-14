using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace CommonUtils
{
    public class SendMail
    {
        public int MailID { get; set; }

        public string MailFromName { get; set; }

        public string MailFrom { get; set; }

        public string MailTo { get; set; }

        public string MailSubject { get; set; }

        public string MailBody { get; set; }

        public int Status { get; set; }

        public int ErrorCode { get; set; }
    }

    public class SendmailHelper
    {
        private static readonly string MailServerUser = ConfigurationManager.AppSettings["MailServerUser"];
        private static readonly string MailServerPassword = ConfigurationManager.AppSettings["MailServerPassword"];
        private static readonly string MailServerHost = ConfigurationManager.AppSettings["MailServerHost"];
        private static readonly string MailServerPort = ConfigurationManager.AppSettings["MailServerPort"];
        private static readonly string MailRecieverError = ConfigurationManager.AppSettings["MailRecieverError"];


        private static void Send(SendMail MailInfo)
        {            
            var message = new MailMessage();
            var smtpClient = new SmtpClient();
            smtpClient.Host = MailServerHost;
            smtpClient.Port = Convert.ToInt32(MailServerPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(MailServerUser, MailServerPassword);
            smtpClient.EnableSsl = true;
            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                
                message.From = new MailAddress(MailServerUser, "QuangChau247.vn");
                message.IsBodyHtml = true;
                message.Subject = MailInfo.MailSubject.Replace("\r\n", " ");
                message.Body = MailInfo.MailBody;
                message.To.Add(MailInfo.MailTo);
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex, false);

                smtpClient = new SmtpClient(MailServerHost, Convert.ToInt32(MailServerPort));
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(MailServerUser, MailServerPassword);
                smtpClient.EnableSsl = true;
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                message.From = new MailAddress(MailServerUser,
                                               string.Format("Alter QC247 Debug [{0}]", Environment.MachineName));
                message.IsBodyHtml = true;
                message.Subject = MailInfo.MailSubject.Replace("\r\n", " ") + " from " + Environment.MachineName;
                message.Body = MailInfo.MailBody;
                message.To.Add(MailRecieverError);
                smtpClient.Send(message);
            }
            finally {
                smtpClient.Dispose();
            }
        }


        /// <summary>
        /// gửi thông báo lỗi tới email được config khi chương trình gặp lỗi
        /// </summary>
        /// <param name="messenger"></param>
        public static void SendError(string messenger)
        {
            try
            {
                messenger = "Lỗi" + Environment.NewLine + "Date :" + DateTime.Now + Environment.NewLine + messenger;
                string[] listMail = MailRecieverError.Split(';');
                for (int i = 0; i < listMail.Length; i++)
                {
                    var sendmail = new SendMail
                    {
                        MailSubject =
                            string.Format("[{0}][ErrorMail][{1}] {2}!", Environment.MachineName,
                                          "QC247",
                                          messenger.Length > 20
                                              ? messenger.Substring(0, 16)
                                              : "Error!").Replace("\r\n", " "),
                        MailTo = listMail[i] ?? MailServerUser,
                        MailBody = messenger
                    };
                    Send(sendmail);
                }
                return;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex, false);
                return;
            }
        }

        /// <summary>
        /// gửi thông báo lỗi tới email được config khi chương trình gặp lỗi
        /// </summary>
        /// <param name="messenger"></param>
        public static void SendDebug(string messenger)
        {
            try
            {
                messenger = "Debug" + Environment.NewLine + "Date :" + DateTime.Now + Environment.NewLine + messenger;
                string[] listMail = MailRecieverError.Split(';');

                for (int i = 0; i < listMail.Length; i++)
                {
                    var sendmail = new SendMail
                    {
                        MailSubject =
                            string.Format("[{0}][DebugInfo][{1}] {2}!",
                                            Environment.MachineName,
                                            "QC247",
                                            messenger.Length > 20
                                              ? messenger.Substring(0, 16)
                                              : "Debug Info!").Replace("\r\n", " "),
                        MailTo = listMail[i] ?? MailServerUser,
                        MailBody = messenger
                    };
                    Send(sendmail);
                }
                return;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex, false);
                return;
            }
        }

        /// <summary>
        /// Gửi email
        /// </summary>
        /// <param name="messenger"></param>
        public static void SendInfo(string messenger, string subject, string mailTo)
        {
            try
            {
                string[] listMail = mailTo.Split(';');
                for (int i = 0; i < listMail.Length; i++)
                {
                    var sendmail = new SendMail
                    {
                        MailSubject = subject,
                        MailTo = listMail[i] ?? MailServerUser,
                        MailBody = messenger
                    };
                    NLogLogger.Info(string.Format("MailSubject = {0} | MailTo = {1}", sendmail.MailSubject, sendmail.MailTo));
                    Send(sendmail);
                    NLogLogger.Info("End send mail");
                }
                return;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex, false);
                return;
            }
        }
    }
}
