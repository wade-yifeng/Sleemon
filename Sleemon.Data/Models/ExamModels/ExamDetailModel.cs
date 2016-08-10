namespace Sleemon.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ExamDetailModel : ExamBasicModel
    {
        public List<ExamQuestionModel> Questions { get; set; }
    }

    public class ExamQuestionModel
    {
        public int ExamQuestionId { get; set; }

        public short No { get; set; }

        [Required(ErrorMessage = "请输入题目")]
        public string Question { get; set; }

        public string Image { get; set; }

        public byte Category { get; set; }

        public string CorrectAnswer { get; set; }

        [Required(ErrorMessage = "请输入单题得分")]
        [Range(0, double.MaxValue, ErrorMessage = "单题得分分必须为数字")]
        public double Score { get; set; }

        public List<ExamChoiceModel> Choices { get; set; }
    }

    public class ExamChoiceModel
    {
        [Required(ErrorMessage = "请选择选项")]
        public byte Choice { get; set; }

        [Required(ErrorMessage = "请输入选项描述")]
        public string Description { get; set; }

        public string Image { get; set; }

        public bool IsAnswer { get; set; }
    }
}
