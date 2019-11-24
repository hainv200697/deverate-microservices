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
                    var testIds = context.Test
                        .Include(c => c.Config)
                        .Where(t => t.IsActive && t.StartTime != null && t.Status == "Doing" && DateTime.Compare(now, t.StartTime.Value.AddMinutes(t.Config.Duration)) > 0 && t.IsActive)
                        .Select(t => t.TestId)
                        .ToList();
                    foreach (int testId in testIds)
                    {
                        SystemDAO.AutoSubmit(testId);
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
