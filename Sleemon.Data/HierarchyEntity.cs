namespace Sleemon.Data
{
    using System.Data.Linq.Mapping;
    using System.Xml.Serialization;

    using Microsoft.SqlServer.Types;

    using Sleemon.Common;

    public abstract class HierarchyEntity : Entity, IHierarchyEntity

    {
        public virtual int UniqueId { get; set; }

        private string _HierarchyPath;
        [XmlIgnore]
        public virtual string HierarchyPath
        {
            get
            {
                return this._HierarchyPath;
            }
            set
            {
                this._HierarchyPath = value;
                this._HierarchyId = SqlHierarchyId.Parse(this._HierarchyPath);
                this._ParentHierarchyId = this._HierarchyId.GetAncestor(1);
                this._ParentHierarchyPath = this._ParentHierarchyId.ToString();
            }
        }

        private SqlHierarchyId _HierarchyId;
        [XmlIgnore]
        public virtual SqlHierarchyId HierarchyId
        {
            get
            {
                return this._HierarchyId;
            }
            set
            {
                this._HierarchyId = value;
                this._HierarchyPath = this._HierarchyId.ToString();
                this._ParentHierarchyId = this._HierarchyId.GetAncestor(1);
                this._ParentHierarchyPath = this._ParentHierarchyId.ToString();
            }
        }

        private string _ParentHierarchyPath;
        [Column(Name = "ParentHierarchyId", DbType = "String", CanBeNull = false)]
        public virtual string ParentHierarchyPath
        {
            get
            {
                return this._ParentHierarchyPath;
            }
            set
            {
                this._ParentHierarchyPath = value;
                this._ParentHierarchyId = SqlHierarchyId.Parse(this._ParentHierarchyPath);
                if (this.UniqueId > 0)
                {
                    this._HierarchyPath = string.Concat(this._ParentHierarchyPath, this.UniqueId, @"/");
                    this._HierarchyId = SqlHierarchyId.Parse(this._HierarchyPath);
                }
            }
        }

        private SqlHierarchyId _ParentHierarchyId;
        [XmlIgnore]
        public virtual SqlHierarchyId ParentHierarchyId
        {
            get
            {
                return this._ParentHierarchyId;
            }
            set
            {
                this._ParentHierarchyId = value;
                this._ParentHierarchyPath = this.ParentHierarchyId.ToString();
                if (this.UniqueId > 0)
                {
                    this._HierarchyPath = string.Concat(this._ParentHierarchyPath, this.UniqueId, @"/");
                    this._HierarchyId = SqlHierarchyId.Parse(this._HierarchyPath);
                }
            }
        }

        [XmlIgnore]
        public virtual short Level
        {
            get
            {
                return this.ParentHierarchyId.IsNull ? (short)-1 : this.ParentHierarchyId.GetLevel().Value;
            }
            set { }
        }
    }
}
