using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestManagementServices.Models;

namespace TestManagementServices.Service
{

    public interface IHangfireService
    {
        void EvaluateRankAllTestNotSubmit();
    }

    public class HangfireService : IHangfireService
    {
        public void EvaluateRankAllTestNotSubmit()
        {
            try
            {
                using (DeverateContext context = new DeverateContext())
                {
                    var now = DateTime.Now;
                    var tests = context.Test
                        .Include(c => c.Config)
                        //Change Status = Doing
                        .Where(t => !t.Status && DateTime.Compare(now, t.StartTime.Value.AddMinutes(t.Config.Duration.Value)) > 0 && t.IsActive).ToList();
                    foreach(Test test in tests)
                    {
                        TestInfoDTO testInfo = new TestInfoDTO
                        {
                            testId = test.TestId
                        };
                        UserTest userTest = SystemDAO.GetQuestionInTest(testInfo, false);
                        SystemDAO.EvaluateRank(userTest);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error hangfire: " + ex);
            }
        }
    }
}
