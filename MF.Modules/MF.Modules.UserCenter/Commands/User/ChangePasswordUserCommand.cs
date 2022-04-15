using MediatR;
using System.ComponentModel.DataAnnotations;
using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class ChangePasswordUserCommand : IRequest<PubResponse>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户ID")]
        [Required]
        public string Id { get; set; }

        [Display(Name = "旧密码")]
        [Required]
        public string OldPassword { get; set; }

        [Display(Name = "新密码")]
        [Required]
        public string NewPassword { get; set; }

    }
}