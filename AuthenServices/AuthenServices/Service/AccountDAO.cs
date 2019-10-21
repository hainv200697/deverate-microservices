using AuthenServices.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace AuthenServices.Service
{
    public class AccountDAO
    {


        public static string CheckLogin(DeverateContext context, string username, string password)
        {
            username = username.ToUpper();
            Account account = context.Account.Include(a => a.Role).Where(a => a.Username == username && a.IsActive == true).SingleOrDefault();
            if (account == null)
            {
                return null;
            }
            bool? result = false;
            try
            {
                result = BCrypt.Net.BCrypt.Verify(password, account.Password);
            }
            catch (Exception e)
            {
                return null;
            }

            if (result.Value == false)
            {
                return null;
            }
            else
            {
                return TokenManager.GenerateToken(account);
            }
        }

        public static string GenerateCompanyAccount(DeverateContext context, MessageAccount ms)
        {
            Account account = new Account();
            var items = ms.Fullname.Split(' ');
            string username = items[items.Length - 1];
            for (int i = 0; i < items.Length - 1; i++)
            {
                username += items[i].ElementAt(0);
            }
            List<Account> accounts = context.Account.ToList();
            username = username.ToUpper() + (accounts[accounts.Count - 1].AccountId + 1);
            username = RemoveVietnameseTone(username);
            bool includeLowercase = true;
            bool includeUppercase = true;
            bool includeNumeric = true;
            bool includeSpecial = true;
            bool includeSpaces = false;
            int lengthOfPassword = 16;

            string password = PasswordGenerator.GeneratePassword(includeLowercase, includeUppercase, includeNumeric, includeSpecial, includeSpaces, lengthOfPassword);

            while (!PasswordGenerator.PasswordIsValid(includeLowercase, includeUppercase, includeNumeric, includeSpecial, includeSpaces, password))
            {
                password = PasswordGenerator.GeneratePassword(includeLowercase, includeUppercase, includeNumeric, includeSpecial, includeSpaces, lengthOfPassword);
            }
            string salt = BCrypt.Net.BCrypt.GenerateSalt(13);
            string encodedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            account.Username = username.ToUpper();
            account.Password = encodedPassword;
            account.Fullname = ms.Fullname;
            account.Gender = false;
            account.JoinDate = DateTime.Now;
            account.RoleId = ms.Role;
            account.CompanyId = ms.CompanyId;
            account.IsActive = true;
            context.Account.Add(account);
            context.SaveChanges();
            return username.ToUpper() + "_" + password;


        }


        public static string RemoveVietnameseTone(string text)
        {
            string result = text.ToLower();
            result = Regex.Replace(result, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|/g", "a");
            result = Regex.Replace(result, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|/g", "e");
            result = Regex.Replace(result, "ì|í|ị|ỉ|ĩ|/g", "i");
            result = Regex.Replace(result, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|/g", "o");
            result = Regex.Replace(result, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|/g", "u");
            result = Regex.Replace(result, "ỳ|ý|ỵ|ỷ|ỹ|/g", "y");
            result = Regex.Replace(result, "đ", "d");
            return result;
        }
    }
}