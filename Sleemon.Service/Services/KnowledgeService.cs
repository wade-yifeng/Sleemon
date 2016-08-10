using System;

namespace Sleemon.Service
{
    using System.Linq;
    using System.Data.Entity;
    using System.Collections.Generic;
    
    using Sleemon.Core;
    using Sleemon.Data;
    using Sleemon.Common;

    public class KnowledgeService : IKnowledgeService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public KnowledgeService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public IList<KnowledgeBaseModel> SearchKBList(string input)
        {
            return this._invoicingEntities.KnowledgeBase
                .Where(
                    p =>
                        p.IsActive &&
                        p.KnowledgeBaseKeyword.Any(
                            o => o.IsActive && o.Keyword.IsActive && o.Keyword.Word.Contains(input)))
                .Select(p => new KnowledgeBaseModel()
                {
                    KnowledgeBaseId = p.Id,
                    Title = p.Title
                })
                .ToList();
        }

        public IList<KnowledgeDetailModel> GetKBListByCategory(int knowledgeCategory)
        {
            return this._invoicingEntities.KnowledgeBase
                .Include("KnowledgeBaseKeyword.Keyword")
                .Where(p => p.IsActive && p.KnowledgeCategory == knowledgeCategory)
                .Select(p => new KnowledgeDetailModel()
                {
                    KnowledgeBaseId = p.Id,
                    Title = p.Title,
                    Detail = p.Detail,
                    Keywords = p.KnowledgeBaseKeyword
                                .Where(o => o.IsActive && o.Keyword.IsActive)
                                .Select(o => o.Keyword.Word)
                                .ToList()
                })
                .ToList();
        }

        public IList<KnowledgeDetailModel> GetKBLisetByKeyword(string keyword)
        {
            var keywordEntities = this._invoicingEntities.Keyword
                .Include("KnowledgeBaseKeyword.KnowledgeBase")
                .Where(p => p.IsActive && p.Word.Contains(keyword))
                .ToList();

            var results = new List<KnowledgeDetailModel>();

            foreach (var keywordEntity in keywordEntities)
            {
                foreach (var knowledgeBaseKeyword in keywordEntity.KnowledgeBaseKeyword)
                {
                    if (knowledgeBaseKeyword.IsActive && knowledgeBaseKeyword.KnowledgeBase.IsActive)
                    {
                        results.Add(new KnowledgeDetailModel()
                        {
                            KnowledgeBaseId = knowledgeBaseKeyword.KnowledgeBase.Id,
                            Title = knowledgeBaseKeyword.KnowledgeBase.Title,
                            Detail = knowledgeBaseKeyword.KnowledgeBase.Detail,
                            Keywords = keywordEntities.Select(p => p.Word).ToList()
                        });
                    }
                }
            }

            return results.Distinct(new KnowledgeDetailComparer()).ToList();
        }

        public KnowledgeDetailModel GetKBDetail(int knowledgeBaseId)
        {
            var knowledgeEntity =
                this._invoicingEntities.KnowledgeBase
                .Include("KnowledgeBaseKeyword.Keyword")
                .FirstOrDefault(p => p.IsActive && p.Id == knowledgeBaseId);

            if (knowledgeEntity == null) return null;

            return new KnowledgeDetailModel()
            {
                Title = knowledgeEntity.Title,
                Detail = knowledgeEntity.Detail,
                LastUpdateTime = knowledgeEntity.LastUpdateTime,
                Keywords = knowledgeEntity.KnowledgeBaseKeyword
                                            .Where(p=> p.IsActive && p.Keyword.IsActive)
                                            .Select(p=> p.Keyword.Word)
                                            .ToList()
            };
        }

