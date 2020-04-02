namespace W1400.Utility
{
    public class Enums
    {
        /// <summary>
        /// Loại tài khoản trên VTCid
        /// </summary>

        public enum FunctionId
        {
            System = 1,
            Function = 2,
            User = 3,
            UserLog = 4,
            ErrorSystem = 5,
            Service = 6,
            Report = 7,
            GrantUser = 12,
            GrantService = 16,
            ReportAccLogin = 29, // Tổng Hợp tài khoản đăng nhập
            ReportAccSecure = 30, // Tổng Hợp Tài Khoản Bảo Mật
            ReportAccVerify = 31, // Tổng Hợp Xác Nhận Thông tin tài khoản
            ReportAccRegister = 32, //Tông Hợp Tại Khoản Đăng Ký
            ReportTransactionInput = 34, //[GD-Nạp] Tổng hợp giao dịch
            ReportDynamicTransactionInput = 35, //Báo cáo tùy chỉnh GD nạp
            ReportTransactionOutput = 37, //[GD-Tiêu] Tổng hợp giao dịch
            ReportAccMonitor = 38, //Tổng Hợp Tài Khoản Khách Hàng
            ReportDynamicTransactionOut = 39, //Báo cáo tùy chỉnh GD tiêu
            TransactionInputByMerchant = 41, //[GD-Nap] Theo Bộ Phận
            TransactionInputByProduct = 43, //[GD-Nap] Theo Sản Phẩm
            TransactionInputByProvider = 44, //[GD-Nap] Theo Nhà Cung Cấp
            TransactionOutputByMerchant = 45, //[GD-Tiêu] Theo Bộ Phận
            TransactionOtherGeneral = 47, //[GD-Khác] Tổng hợp
            ReportDynamicAccount = 47, //[Tài Khoản] Báo cáo TK tùy chỉnh
            TransactionSummaryImExIn = 49, //Tổng hợp dữ liệu Xuất Nhập Tồn của TK
            ReportGeneralSystem = 50 //Tổng hợp báo cáo hệ thống tài khoản VTC
        }
    }
}
