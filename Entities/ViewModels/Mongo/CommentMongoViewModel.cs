using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Entities.ViewModels.Mongo
{
    public class CommentMongoViewModel
    {
        public string _id { get; set; }
        public string Note { get; set; }
        public string phone { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
    }
    public class CommentMongoSearchViewModel
    {
        public string FromDateStr { get; set; }
        public DateTime? FromDate
        {
            get
            {
                if (!string.IsNullOrEmpty(FromDateStr))
                {
                    var lstDate = FromDateStr.Split('-');
                    if (lstDate.Length == 0 || lstDate.Length == 1)
                        lstDate = FromDateStr.Split('/');
                    FromDateStr = lstDate[0] + "/" + lstDate[1] + "/" + lstDate[2];
                    var fromDate = DateUtil.StringToDate(FromDateStr);
                    return new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day, 00, 00, 00, DateTimeKind.Local);
                }
                return null;
            }
        }
        public string ToDateStr { get; set; }
        public DateTime? ToDate
        {
            get
            {
                if (!string.IsNullOrEmpty(ToDateStr))
                {
                    var lstDate = ToDateStr.Split('-');
                    if (lstDate.Length == 0 || lstDate.Length == 1)
                        lstDate = ToDateStr.Split('/');
                    ToDateStr = lstDate[0] + "/" + lstDate[1] + "/" + lstDate[2];
                    var toDate = DateUtil.StringToDate(ToDateStr);
                    return new DateTime(toDate.Value.Year, toDate.Value.Month, toDate.Value.Day, 23, 59, 59, DateTimeKind.Local);
                }
                return null;
            }
        }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}
