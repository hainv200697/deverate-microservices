using AuthenServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;
using TestManagementServices.Service;

namespace TestManagementServices.Model
{
    [JsonObject("CatalogueDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CatalogueDTO
    {
        public CatalogueDTO()
        {

        }

        public CatalogueDTO(Catalogue catalogue)
        {
            this.catalogueId = catalogue.CatalogueId;
            this.name = catalogue.Name;
            this.isActive = catalogue.IsActive;
        }

        public CatalogueDTO(int catalogueId, string name, int numberOfQuestion, double weightPoint,
            List<QuestionDTO> questions, bool isActive)
        {
            this.catalogueId = catalogueId;
            this.name = name;
            this.numberOfQuestion = numberOfQuestion;
            this.weightPoint = weightPoint;
            this.questions = questions;
            this.isActive = isActive;
        }

        public CatalogueDTO(int catalogueId, string name, int numberOfQuestion, double weightPoint,
            List<Question> questionList)
        {
            this.catalogueId = catalogueId;
            this.name = name;
            this.numberOfQuestion = numberOfQuestion;
            this.weightPoint = weightPoint;
            for(int i = 0; i < questionList.Count; i++)
            {
                questionList[i].Answer = SystemDAO.GetAnswers(questionList[i].QuestionId);
            }
            this.questionList = questionList;
        }

        public CatalogueDTO(int catalogueId, string name, double? overallPoint, double? thresholdPoint)
        {
            this.catalogueId = catalogueId;
            this.name = name;
            this.overallPoint = overallPoint;
            this.thresholdPoint = thresholdPoint;
            this.differentPoint = differentPoint;
        }
        public CatalogueDTO(int catalogueId, string name, double value)
        {
            this.catalogueId = catalogueId;
            this.name = name;
            this.value = value;
        }

        public CatalogueDTO(int catalogueId, string name, double? overallPoint, int identify = 1)
        {
            this.catalogueId = catalogueId;
            this.name = name;
            this.overallPoint = overallPoint;
        }
        public CatalogueDTO(int configId)
        {
            this.configId = configId;
        }
        public int catalogueId { get; set; }
        public int companyId { get; set; }
        public int configId { get; set; }
        public string name { get; set; }
        public int numberOfQuestion { get; set; }
        public double? overallPoint { get; set; }
        public double? weightPoint { get; set; }
        public double? thresholdPoint { get; set; }
        /// <summary>
        /// điểm trung bình
        /// </summary>
        public double value { get; set; }
        public List<QuestionDTO> questions { get; set; }
        public List<Question> questionList { get; set; }
        public double differentPoint { get; set; }
        public bool isActive { get; set; }
    }
}
