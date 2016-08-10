namespace Sleemon.Data
{
    using System;
    using System.Collections.Generic;

    public class KnowledgeDetailModel : KnowledgeBaseModel
    {
        public string Detail { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public IList<string> Keywords { get; set; }
    }

    public class KnowledgeDetailComparer : IEqualityComparer<KnowledgeDetailModel>
    {
        public bool Equals(KnowledgeDetailModel x, KnowledgeDetailModel y)
        {
            return x.KnowledgeBaseId == y.KnowledgeBaseId;
        }

        public int GetHashCode(KnowledgeDetailModel obj)
        {
            return obj.KnowledgeBaseId.GetHashCode();
        }
    }
}
