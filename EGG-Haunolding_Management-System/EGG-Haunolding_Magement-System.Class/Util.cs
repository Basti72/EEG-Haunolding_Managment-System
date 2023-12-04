using EGG_Haunolding_Management_System.Class;

namespace EGG_Haunolding_Magement_System.Class
{
    public static class Util
    {
        public static DataItem ToDataItem(this JsonDataItem jsonDataItem)
        {
            DataItem dataItem = new DataItem();
            dataItem.Time = DateTime.Parse(jsonDataItem.zeittext);
            dataItem.Saldo = jsonDataItem.saldo;
            dataItem.SaldoAvrg = jsonDataItem.saldoavg;
            dataItem.Origin = "dummy";
            return dataItem;
        }
    }
}