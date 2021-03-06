﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestManagementServices.Models;

namespace TestManagementServices.Service
{

    public interface IHangfireService
    {
        void EvaluateRankAllTestNotSubmit();
        void ExpireTest();
    }

    public class HangfireService : IHangfireService
    {
        public void EvaluateRankAllTestNotSubmit()
        {
            try
            {
                using (DeverateContext context = new DeverateContext())
                {
                    var now = DateTime.UtcNow;
                    var testIds = context.Test
                        .Include(c => c.Semester)
                        .Where(t => t.IsActive && t.StartTime != null && t.Status == "Doing" && DateTime.Compare(now, t.StartTime.Value.AddMinutes(t.Semester.Duration)) > 0)
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
                Console.WriteLine("Error hangfire Not Submit: " + ex);
            }
        }

        public void ExpireTest()
        {
            try
            {
                using (DeverateContext context = new DeverateContext())
                {
                    var now = DateTime.UtcNow;
                    var tests = context.Test
                        .Include(c => c.Semester)
                        .Where(t => t.Status == "Pending"
                        && (DateTime.Compare(now, t.EndDate) > 0))
                        .ToList();
                    tests.ForEach(t => t.Status = "Expired");
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error hangfire Expire Test: " + ex);
            }
        }
    }
}
