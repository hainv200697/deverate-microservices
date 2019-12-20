using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ResourceServices.Model;
using ResourceServices.Models;
using ResourceServices.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Service
{
    public class SemesterDAO
    {
        DeverateContext context;

        public SemesterDAO(DeverateContext context)
        {
            this.context = context;
        }


        public static List<SemesterDTO> getAllEmployeeInCompany(int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var listEmployee = db.Account.Include(x => x.CompanyRank).Where(x => x.CompanyId == companyId && x.RoleId == 3).Select(x => new SemesterDTO(x)).ToList();
                return listEmployee;
            }
        }
    }
}
