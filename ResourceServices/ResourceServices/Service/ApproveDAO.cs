using ResourceServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using ResourceServices.Models;
using Microsoft.EntityFrameworkCore;
using AuthenServices.Model;

namespace ResourceServices.Service
{
    public class ApproveDAO
    {

        public static List<ApproveRankDTO> GetApproveRequest(int configId)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var newConfig = context.Configuration.OrderByDescending(c => c.CreateDate).FirstOrDefault();
                var approve = context.Test.Include(x => x.Rank).Include(x => x.Account).ThenInclude(x => x.Rank)
                    .Where(x => x.ConfigId == configId && x.IsApprove == null && x.FinishTime != null)
                    .Select(x => new ApproveRankDTO
                    {
                        accountId = x.AccountId,
                        fullname = x.Account.Fullname,
                        username = x.Account.Username,
                        accountRankId = x.Account.RankId,
                        accountRankName = x.Account.Rank.Name,
                        rankInTestId = x.RankId,
                        rankInTestName = x.Rank.Name,
                    }).ToList();
                if (newConfig.ConfigId != configId)
                {
                    var newApprove = context.Test.Include(x => x.Rank).Include(x => x.Account).ThenInclude(x => x.Rank)
                    .Where(x => x.ConfigId == newConfig.ConfigId && x.IsApprove == null && x.FinishTime != null)
                    .Select(x => new ApproveRankDTO
                    {
                        accountId = x.AccountId,
                        fullname = x.Account.Fullname,
                        username = x.Account.Username,
                        accountRankId = x.Account.RankId,
                        accountRankName = x.Account.Rank.Name,
                        rankInTestId = x.RankId,
                        rankInTestName = x.Rank.Name,
                    }).ToList();
                    foreach(var newItem in newApprove)
                    {
                        foreach (var item in approve.ToList())
                        {
                            if(item.accountId == newItem.accountId)
                            {
                                approve.Remove(item);
                            }
                        }
                    }
                }
                return approve;
            }
        }

        public static void ActionRequest(int configId,int accountId, bool isApprove)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Test test = context.Test.Include(x=>x.Account).SingleOrDefault(x => x.ConfigId == configId && x.AccountId == accountId);
                test.IsApprove = isApprove;
                if (isApprove)
                {
                    test.Account.RankId = test.RankId;
                }
                context.Test.Update(test);
                context.SaveChanges();
            }
        }

    }
}
