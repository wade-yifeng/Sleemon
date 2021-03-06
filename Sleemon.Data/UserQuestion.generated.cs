//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sleemon.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq.Mapping;
    
    public partial class UserQuestion
        : Entity
    {
        [Column(Name = "Id", DbType = "Int32", IsPrimaryKey = true, CanBeNull = false, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(Name = "UserUniqueId", DbType = "String", CanBeNull = false)]
        public string UserUniqueId { get; set; }
        [Column(Name = "Title", DbType = "String", CanBeNull = false)]
        public string Title { get; set; }
        [Column(Name = "Question", DbType = "String")]
        public string Question { get; set; }
        [Column(Name = "AskTime", DbType = "DateTime")]
    	private Nullable<System.DateTime> _askTime;
    	public virtual Nullable<System.DateTime> AskTime 
    	{
    		get { return _askTime; }
    		set 
    		{
    			if (value != null) {
    				if (value.Value.Kind == DateTimeKind.Unspecified) {
    					_askTime = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
    				} else {
    					_askTime = value;
    				}
    			} else {
    				_askTime = value;
    			}
    		}
    	}
        [Column(Name = "Replier", DbType = "String")]
        public string Replier { get; set; }
        [Column(Name = "Answer", DbType = "String")]
        public string Answer { get; set; }
        [Column(Name = "AnswerTime", DbType = "DateTime")]
    	private Nullable<System.DateTime> _answerTime;
    	public virtual Nullable<System.DateTime> AnswerTime 
    	{
    		get { return _answerTime; }
    		set 
    		{
    			if (value != null) {
    				if (value.Value.Kind == DateTimeKind.Unspecified) {
    					_answerTime = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
    				} else {
    					_answerTime = value;
    				}
    			} else {
    				_answerTime = value;
    			}
    		}
    	}
        [Column(Name = "Status", DbType = "Byte", CanBeNull = false)]
        public byte Status { get; set; }
        [Column(Name = "IsActive", DbType = "Boolean", CanBeNull = false)]
        public bool IsActive { get; set; }
    }
}
