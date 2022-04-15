using FluentValidation;
using UserCenter.Commands;
using DAL.UserCenter.IRepository;

namespace UserCenter.Validators
{
    public class CreateUcAccessLogCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IAccesslogRepository _Repository;

        public CreateUcAccessLogCommandValidator(IAccesslogRepository Repository)
        {
            _Repository = Repository;

            RuleFor(c => c.Dto.Id).NotEmpty().WithMessage("id不能为空");
            //RuleFor(c => c.Id).Must(Validator).WithMessage("重复");
        }

        // 自定义逻辑
        private bool Validator(CreateAccessLogCommand command, string No)
        {
            //var dept = _Repository.QueryableToEntity(c => c.No == No && c.DelFlag == 0);
            //return dept is null;

            return false;
        }
    }
}