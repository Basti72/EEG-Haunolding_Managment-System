namespace EGG_Haunolding_Management_System.Class
{
    public class DataItem
    {
        public string Origin { get; set; }

        public DateTime Time { get; set; }

        public int Saldo { get; set; }

        public int SaldoAvg { get; set; }

        public DataItem(string origin, DateTime time, int saldo, int saldoAvg)
        {
            Origin = origin;
            Time = time;
            Saldo = saldo;
            SaldoAvg = saldoAvg;
        }

        public DataItem()
        {
            
        }
    }
}