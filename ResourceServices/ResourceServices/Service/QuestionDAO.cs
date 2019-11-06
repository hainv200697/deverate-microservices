﻿using ResourceServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenServices.Models;
using ResourceServices.Models;
using Microsoft.EntityFrameworkCore;

namespace ResourceServices.Service
{
    public class QuestionDAO
    {
        public static List<QuestionDTO> GetAllQuestion()
        {
            using (DeverateContext context = new DeverateContext())
            {
                var question = from ques in context.Question
                               where ques.IsActive == true
                                select new QuestionDTO(ques ,ques.Answer.ToList());
                return question.ToList();
            }

        }

        public static List<QuestionDTO> GetQuestionByStatus(bool status,int id)
        {
            using (DeverateContext context = new DeverateContext())
            {
                var question = from ques in context.Question
                               where ques.IsActive == status && ques.Cicid == id
                               select new QuestionDTO(ques, ques.Answer.ToList());
                return question.ToList();
            }

        }

        public static QuestionDTO GetQuestionByCatalogue(int catalogueId, int companyId, bool status)
        {
            using (DeverateContext context = new DeverateContext())

            {
                var companyCata = context.CatalogueInCompany.Include(x=>x.Catalogue).Include(x=>x.Question)
                    .Where(x => x.CompanyId == companyId && x.CatalogueId == catalogueId)
                    .Select(x => new QuestionDTO(x.Question.Where(q=>q.IsActive==status).ToList(), x.Catalogue.Name, x.Cicid))
                    .SingleOrDefault();
                return companyCata;
            }

        }

        public static string CreateQuestion(List<QuestionDTO> quest)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ques in quest)
                {
                    Question question = new Question();
                    question.Cicid = ques.cicid;
                    question.Question1 = ques.question1;
                    question.IsActive = true;
                    question.MaxPoint = ques.maxPoint;
                    question.CreateBy = ques.createBy;
                    question.Answer = ques.answer;
                    question.CreateBy = ques.createBy;
                    question.Answer = ques.answer;
                    context.Question.Add(question);
                    context.SaveChanges();
                }
                return Message.createQuestionSucceed;
            }

        }
        

        public static string UpdateQuestion(QuestionDTO ques)
        {
            try
            {
                using (DeverateContext context = new DeverateContext())
                {
                    Question question = context.Question.SingleOrDefault(x=>x.QuestionId == ques.questionId);
                    question.Question1 = ques.question1;
                    context.Question.Update(question);
                    context.SaveChanges();
                    return Message.updateQuestionSucceed; 
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "{\"message\" : \"Update Question fail\"}";
            }
            

        }

        public static string removeQuestion(List<QuestionDTO> Question)
        {
            using (DeverateContext context = new DeverateContext())
            {
                foreach (var ques in Question)
                {
                    Question questionDb = context.Question.SingleOrDefault(c => c.QuestionId == ques.questionId);
                    questionDb.IsActive = ques.isActive;
                    if(ques.isActive == false) { 
                        List<AnswerDTO> answers = context.Answer.Where(answer => answer.QuestionId == questionDb.QuestionId).Select(answer => new AnswerDTO(answer)).ToList();
                        foreach(var item in answers)
                        {
                            Answer AnswerDb = context.Answer.SingleOrDefault(c => c.AnswerId == item.answerId);
                            AnswerDb.IsActive = false;
                            context.SaveChanges();
                        }
                    }
                    context.SaveChanges();
                }
                return Message.removeQuestionSucceed;
            }
        }
    }
}
