namespace Sleemon.Data
{
    using System;
    using System.Collections.Generic;

    public class KnowledgeBaseDetailModel
    {
        public int KnowledgeBaseId { get; set; }

        public string Title { get; set; }

        public byte KnowledgeCategory { get; set; }

        public string Detail { get; set; }

        public string LastUpdateUserName { get; set; }

        public string LastUpdateUser { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public IList<KeyWord> Keywords { get; set; }
    }

    public class KeyWord
    {
        public int Id { get; set; }

        public string Word { get; set; }
    }
}
