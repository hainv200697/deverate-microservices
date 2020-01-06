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
                var defaultCatalogues = db.Catalogue.Where(x => x.IsActive && x.IsDefault == true).ToList();
                var defaultQuestions = db.Question.Where(x =>  x.IsActive).ToList();
                var defaultAnswers = db.Answer.Where(x => x.IsActive).ToList();
                var defaultRanks = db.Rank.Where(x => x.IsActive).ToList();

                // Clone Catalogue
                List<Catalogue> companyCatalogues = new List<Catalogue>();
                foreach (Catalogue defaultCatalogue in defaultCatalogues)
                {
                    var companyCatalogue = new Catalogue
                    {
                        Company = com,
                        Name = defaultCatalogue.Name,
                        Description = defaultCatalogue.Description,
                        CreateDate = DateTime.UtcNow,
                        IsActive = true
                    };

                    // Clone Question
                    List<Question> questions = new List<Question>();
                    var defaultQuestionsOfCatalogue = defaultQuestions.Where(x => x.CatalogueId == defaultCatalogue.CatalogueId).ToList();
                    foreach (Question defaultQuestion in defaultQuestionsOfCatalogue)
                    {
                        var question = new Question
                        {
                            Question1 = defaultQuestion.Question1,
                            Point = defaultQuestion.Point,
                            IsActive = true,
                            CreateAt = DateTime.UtcNow
                        };
                        var defaultAnswersOfQuestion = defaultAnswers.Where(x => x.QuestionId == defaultQuestion.QuestionId).ToList();
                        // Clone Answer
                        List<Answer> answers = new List<Answer>();
                        foreach(Answer defaultAnswer in defaultAnswersOfQuestion)
                        {
                            answers.Add(new Answer
                            {
                                Answer1 = defaultAnswer.Answer1,
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
                com.Catalogue = companyCatalogues;

                // Clone Rank
                var companyRanks = new List<Rank>();
                foreach(var defaultRank in defaultRanks)
                {
                    companyRanks.Add(new Rank
                    {
                        Name = defaultRank.Name,
                        CreateDate = DateTime.UtcNow,
                        IsActive = true,
                        Position = defaultRank.Position
                    });
                }
                com.Rank = companyRanks;
                db.SaveChanges();
                return result.Entity;
            }
        }
    }
}
