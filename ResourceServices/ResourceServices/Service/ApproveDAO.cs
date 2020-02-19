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
                var approve = context.Test.Include(x => x.Rank).Include(x => x.Account).ThenInclude(x => x.Rank)
                    .Where(x => x.SemesterId == configId && x.IsApprove == null && x.FinishTime != null && x.RankId != null)
                    .Select(x => new ApproveRankDTO
                    {
                        testId = x.TestId,
                        accountId = x.AccountId.Value,
                        fullname = x.Account.Fullname,
                        username = x.Account.Username,
                        accountRankName = x.Account.Rank.Name,
                        rankInTestName = x.Rank.Name,
                    }).ToList();

                return approve;
            }
        }

        public static void ActionRequest(int testId, bool isApprove)
        {
            using (DeverateContext context = new DeverateContext())
            {
                Test test = context.Test.Include(x => x.Account).SingleOrDefault(x => x.TestId == testId);
                test.IsApprove = isApprove;
                if (isApprove)
                {
                    test.Account.RankId = test.RankId;
                }
                context.Test.Update(test);
                context.Test.Where(x => x.SemesterId < test.SemesterId && x.AccountId == test.AccountId && x.IsApprove == null).ToList().ForEach(x=> x.IsApprove = false);
                context.SaveChanges();
            }
        }

    }
}
