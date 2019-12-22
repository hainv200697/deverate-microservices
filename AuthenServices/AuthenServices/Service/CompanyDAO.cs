using System;
using System.Collections.Generic;
using System.Linq;
using AuthenServices.Model;
using AuthenServices.Models;

namespace AuthenServices.Service
{
    public class CompanyDAO
    {
        public static bool checkExistedCompany(string companyName)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var check = db.Company.Where(x => companyName == x.Name);
                return check.Any();
            }

        }

        public static Company CreateCompany(CompanyDataDTO companyData)
        {
            using (DeverateContext db = new DeverateContext())
            {
                Company com = new Company {
                    Address = companyData.CompanyDTO.address,
                    Name = companyData.CompanyDTO.name,
                    CreateDate = DateTime.UtcNow,
                    Phone = companyData.CompanyDTO.phone,
                    IsActive = companyData.CompanyDTO.isActive,
                  };
                var result = db.Company.Add(com);
                var defaultCatalogues = db.DefaultCatalogue.Where(x => x.IsActive).ToList();
                var defaultQuestions = db.DefaultQuestion.Where(x =>  x.IsActive).ToList();
                var defaultAnswers = db.DefaultAnswer.Where(x => x.IsActive).ToList();
                var defaultRanks = db.DefaultRank.Where(x => x.IsActive).ToList();

                // Clone Catalogue
                List<CompanyCatalogue> companyCatalogues = new List<CompanyCatalogue>();
                foreach (DefaultCatalogue defaultCatalogue in defaultCatalogues)
                {
                    var companyCatalogue = new CompanyCatalogue
                    {
                        Company = com,
                        Name = defaultCatalogue.Name,
                        Description = defaultCatalogue.Description,
                        CreateDate = DateTime.UtcNow,
                        IsActive = true
                    };

                    // Clone Question
                    List<Question> questions = new List<Question>();
                    var defaultQuestionsOfCatalogue = defaultQuestions.Where(x => x.DefaultCatalogueId == defaultCatalogue.DefaultCatalogueId).ToList();
                    foreach (DefaultQuestion defaultQuestion in defaultQuestionsOfCatalogue)
                    {
                        var question = new Question
                        {
                            Question1 = defaultQuestion.Question,
                            Point = defaultQuestion.Point,
                            IsActive = true,
                            CreateAt = DateTime.UtcNow
                        };
                        var defaultAnswersOfQuestion = defaultAnswers.Where(x => x.DefaultQuestionId == defaultQuestion.DefaultQuestionId).ToList();
                        // Clone Answer
                        List<Answer> answers = new List<Answer>();
                        foreach(DefaultAnswer defaultAnswer in defaultAnswersOfQuestion)
                        {
                            answers.Add(new Answer
                            {
                                Answer1 = defaultAnswer.Answer,
                                Percent = defaultAnswer.Percent,
                                IsActive = true
                            });
                        }
                        question.Answer = answers;
                        questions.Add(question);
                    }
                    companyCatalogue.Question = questions;
                    companyCatalogues.Add(companyCatalogue);
                }
                com.CompanyCatalogue = companyCatalogues;

                // Clone Rank
                var companyRanks = new List<CompanyRank>();
                foreach(var defaultRank in defaultRanks)
                {
                    companyRanks.Add(new CompanyRank
                    {
                        Name = defaultRank.Name,
                        CreateDate = DateTime.UtcNow,
                        IsActive = true
                    });
                }
                com.CompanyRank = companyRanks;
                db.SaveChanges();
                return result.Entity;
            }
        }
    }
}
