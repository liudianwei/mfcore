using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

using System;
using System.Linq;
using System.Reflection;

using MF.FluentValidation;
using MF.Utils;
using Folke.Localization.Json;

namespace MF.Extensions.DependencyInjection
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddMvcValidationBehavior(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    if (context.ModelState.IsValid)
                        return null;

                    //var localizer = services.BuildServiceProvider().GetService(typeof(IStringLocalizer<Msg>));

                    var localizer = new JsonStringLocalizer("lang", "MF.Utils.Msg", "errMsg", "zh-cn");
                    var error = "";
                    foreach (var item in context.ModelState)
                    {
                        var state = item.Value;
                        var message = state.Errors.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.ErrorMessage))?.ErrorMessage;
                        if (string.IsNullOrWhiteSpace(message))
                        {
                            message = state.Errors.FirstOrDefault(o => o.Exception != null)?.Exception.Message;
                        }
                        if (string.IsNullOrWhiteSpace(message))
                            continue;
                        error = localizer.Get(message).Value;
                        break;
                    }
                    throw new Exception(error);
                };
            });

            return services;
        }

        public static IServiceCollection AddValidationBehavior(this IServiceCollection services)
        {
            return services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public static IServiceCollection AddValidation(this IServiceCollection services, Assembly assembly)
        {
            AssemblyScanner
               .FindValidatorsInAssembly(assembly)
               .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
            return services;
        }
    }
}