using System;
using System.Collections.Generic;

namespace HedgeNet.Models
{
    class ConvertData
    {
        public static string GetPasswordFreshness(string date)
        {
            DateTime updateDate = DateTime.ParseExact(date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            var total = (int)(DateTime.Now - updateDate).TotalDays;
            return $"{total} days";
        }

        public static string GetLogin(string username, string email)
        {
            if (username != "") return username;
            return email;
        }

        public static List<PasswordItem> GetPasswordItems(List<PasswordInfo> passwords, bool isCheck)
        {
            var result = new List<PasswordItem>();
            foreach (var password in passwords)
            {
                var newItem = new PasswordItem();
                newItem.id = password.ID;
                newItem.website = password.Website;
                newItem.login = GetLogin(password.Username, password.Email);
                if (isCheck) newItem.Password = GetPasswordFreshness(password.UpdateDate);
                else newItem.Password = "******";
                result.Add(newItem);
            }
            return result;
        }
    }
}
