using AuthenServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestManagementServices.Models;

namespace TestManagementServices.Model
{
    [JsonObject("CompanyCatalogueDTO", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CompanyCatalogueDTO
    {
        public CompanyCatalogueDTO()
        {

        }

        public CompanyCatalogueDTO(CompanyCatalogue catalogue)
        {
            this.companyCatalogueId = catalogue.CompanyCatalogueId;
            this.name = catalogue.Name;
            this.isActive = catalogue.IsActive;
        }

        public CompanyCatalogueDTO(int? companyCatalogueId, string name, int? numberOfQuestion, double? weightPoint,
            List<QuestionDTO> questions, bool? isActive)
        {
            this.companyCatalogueId = companyCatalogueId;
            this.name = name;
            this.numberOfQuestion = numberOfQuestion;
            this.weightPoint = weightPoint;
            this.questions = questions;
            this.isActive = isActive;
        }

        public CompanyCatalogueDTO(int? companyCatalogueId, string name, int? numberOfQuestion, double? weightPoint,
            List<Question> questionList)
        {
            this.companyCatalogueId = companyCatalogueId;
            this.name = name;
            this.numberOfQuestion = numberOfQuestion;
            this.weightPoint = weightPoint;
            this.questionList = questionList;
        }

        public CompanyCatalogueDTO(int? companyCatalogueId, string name, double? overallPoint, double? thresholdPoint)
        {
            this.companyCatalogueId = companyCatalogueId;
            this.name = name;
            this.overallPoint = overallPoint;
            this.thresholdPoint = thresholdPoint;
        }
        public CompanyCatalogueDTO(int? companyCatalogueId, string name, double? value)
        {
            this.companyCatalogueId = companyCatalogueId;
            this.name = name;
            this.value = value;
        }

        public CompanyCatalogueDTO(int? companyCatalogueId, string name, double? overallPoint, int identify = 1)
        {
            this.companyCatalogueId = companyCatalogueId;
            this.name = name;
            this.overallPoint = overallPoint;
        }
        public CompanyCatalogueDTO(int? configId)
        {
            this.configId = configId;
        }
        public int? companyCatalogueId { get; set; }
        public int? companyId { get; set; }
        public int? configId { get; set; }
        public string name { get; set; }
        public int? numberOfQuestion { get; set; }
        public double? overallPoint { get; set; }
        public double? weightPoint { get; set; }
        public double? thresholdPoint { get; set; }
        /// <summary>
        /// điểm trung bình
        /// </summary>
        public double? value { get; set; }
        public List<QuestionDTO> questions { get; set; }
        public List<Question> questionList { get; set; }
        public bool? isActive { get; set; }
    }
}
