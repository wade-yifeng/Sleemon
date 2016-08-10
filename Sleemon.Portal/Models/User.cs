using System.ComponentModel.DataAnnotations;

namespace Sleemon.Portal.Models
{
    public class User
    {
        /// <summary>
        /// 企业号用户ID(员工号)
        /// </summary>
        public string UserUniqueId { get; set; }

        /// <summary>
        /// 存放在ClaimsIdentity
        /// 显示用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 存放在ClaimsIdentity
        /// 显示用户头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 登陆跳转地址
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 是否自动登陆
        /// </summary>
        public bool IsAutoLogin { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
    }
}
