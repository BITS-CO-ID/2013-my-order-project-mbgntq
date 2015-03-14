using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public static class Constansts
    {
		public const string CustomerAccount_MBGN = "200300402";
		public const int CustomerIdAccount_MBGN = 225;
		public const int CountryIdChina = 5;
		public const string FlagActive = "1";
		public const string FlagInActive = "0";
		public const string sysadmin = "sysadmin";
		public const string CssClass_buttonDisable = "buttonDisable";
		public const string CssClass_button = "button";
		public const string Const_Empty = "empty";
		public const string CalFeeDelay = "1"; // Có tính phí trả chậm, mặc định = null hoặc =1 là có tính phí
		public const string NoCalFeeDelay = "0"; // Không tính phí trả chậm
		public const int DayFeeDefault = 3;
		public const int AddValueDefault = 5;
		public const int NumberRoundDefault = 2;
		public const int NumberRoundMin = 0;

		public const string SESSION_ORDERMODEL_LIST = "SESSION_ORDERMODEL_LIST";

		public const string SESSION_ORDERID = "SESSION_ORDERID";
		public const string SESSION_ORDERMODEL = "SESSION_ORDERMODEL";
		public const string SESSION_ORDERNO = "SESSION_ORDERNO";
		public const string SESSION_ORDERCART = "SESSION_ORDERCART";
		public const string SESSION_CURRENT_ORDERCART = "SESSION_CURRENT_ORDERCART";

        public const string SS_ORDERMODEL_LIST_ADMIN = "SS_ORDERMODEL_LIST_ADMIN";
        public const string SS_ORDERMODEL_ADMIN = "SS_ORDERMODEL_ADMIN";

        public const string SS_INVOICE_LIST_ADMIN = "SS_INVOICE_LIST_ADMIN";
        public const string SS_INVOICE_ADMIN = "SS_INVOICE_ADMIN";

        public const string SS_ORDER_OUTBOUND_LIST_ADMIN = "SS_ORDER_OUTBOUND_LIST_ADMIN";
        public const string SS_ORDER_OUTBOUND_ADMIN = "SS_ORDER_OUTBOUND_ADMIN";

        public const string SS_STOCK_INOUT_LIST_ADMIN = "SS_STOCK_INOUT_LIST_ADMIN";
        public const string SS_STOCK_INOUT_ADMIN = "SS_STOCK_INOUT_ADMIN";

        public const string SS_ORDER_DETAIL_ADMIN = "SS_ORDER_DETAIL_ADMIN";
		public const string SESSION_EMPLOYEE = "SESSION_EMPLOYEE";

		public const bool IsAuthenticateOn = true;
    }

	#region // Trạng thái Order
	/* Note các trạng thái: Đã đến Mỹ, Đã về việt Nam, Đã giao hàng:
	 * Với đơn hàng vận chuyển thì các trạng thái này ghi nhận ở OrderDetail cùng với chi tiết ngày cho các trạng thái đó
	 * Với đơn hàng mua hộ thì các trạng thai này được ghi nhận ở đơn hàng nước ngoài cùng với các trạng thái đó
	 */

	//public static class TransportStatus
	//{
	//    public const Int32 Created = 0;
	//    public const string CreatedText = "Thay đổi";
        
	//    public const Int32 ToUsa = 1; // Đã đến Mỹ
	//    public const string ToUsaText = "Đã đến Mỹ";

	//    public const Int32 Processing = 2; // Đang xử lý
	//    public const string ProcessingText = "Đang xử lý";

	//    public const Int32 ToVn = 3; // Đã về vn
	//    public const string ToVnText = "Đã về Việt Nam";

	//    public const Int32 Paided = 4; // MBGN đã check, không khớp
	//    public const string PaidedText = "Đã giao hàng";

	//}

	public static class OrderInStatus
	{
		// Trạng thái Order
		public const Int32 QuotePending = 1; // BG Chưa trả lời
		public const string QuotePendingText = "Chưa trả lời";

		public const Int32 QuoteConfirmed = 2; // BG Đã trả lời
		public const string QuoteConfirmedText = "Đã trả lời";

		public const Int32 OrderPending = 3; // ĐH Chưa trả lời
		public const string OrderPendingText = "Chưa trả lời";

		public const Int32 OrderConfirmed = 4; // Đã xác nhận ĐH
		public const string OrderConfirmedText = "Đã xác nhận";

		public const Int32 OrderCancel = 5; // Hủy
		public const string OrderCancelText = "Hủy";

		public const Int32 Finished = 6; // Hoàn thành
		public const string FinishedText = "Hoàn thành";

        public const Int32 Deliveried = 7; // Đã giao hàng
        public const string DeliveriedText = "Đã giao hàng";

		public const Int32 OrderDeleted = 8; // Tạm xóa
		//public const string DeliveriedText = "Đã giao hàng";
	}

	public static class OrderOutboundStatus
	{
		// Trạng gom hàng
		// Trạng gom hàng
		public const Int32 NoStatus = 0; // Đang gom hàng
		//public const string InProcessText = "Đã gom, đang xử lý";

		public const Int32 InProcess = 1; // Đang gom hàng
		public const string InProcessText = "Đã gom, đang xử lý";

		public const Int32 IsBuy = 2; // Đã mua hàng
		public const string IsBuyText = "Đã mua";

		public const Int32 Cancel = 3; // Đã hủy
		public const string CancelText = "Hủy";

		public const Int32 InvOutbound = 4; // Đã đến Mỹ
        public const string InvOutboundText = "Đã đến Mỹ";

		public const Int32 InvInbound = 5; // Đã về Việt Nam
        public const string InvInboundText = "Đã về Việt Nam";

		public const Int32 InvInboundMBGN = 6; // Hàng đã về kho MGBN
		public const string InvInboundMBGNText = "Đã giao hàng";
	}

	public static class InvoiceStatus
	{
		// Trạng thái thanh toán
		public const Int32 Pending = 1; // Gửi thanh toán
		public const string PendingText = "Mới gửi thanh toán";

		public const Int32 Confirm = 2; // Đã xác nhận khớp thanh toán
		public const string ConfirmText = "Đã khớp thanh toán";

		public const Int32 NotConfirm = 3; // Không khớp thanh toán
		public const string NotConfirmText = "Không khớp thanh toán";

		//public const Int32 Reject = 4; // Hủy thanh toán
		//public const string RejectText = "Hủy thanh toán";
	}

	public static class StockStatus
	{
		// Trạng thái Order
		public const Int32 StockPending = 1; // PN/PX chưa xác nhận
		public const string StockPendingText = "Chưa xác nhận";

		public const Int32 StockConfirmed = 2; // PN/PX đã xác nhận
		public const string StockConfirmedText = "Đã xác nhận";

		public const Int32 StockCancel = 3; // Đã Hủy
		public const string StockCancelText = "Đã Hủy";
	}

	public static class GoodStatus
	{
		// Tình trạng món hàng trong kho
		public const Int32 GoodInStock = 1; // Sản phẩm còn trong kho
		public const Int32 GoodOutStock = 2; // Sản phẩn đã bán
	}

	public static class StockType
	{
		// Trạng thái Order
		public const Int32 StockIn = 1; // Phiếu nhập
		public const Int32 StockOut = 2; // Phiếu xuất
	}

	public static class OrderType_Const
	{
		public const int Quote = 1;
		public const string Quote_Text = "BG - Yêu cầu báo giá";

		public const int OrderBylink = 2;
		public const string OrderBylink_Text = "MH - Đơn hàng mua hộ";

        public const int OrderShipOnly = 3;
		public const string OrderShipOnly_Text = "VC - Đơn hàng vận chuyển";

		public const int OrderByProduct = 4;
		public const string OrderByProduct_Text = "CS - Đặt hàng sản phẩm";
	}

	#endregion

	// BusinessCode
	public static class Const_BusinessCode
	{
		public const string Business_101 = "101";
		public const string Business_101_Text = "Công";

		public const string Business_ORGRATE = "ORGRATE";
		public const string Business_ORGRATE_Text = "Tỉ giá quy đổi theo xuất xứ";

		public const string Business_ORGRATEDE = "ORGRATEDE";
		public const string Business_ORGRATEDE_Text = "Tỉ giá vận chuyển";

		public const string Business_201 = "201";
		public const string Business_201_Text = "Khách hàng gửi xác nhận thanh toán";

		public const string Business_202 = "202";
		public const string Business_202_Text = "QC247 Hoàn tiền cho khách hàng";

		public const string Business_203 = "203";
		public const string Business_203_Text = "QC247 mua hàng từ đối tác";

		public const string Business_204 = "204";
		public const string Business_204_Text = "QC247 nạp tiền vào tài khoản";

		public const string Business_205 = "205";
		public const string Business_205_Text = "Khách hàng nạp tiền vào tài khoản";

		public const string Business_206 = "206";
		public const string Business_206_Text = "QC247 Xác nhận đã giao hàng";

		public const string Business_207 = "207";
		public const string Business_207_Text = "Chuyển thanh toán từ TK KH cho Đơn hàng";

		public const string Business_208 = "208";
		public const string Business_208_Text = "Phân bổ thanh toán cho đơn hàng";

		public const string Business_209 = "209";
		public const string Business_209_Text = "Phân bổ thanh toán cho đơn hàng từ SD khả dụng";

		public const string Business_301 = "301"; // Có áp phí trả chậm cho toàn hệ thống ko?
		public const string Business_302 = "302"; // Số ngày cho phép trả chậm
		public const string Business_303 = "303"; // Giá trị trả chậm

		public const string Business_401 = "401"; // Phí Ship mua hộ
		public const string Business_402 = "402"; // Phí Ship vận chuyển
	}

	public static class Const_ProductType
	{
		public const string ProductType_Order = "Order";
		public const string ProductType_OrderText = "Hàng chờ đặt";

		public const string ProductType_InStock = "InStock";
		public const string ProductType_InStockText = "Hàng có sẵn trong kho";
	}

	public static class Const_QuantitySatus
	{
		public const string QuantitySatus_HaveText = "Còn hàng";
		public const string QuantitySatus_NoHaveText = "Hết hàng";
	}

	public static class Const_ProductStatus
	{
		public const int ProductStatus_New = 1;
		public const string ProductStatus_NewText = "Hàng mới";

		public const int ProductStatus_Secondhand = 2;
		public const string ProductStatus_SecondhandText = "Hàng đã qua sử dụng";
	}

	public class MenuFunction
	{
		public const string Func_VCConfirm = "Func_VCConfirm"; // Quyền Nút xác nhận đơn hàng vận chuyển
		public const string Func_VCFinished = "Func_VCFinished"; // Quyền Nút xác nhận Hoàn thành đơn hàng vận chuyển
		public const string Func_VCDeliverly = "Func_VCDeliverly"; // Quyền nút xác nhận giao hàng vận chuyển
		public const string Func_VCCancel = "Func_VCCancel"; // Quyền nút Hủy đơn hàng vận chuyển
		public const string Func_VCRevertPending = "Func_VCRevertPending"; // Quyền Nút Hoàn lại TT chưa xác nhận đơn hàng vận chuyển
		public const string Func_VCRevertFinished = "Func_VCRevertFinished"; // Quyền Nút Hoàn lại TT Hoàn thành đơn hàng vận chuyển

		public const string Func_MHConfirm = "Func_MHConfirm"; // Quyền Nút xác nhận đơn hàng Mua hộ
		public const string Func_MHFinished = "Func_MHFinished"; // Quyền Nút xác nhận Hoàn thành đơn hàng Mua hộ
		public const string Func_MHDeliverly = "Func_MHDeliverly"; // Quyền nút xác nhận giao hàng Mua hộ
		public const string Func_MHCancel = "Func_MHCancel"; // Quyền nút Hủy đơn hàng Mua hộ
		public const string Func_MHRevertPending = "Func_MHRevertPending"; // Quyền Nút Hoàn lại TT chưa xác nhận đơn hàng Mua hộ
		public const string Func_MHRevertFinished = "Func_MHRevertFinished"; // Quyền Nút Hoàn lại TT Hoàn thành đơn hàng Mua hộ

	}
}
