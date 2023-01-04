using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;

namespace MicroserviceDemo.Web.Helpers
{
    public static class FluentValidatorExtensions
    {
        public static Func<object, string, Task<IEnumerable<string>>> ValidateValue<TModel>(this IValidator<TModel> validator)
        {
            return async (model, propertyName) =>
            {
                var result = await validator.ValidateAsync((TModel) model, x => x.IncludeProperties(propertyName));

                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        public static Func<TValue, Task<IEnumerable<string>>> ValidateValue<TModel, TValue>(this IValidator<TModel> validator, TModel model, string propertyPath)
        {
            return async _ =>
            {
                var result = await validator.ValidateAsync(model, x => x.IncludeProperties(propertyPath));

                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }

        public static Func<TValue, Task<IEnumerable<string>>> ValidateValue<TModel, TValue>(this IValidator<TModel> validator, TModel model, Expression<Func<TModel, object>> expression)
        {
            return async _ =>
            {
                var result = await validator.ValidateAsync(model, x => x.IncludeProperties(PropertyPath<TModel>.GetString(expression)));

                return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
            };
        }
    }
}