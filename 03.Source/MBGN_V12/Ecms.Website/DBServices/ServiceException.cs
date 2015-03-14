using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecms.Website.DBServices
{
	public class ServiceException : Exception
	{
		private string _errorMessage;
		private string _errorDetail;
		private string _errorCode;
		private string _errorCode01;

		private object _tag;

		public string ErrorDetail
		{
			set { _errorDetail = value; }
			get { return _errorDetail; }
		}

		public string ErrorCode
		{
			set { _errorCode = value; }
			get { return _errorCode; }
		}

		public string ErrorCode01
		{
			set { _errorCode01 = value; }
			get { return _errorCode01; }
		}

		public string ErrorMessage
		{
			set { _errorMessage = value; }
			get { return _errorMessage; }
		}

		public object Tag
		{
			set { _tag = value; }
			get { return _tag; }
		}
	}
}
