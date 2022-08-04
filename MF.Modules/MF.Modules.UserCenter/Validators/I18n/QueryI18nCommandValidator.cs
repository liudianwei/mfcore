using FluentValidation;
using MDCenter.Commands.I18n;
using DAL.MDCenter.IRepository;

namespace MDCenter.Validators
{
    public class QueryI18nCommandValidator : AbstractValidator<QueryI18nCommand>
    {
        private readonly II18nRepository _ucI18nRepository;

        public QueryI18nCommandValidator(II18nRepository I18nRepository)
        {
            _ucI18nRepository = I18nRepository;

            RuleFor(c => c.Id).NotNull().NotEmpty().WithMessage("id 不能为空");
            //RuleFor(c => c).Must(Validator).WithMessage("xxx 不能重复");
        }

        // 自定义校验逻辑
        private bool Validator(QueryI18nCommand cmd)
        {
            //var entity = _ucI18nRepository.QueryableToEntity(e => e.xxx.Equals(cmd.xxx));
            //if (entity != null)
            //{
            //    return false;
            //}

            return false;
        }
    }
}