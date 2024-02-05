using EGG_Haunolding_Management_System.Class;
using System.Globalization;
using System.Security.Cryptography;

namespace EGG_Haunolding_Management_System.Class
{
    public static class Util
    {
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
        public static DataItem ToDataItem(this JsonDataItem jsonDataItem, string Origin)
        {
            Origin = Origin.Substring(Origin.IndexOf("/")+1);
            DataItem dataItem = new DataItem();
            if (jsonDataItem.zeittext == null)
                dataItem.Time = DateTime.Now;
            else
                dataItem.Time = DateTime.ParseExact(jsonDataItem.zeittext, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            dataItem.Saldo = jsonDataItem.saldo;
            dataItem.SaldoAvg = jsonDataItem.saldoavg;
            dataItem.Origin = Origin;
            return dataItem;
        }
        public static string CreateHash(string password, out string salt)
        {
            byte[] byteSalt = RandomNumberGenerator.GetBytes(16);
            salt = ToHex(byteSalt);
            return CreateHash(password, byteSalt);
        }

        private static string CreateHash(string password, byte[] salt)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 600000, HashAlgorithmName.SHA256, 16);
            return ToHex(hash);
        }

        private static string ToHex(byte[] hash)
        {
            return Convert.ToHexString(hash);
        }

        public static bool DoesPasswordMatch(string password, string hash, string salt)
        {
            byte[] saltBytes = Convert.FromHexString(salt);
            return CreateHash(password, saltBytes) == hash;
        }
    }
}