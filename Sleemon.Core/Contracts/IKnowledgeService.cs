namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public interface IKnowledgeService
    {
        IList<KnowledgeBaseModel> SearchKBList(string input);

        IList<KnowledgeDetailModel> GetKBListByCategory(int knowledgeCategory);

        IList<KnowledgeDetailModel> GetKBLisetByKeyword(string keyword);

        KnowledgeDetailModel GetKBDetail(int knowledgeBaseId);

        IList<KnowledgeBaseDetailModel> GetKnowledgeBaseList();

        KnowledgeBaseDetailModel GetKnowledgeBaseDetail(int knowledgeBaseId);

        ResultBase SaveKnowledgeBaseDetail(KnowledgeBaseDetailModel knowledge);

        IList<KeyWord> GetKeyWordsList();
    }
}
