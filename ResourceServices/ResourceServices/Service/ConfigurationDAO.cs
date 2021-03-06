﻿using AuthenServices.Model;
using Microsoft.EntityFrameworkCore;
using ResourceServices.Model;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Service
{
    public class ConfigurationDAO
    {
        public static List<ConfigurationDTO> GetAllConfigurationForApplicant(bool type, int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Semester.Include(a => a.Company).Where(c => c.CompanyId == companyId && c.Type == type).Select(c => new ConfigurationDTO(c)).OrderByDescending(x => x.configId).ToList();
            }
        }

        public static List<ConfigurationDTO> GetAllConfiguration(bool type, int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Semester.Include(a => a.Company).Where(c => c.CompanyId == companyId && c.Type == type).Select(c => new ConfigurationDTO(c)).OrderByDescending(x => x.configId).ToList();
            }
        }

        public static List<ConfigurationDTO> GetConfigurationForEmployee(int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                return db.Semester.Where(x => x.CompanyId == companyId && x.IsActive == true && x.Type == true).Select(x => new ConfigurationDTO(x)).ToList();
            }
        }

        public static void CreateConfiguration(ConfigurationDTO configurationDTO)
        {
            using (DeverateContext db = new DeverateContext())
            {

                Semester configuration = new Semester();

                var newLstCatalougeInConfig = new List<CatalogueInSemester>();
                var newLstRankInConfig = new List<RankInSemester>();

                foreach (var item in configurationDTO.catalogueInConfigurations)
                {
                    var catalougeInConfig = new CatalogueInSemester
                    {
                        CatalogueId = item.catalogueId,
                        WeightPoint = item.weightPoint,
                        NumberQuestion = item.numberQuestion,
                        IsActive = true
                    };
                    newLstCatalougeInConfig.Add(catalougeInConfig);
                }

                foreach (var item in configurationDTO.rankInConfigs)
                {
                    var rankInConfig = new RankInSemester
                    {
                        RankId = item.rankId,
                        Point = item.point,
                        IsActive = true
                    };
                    newLstRankInConfig.Add(rankInConfig);
                }

                configuration.CompanyId = configurationDTO.companyId;
                configuration.Title = configurationDTO.title;
                configuration.CreateDate = DateTime.UtcNow;
                configuration.ExpiredDays = configurationDTO.expiredDays;
                configuration.Duration = configurationDTO.duration;
                configuration.Type = configurationDTO.type;
                configuration.IsActive = true;
                configuration.CatalogueInSemester = newLstCatalougeInConfig;
                configuration.RankInSemester = newLstRankInConfig;
                db.Semester.Add(configuration);
                db.SaveChanges();
            }
        }

        public static void CloneConfiguration(int configId, string title)
        {
            using (DeverateContext db = new DeverateContext())
            {

                Semester semester = db.Semester
                    .Include(c => c.CatalogueInSemester)
                    .Include(r => r.RankInSemester)
                    .Where(x => x.SemesterId == configId).FirstOrDefault();
                var cloneCatalougeInConfig = new List<CatalogueInSemester>();
                foreach (var item in semester.CatalogueInSemester)
                {
                    var catalougeInConfig = new CatalogueInSemester
                    {
                        CatalogueId = item.CatalogueId,
                        WeightPoint = item.WeightPoint,
                        NumberQuestion = item.NumberQuestion,
                        IsActive = true
                    };
                    cloneCatalougeInConfig.Add(catalougeInConfig);
                }

                var cloneRankInConfig = new List<RankInSemester>();
                foreach (var item in semester.RankInSemester)
                {
                    var rankInConfig = new RankInSemester
                    {
                        RankId = item.RankId,
                        Point = item.Point,
                        IsActive = true
                    };
                    cloneRankInConfig.Add(rankInConfig);
                }
                Semester cloneSemester = new Semester();
                cloneSemester.CompanyId = semester.CompanyId;
                cloneSemester.Title = title;
                cloneSemester.CreateDate = DateTime.UtcNow;
                cloneSemester.ExpiredDays = semester.ExpiredDays;
                cloneSemester.Duration = semester.Duration;
                cloneSemester.Type = semester.Type;
                cloneSemester.IsActive = true;
                cloneSemester.CatalogueInSemester = cloneCatalougeInConfig;
                cloneSemester.RankInSemester = cloneRankInConfig;
                db.Semester.Add(cloneSemester);
                db.SaveChanges();
            }
        }

        public static ConfigurationDTO GetConfigurationById(int configId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var config = db.Semester
                    .Include(c => c.CatalogueInSemester)
                    .ThenInclude(c => c.Catalogue)
                    .Include(c => c.RankInSemester)
                    .ThenInclude(c => c.Rank)
                    .Where(c => c.SemesterId == configId)
                    .Select(c => new ConfigurationDTO(c));
                return config.FirstOrDefault();
            }
        }

        public static void ChangeStatusConfiguration(List<int> configIds, bool isActive)
        {
            using (DeverateContext db = new DeverateContext())
            {
                db.Semester
                    .Where(x => configIds.Contains(x.SemesterId))
                    .ToList()
                    .ForEach(x => x.IsActive = isActive);
                db.SaveChanges();
            }
        }

    }
}
