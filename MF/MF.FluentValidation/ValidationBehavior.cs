using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MF.FluentValidation
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
   where TRequest : IRequest<TResponse>
       where TResponse : PubResponse
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();
            //List<Error> errors = _validators
            //    .Select(x => x.Validate(context))
            //    .SelectMany(x => x.Errors)
            //    .Select(x => new Error(x.ErrorCode, x.ErrorMessage, x.PropertyName))
            //    .ToList();
            return failures.Any() ? Errors(failures) : next();
        }

        private static Task<TResponse> Errors(IEnumerable<ValidationFailure> validations)
        {
            var response = new PubResponse(validations);
            return Task.FromResult(response as TResponse);
        }
    }
}