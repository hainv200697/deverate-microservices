﻿using AuthenServices.Models;
using Microsoft.EntityFrameworkCore;
using ResourceServices.Model;
using ResourceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServices.Service
{
    public class RankDAO
    {
        DeverateContext context;

        public RankDAO(DeverateContext context)
        {
            this.context = context;
        }

        public static List<CompanyRankDTO> getAllCompanyRank(bool isActive, int companyId)
        {
            using (DeverateContext db = new DeverateContext())
            {
                var ranks = db.Rank.Where(c => c.IsActive == isActive && c.CompanyId == companyId).Select(c => new CompanyRankDTO(c));
                return ranks.ToList().OrderBy(x => x.position).ToList();
            }
        }
        public static ListRankAndListCatalogueDTO getAllDefaultRank()
        {
            using (DeverateContext db = new DeverateContext())
            {
                var ranks = db.Rank.Where(c => c.IsActive && c.IsDefault)
                    .Include(x => x.CatalogueInRank)
                    .ThenInclude(x => x.Catalogue)
                    .Select(c => new DefaultRankDTO(c))
                    .ToList();
                var catalogues = db.Catalogue
                    .Where(x => x.IsActive && x.IsDefault)
                    .Select(x => new CatalogueDefaultDTO(x))
                    .OrderBy(x => x.catalogueId)
                    .ToList();
                return new ListRankAndListCatalogueDTO
                {
                    catalogueDefaultDTOs = catalogues,
                    defaultRankDTOs = ranks                };
            }
        }

        public static void SaveDefaultRank(List<DefaultRankDTO> defaultRankDTOs)
        {
            using (DeverateContext db = new DeverateContext())
            {
                //Disable Rank has rankId
                var defaultRankIdDisable = defaultRankDTOs.Where(x => x.rankId > 0).Select(x => x.rankId).ToList();
                if (defaultRankIdDisable.Any()) {
                    var ids = defaultRankIdDisable;
                    db.Rank
                        .Where(x => ids.Contains(x.RankId))
                        .ToList()
                        .ForEach(r => r.IsActive = false);
                }
                List<Rank> defaultRanks = new List<Rank>();
                foreach(var defaultRank in defaultRankDTOs)
                {
                    List<CatalogueInRank> catalogueInRanks = new List<CatalogueInRank>();
                    foreach(var catalogueInRank in defaultRank.catalogueInRanks)
                    {
                        catalogueInRanks.Add(new CatalogueInRank
                        {
                            CatalogueId = catalogueInRank.catalogueId,
                            IsActive = true,
                            Point = catalogueInRank.point
                        });
                    }
                    defaultRanks.Add(new Rank
                    {
                        IsActive = true,
                        IsDefault = true,
                        CreateDate = DateTime.UtcNow,
                        CatalogueInRank = catalogueInRanks,
                        Name = defaultRank.name
                    });
                }
                db.AddRange(defaultRanks);
                db.SaveChanges();
            }
        }

        public static void DisableDefaultRank(List<int> ids)
        {
            using (DeverateContext db = new DeverateContext())
            {
                db.Rank
                    .Where(x => ids.Contains(x.RankId))
                    .ToList()
                    .ForEach(r => r.IsActive = false);
                db.SaveChanges();
            }
        }

        public static void changeStatusCompanyRank(List<int> rankId, bool status)
        {
            using (DeverateContext db = new DeverateContext())
            {
                db.Rank.Where(x => rankId.Contains(x.RankId)).ToList().ForEach(x => x.IsActive = status);
                db.SaveChanges();
            }
        }

        public static void UpdateRalationIfNot()
        {
            using (DeverateContext db = new DeverateContext())
            {
               
                var catalogues = db.Catalogue
                    .Where(c => c.IsDefault && c.IsActive &&
                        !db.CatalogueInRank
                            .Select(cr => cr.CatalogueId)
                            .Contains(c.CatalogueId)).ToList();
                if (catalogues.Any())
                {
                    List<CatalogueInRank> catalogueInRanks = new List<CatalogueInRank>();
                    var ranks = db.Rank.Where(r => r.IsActive == true && r.IsDefault);
                    foreach (var catalogue in catalogues)
                    {
                        foreach(var rank in ranks)
                        {
                            catalogueInRanks.Add(new CatalogueInRank
                            {
                                CatalogueId = catalogue.CatalogueId,
                                RankId = rank.RankId,
                                Point = 0,
                                IsActive = true
                            });
                        }
                    }
                    db.AddRange(catalogueInRanks);
                    db.SaveChanges();
                }
            }
        }

    }
}
