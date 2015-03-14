using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System.Reflection;
using System.Diagnostics;

namespace CommonUtils
{
    public static class NLogLogger
    {
        static NLogLogger()
        {
			Logger = LogManager.GetCurrentClassLogger();
        }

        public static Logger Logger { get; set; }

        public static void LogInfo(string message)
        {
            Info(message, false);
        }

        public static void Info(string message, bool sendMail)
        {
            Logger.Info(":\t" + message);
            if (sendMail)
                SendmailHelper.SendDebug(message);
        }

		public static void Info(object[] arrobjParamsCouple, string message, bool sendMail)
		{
			StringBuilder strMessage= new StringBuilder();
			strMessage.Append(message + "\n");
			for (int i = 0; i < arrobjParamsCouple.Length; i += 2)
			{
				strMessage.Append("------------------------------------------------------------\n");
				strMessage.AppendFormat("{0} : {1}", arrobjParamsCouple[i], arrobjParamsCouple[i + 1]);
				strMessage.Append("\n");
			}
			Logger.Info(strMessage.ToString());
			if (sendMail)
				SendmailHelper.SendDebug(strMessage.ToString());
		}

        public static void Info(string message)
        {
            Info(message, false);
        }

        public static void TraceMessage(string message)
        {
            Logger.Trace("\t" + message);
        }

        public static void PublishException(Exception ex, bool sendmail)
        {
            DebugMessage(ex.Message + Environment.NewLine + ex.StackTrace, sendmail);
        }

		public static void PublishParamater(object[] arrobjParamsCouple)
		{
			StringBuilder strMessage = new StringBuilder();
			for (int i = 0; i < arrobjParamsCouple.Length; i += 2)
			{
				strMessage.Append("------------------------------------------------------------\n");
				strMessage.AppendFormat("{0} : {1}", arrobjParamsCouple[i], arrobjParamsCouple[i + 1]);
				strMessage.Append("\n");
			}
			strMessage.Append("------------------------------------------------------------\n");
			//strMessage.AppendFormat("{0}\t\n{1}\t\n{2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
			DebugMessage(strMessage.ToString(), false);
		}

		public static void PublishException(object[] arrobjParamsCouple, Exception ex, bool sendmail)
		{
			StringBuilder strMessage = new StringBuilder();
			for (int i = 0; i < arrobjParamsCouple.Length; i += 2)
			{
				strMessage.Append("------------------------------------------------------------\n");
				strMessage.AppendFormat("{0} : {1}", arrobjParamsCouple[i], arrobjParamsCouple[i + 1]);
				strMessage.Append("\n");
			}
			strMessage.Append("------------------------------------------------------------\n");
			strMessage.AppendFormat("{0}\t\n{1}\t\n{2}", ex.InnerException == null ? "" : ex.InnerException.Message, ex.Message, ex.StackTrace);
			DebugMessage(strMessage.ToString(), sendmail);
		}

        public static void PublishException(Exception ex)
        {
            PublishException(ex, false);
        }

        public static void DebugMessage(object o)
        {
            DebugMessage(GetValueOfObject(o));
        }

        public static void DebugMessage(string message, bool sendEmail)
        {
            string m = GetCalleeString() + Environment.NewLine + "\t" + message;
            Logger.Debug(":\t" + m);

            if (sendEmail)
                SendmailHelper.SendDebug(GetCalleeString() + Environment.NewLine + "\t" + message);
        }

        public static void DebugMessage(string message)
        {
            DebugMessage(message, false);
        }

        public static void LogDebug(string p)
        {
            DebugMessage(p);
        }

        public static void LogWarning(object o)
        {
            LogWarning(GetValueOfObject(o));
        }

        public static void LogWarning(string message)
        {
            LogWarning(message, true);
        }

        public static void LogWarning(string message, bool sendMail)
        {
            string error = GetCalleeString() + Environment.NewLine + "\t" + message;

            Logger.Warn(":\t" + error);

            if (sendMail)
                SendmailHelper.SendError(error);
        }

        private static string GetCalleeString()
        {
            foreach (StackFrame sf in new StackTrace().GetFrames())
            {
                if (sf.GetMethod().ReflectedType.Namespace != "Portal.Common")
                {
                    return string.Format("{0}.{1} ", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
                }
            }

            return string.Empty;
        }

        public static string GetValueOfObject(object ob)
        {
            var sb = new StringBuilder();
            try
            {
                foreach (PropertyInfo piOrig in ob.GetType().GetProperties())
                {
                    object editedVal = ob.GetType().GetProperty(piOrig.Name).GetValue(ob, null);
                    sb.AppendFormat("{0}:{1}\t ", piOrig.Name, editedVal);
                }
            }
            catch
            {
            }
            return sb.ToString();
        }
    }
}
