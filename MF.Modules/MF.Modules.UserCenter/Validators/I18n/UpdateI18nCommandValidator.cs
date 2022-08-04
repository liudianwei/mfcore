using FluentValidation;
using MDCenter.Commands.I18n;
using DAL.MDCenter.IRepository;

namespace MDCenter.Validators
{
    public class UpdateI18nCommandValidator : AbstractValidator<UpdateI18nCommand>
    {
        private readonly II18nRepository _ucI18nRepository;

        public UpdateI18nCommandValidator(II18nRepository I18nRepository)
        {
            _ucI18nRepository = I18nRepository;

            RuleFor(c => c.Id).NotNull().NotEmpty().WithMessage("id 不能为空");
            RuleFor(c => c).Must(Validator).WithMessage("没有找到该对象");
        }

        // 自定义校验逻辑
        private bool Validator(UpdateI18nCommand cmd)
        {
            var entity = _ucI18nRepository.Queryable().InSingle(cmd.Id);
            if (entity == null)
            {
                return false;
            }

            return true;
        }
    }
}