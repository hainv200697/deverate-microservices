using ResourceServices.Model;
using ResourceServices.Models;
using System.Collections.Generic;
using System.Linq;


namespace AuthenServices.Service
{
    public class ApplicantDAO
    {



        public static List<ApplicantDTO> createApplicant(List<ApplicantDTO> apps)
        {
            using (DeverateContext context = new DeverateContext())

            {
                List<ApplicantDTO> listApplicant = new List<ApplicantDTO>();
                foreach (var item in apps) { 
                    Applicant apl = new Applicant();
                    apl.Fullname = item.fullname;
                    apl.Email = item.email;
                    apl.IsActive = true;
                    context.Applicant.Add(apl);
                    context.SaveChanges();
                    item.applicantId = apl.ApplicantId;
                    listApplicant.Add(item);
                }
                
                return listApplicant;
            }

        }


        public static int getConfigApplicant()
        {
            using (DeverateContext context = new DeverateContext())
            {
                int configId = context.Semester.Where(c => c.IsActive == true && c.Type == false).Select(x => x.SemesterId).FirstOrDefault();
                return configId;
            }

        }

    }
}
