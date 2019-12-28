using AuthenServices.Model;
using AuthenServices.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace AuthenServices.Service
{
    public class AccountDAO
    {
        public static string CheckLogin(string username, string password)
        {
            using(DeverateContext context = new DeverateContext())
            {
                username = username.ToUpper();
                Account account = context.Account.Include(a => a.Role).Include(a => a.Company).Where(a => a.Username == username && a.IsActive == true).SingleOrDefault();
                if (account == null)
                {
                    return null;
                }
                if (account.RoleId != 1 && account.Company.IsActive == false)
                {
                    return null;
                }
                bool result = false;
                try
                {
                    result = BCrypt.Net.BCrypt.Verify(password, account.Password);
                }
                catch (Exception)
                {
                    return null;
                }

                if (!result)
                {
                    return null;
                }
                return TokenManager.GenerateToken(account);
            }
        }

        public static MessageAccountDTO GenerateCompanyAccount(MessageAccount ms)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var items = ms.Fullname.Split(' ');
                string username = items[items.Length - 1];
                for (int i = 0; i < items.Length - 1; i++)
                {
                    username += items[i].ElementAt(0);
                }
                if (AppConstrain.newestAccount == null)
                {
                    List<Account> accounts = context.Account.ToList();
                    AppConstrain.newestAccount = accounts.Count + 1;
                }
                else
                {
                    AppConstrain.newestAccount++;
                }

                username = username.ToUpper() + AppConstrain.newestAccount;
                username = RemoveVietnameseTone(username);

                string password = "";
                Account account = new Account {
                    Username = username.ToUpper(),
                    Password = GeneratePasswordHash(out password),
                    Fullname = ms.Fullname,
                    Email = ms.Email,
                    Gender = ms.Gender,
                    Address = ms.Address,
                    Phone = ms.Phone,
                    JoinDate = DateTime.UtcNow,
                    RoleId = ms.Role,
                    CompanyId = ms.CompanyId,
                    IsActive = true
                };
                context.Account.Add(account);
                context.SaveChanges();
                return new MessageAccountDTO(account.Username, password, ms.Email, ms.Fullname);
            }
        }

        public static List<MessageAccountDTO> GenerateCompanyAccount(List<MessageAccount> listAccountGenerate)
        {
            using (DeverateContext context = new DeverateContext())
            {
                List<MessageAccountDTO> result = new List<MessageAccountDTO>();
                List<Account> accountsSave = new List<Account>();
                foreach (MessageAccount ms in listAccountGenerate)
                {
                    var items = ms.Fullname.Split(' ');
                    string username = items[items.Length - 1];
                    for (int i = 0; i < items.Length - 1; i++)
                    {
                        username += items[i].ElementAt(0);
                    }
                    if (AppConstrain.newestAccount == null)
                    {
                        List<Account> accounts = context.Account.ToList();
                        AppConstrain.newestAccount = accounts.Count + 1;
                    }
                    else
                    {
                        AppConstrain.newestAccount++;
                    }

                    username = username.ToUpper() + AppConstrain.newestAccount;
                    username = RemoveVietnameseTone(username);

                    string password = "";
                    Account account = new Account
                    {
                        Username = username.ToUpper(),
                        Fullname = ms.Fullname,
                        Email = ms.Email,
                        Gender = ms.Gender,
                        Address = ms.Address,
                        Phone = ms.Phone,
                        JoinDate = DateTime.UtcNow,
                        RoleId = ms.Role,
                        CompanyId = ms.CompanyId,
                        IsActive = true,
                    };
                    account.Password = GeneratePasswordHash(out password);

                    accountsSave.Add(account);
                    result.Add(new MessageAccountDTO(account.Username, password, ms.Email, ms.Fullname));
                }
                context.Account.AddRange(accountsSave);
                context.SaveChanges();
                return result;
            }
        }

        private static string GeneratePasswordHash(out string password)
        {
            bool includeLowercase = true;
            bool includeUppercase = true;
            bool includeNumeric = true;
            bool includeSpecial = true;
            bool includeSpaces = false;
            int lengthOfPassword = 16;

            password = PasswordGenerator.GeneratePassword(includeLowercase, includeUppercase, includeNumeric, includeSpecial, includeSpaces, lengthOfPassword);
            while (!PasswordGenerator.PasswordIsValid(includeLowercase, includeUppercase, includeNumeric, includeSpecial, includeSpaces, password))
            {
                password = PasswordGenerator.GeneratePassword(includeLowercase, includeUppercase, includeNumeric, includeSpecial, includeSpaces, lengthOfPassword);
            }
            string salt = BCrypt.Net.BCrypt.GenerateSalt(13);
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }


        public static List<MessageAccountDTO> Resend(List<String> listUsername)
        {
            using (DeverateContext db = new DeverateContext())
            {

                List<MessageAccountDTO> result = new List<MessageAccountDTO>();
                List<Account> accounts = db.Account.Where(a => listUsername.Contains(a.Username)).ToList();
                foreach (Account account in accounts)
                {
                    string password = "";
                    account.Password = GeneratePasswordHash(out password);
                    result.Add(new MessageAccountDTO(account.Username, password, account.Email, account.Fullname));
                }
                db.SaveChanges();
                return result;
            }
        }

        public static bool ChangePassword(ChangePassRequest changePassRequest)
        {
            using (DeverateContext db = new DeverateContext())
            {
                changePassRequest.username = changePassRequest.username.ToUpper();
                Account account = db.Account.SingleOrDefault(a => a.Username == changePassRequest.username);
                bool check = BCrypt.Net.BCrypt.Verify(changePassRequest.oldPassword, account.Password);
                if (!check)
                {
                    return check;
                }
                string salt = BCrypt.Net.BCrypt.GenerateSalt(13);
                account.Password = BCrypt.Net.BCrypt.HashPassword(changePassRequest.newPassword, salt);
                db.SaveChanges();
                return true;
            }
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

        public static List<string> CheckExistedEmail(List<string> listemail, int? companyId)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var check = context.Account.Where(x => listemail.Contains(x.Email) && x.CompanyId == companyId).Select(x => x.Email).ToList();
                return check;
            }

        }
    }
}