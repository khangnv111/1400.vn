using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W1400.DataAccess.DTO
{
    public class ArticleImage
    {
        public int STT { get; set; }
        public int ImageID { get; set; }
        public int ArticleID { get; set; }
        public string ArticleTitle { get; set; }
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string FilePath { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string Description { get; set; }
    }

    public class Event
    {
        public int STT { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; }
        public int ServiceID { get; set; }
        public int Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Order { get; set; } 
        public string Image { get; set; }
        public string Porpose { get; set; }
        public string HostUnit { get; set; }
        public string CoordUnit { get; set; }
        public string LegalDoc { get; set; }
        public string LegalFile { get; set; }
        public string Revenue { get; set; }
        public string Result { get; set; }
        public string Detail { get; set; }
        public bool IsDone { get; set; }
        public bool IsSetted { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public string EventDetail { get; set; }
        public string Commandcode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class Suggest
    {
        public long STT { get; set; }
        public int SuggestionID { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Suggestion { get; set; } 
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public string Answer { get; set; }
        public string CreateUser { get; set; }
        public DateTime ConfirmDate { get; set; }
    }

    public class EventReport
    {
        public int STT { get; set; }
        public int LogID { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; }
        public string Vender { get; set; }
        public long DataControl { get; set; }
        public long Pay1400 { get; set; }
        public long PayCharit { get; set; }
        public string Note { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string DataReviewTime { get; set; }
        public string Pay14ReviewTime { get; set; }
        public string PayCharReviewTime { get; set; }
        public string TimeAnswer { get; set; }

        public string TimeSignRecord { get; set; }
        public string TimehsPay { get; set; }
        public string TimeTelcoPay { get; set; }
        public string TimePay { get; set; }
        public string CreateUser { get; set; }
        public int SMSC_ID { get; set; }
        public string SMSHost { get; set; }
        public string CommandCode { get; set; }
        public string ServiceID { get; set; }
        public int Price { get; set; }
        public int Total_SMS { get; set; }
    }
}