        public IList<KnowledgeBaseDetailModel> GetKnowledgeBaseList()
        {
            var kbEntities =
                this._invoicingEntities.KnowledgeBase.Where(p => p.IsActive).ToList();
            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();

            return kbEntities
                .Select(p =>
                {
                    var lastUpdateUserName = string.Empty;
                    var firstOrDefault = userEntities.FirstOrDefault(o => o.UserUniqueId == p.LastUpdateUser);
                    if (firstOrDefault != null)
                    {
                        lastUpdateUserName = firstOrDefault.Name;
                    }

                    return new KnowledgeBaseDetailModel()
                    {
                        KnowledgeBaseId = p.Id,
                        Title = p.Title,
                        Detail = p.Detail,
                        KnowledgeCategory = (byte) p.KnowledgeCategory,
                        LastUpdateTime = p.LastUpdateTime,
                        LastUpdateUser = p.LastUpdateUser,
                        LastUpdateUserName = lastUpdateUserName
                    };
                })
                .ToList();
        }

        public KnowledgeBaseDetailModel GetKnowledgeBaseDetail(int knowledgeBaseId)
        {
            var kbEntity =
                this._invoicingEntities.KnowledgeBase.Include(p => p.KnowledgeBaseKeyword)
                    .FirstOrDefault(p => p.IsActive && p.Id == knowledgeBaseId);

            if (kbEntity == null) return null;

            var lastUpdateUser =
                this._invoicingEntities.User.FirstOrDefault(p => p.IsActive && p.UserUniqueId == kbEntity.LastUpdateUser);

            var kbKeyworkEntities =
                this._invoicingEntities.KnowledgeBaseKeyword.Include(u => u.Keyword)
                    .Where(u => u.KnowledgeBaseId == knowledgeBaseId && u.IsActive && u.Keyword.IsActive);

            return new KnowledgeBaseDetailModel()
            {
                KnowledgeBaseId = kbEntity.Id,
                Title = kbEntity.Title,
                Detail = kbEntity.Detail,
                KnowledgeCategory = (byte)kbEntity.KnowledgeCategory,
                LastUpdateTime = kbEntity.LastUpdateTime,
                LastUpdateUser = kbEntity.LastUpdateUser,
                LastUpdateUserName = lastUpdateUser == null ? string.Empty : lastUpdateUser.Name,
                Keywords = kbKeyworkEntities.Select(o => new KeyWord()
                {
                    Id = o.KeywordId,
                    Word = o.Keyword.Word
                }).ToList()
            };
        }

        public ResultBase SaveKnowledgeBaseDetail(KnowledgeBaseDetailModel knowledge)
        {
            var kbEntity = this._invoicingEntities.KnowledgeBase.Create();
            kbEntity.Title = knowledge.Title;
            kbEntity.Detail = knowledge.Detail;
            kbEntity.KnowledgeCategory = knowledge.KnowledgeCategory;
            kbEntity.LastUpdateUser = knowledge.LastUpdateUser;
            kbEntity.LastUpdateTime = DateTime.UtcNow;
            kbEntity.IsActive = true;

            this._invoicingEntities.KnowledgeBase.Add(kbEntity);
            this._invoicingEntities.SaveChanges();

            foreach (KeyWord word in knowledge.Keywords)
            {
                var kbKeyword = this._invoicingEntities.KnowledgeBaseKeyword.Create();
                kbKeyword.KnowledgeBaseId = kbEntity.Id;
                kbKeyword.KeywordId = word.Id;
                kbKeyword.LastUpdateUser = kbEntity.LastUpdateUser;
                kbKeyword.LastUpdateTime = kbEntity.LastUpdateTime;
                kbKeyword.IsActive = true;

                this._invoicingEntities.KnowledgeBaseKeyword.Add(kbKeyword);
            }
            this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };
        }

        public IList<KeyWord> GetKeyWordsList()
        {
            return this._invoicingEntities.Keyword.Where(p => p.IsActive).Select(p => new KeyWord()
            {
                Id = p.Id,
                Word = p.Word
            }).ToList();
        }
    }
}
