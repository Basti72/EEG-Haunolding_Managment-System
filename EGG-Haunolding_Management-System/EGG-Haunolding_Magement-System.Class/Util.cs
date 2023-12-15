using EGG_Haunolding_Management_System.Class;
using System.Globalization;

namespace EGG_Haunolding_Management_System.Class
{
    public static class Util
    {
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
        public static DataItem ToDataItem(this JsonDataItem jsonDataItem)
        {
            DataItem dataItem = new DataItem();
            if (jsonDataItem.zeittext == null)
                dataItem.Time = DateTime.Now;
            else
                dataItem.Time = DateTime.Parse(jsonDataItem.zeittext, null, DateTimeStyles.AssumeUniversal);

            dataItem.Saldo = jsonDataItem.saldo;
            dataItem.SaldoAvg = jsonDataItem.saldoavg;
            dataItem.Origin = "dummy";
            return dataItem;
        }
    }
}