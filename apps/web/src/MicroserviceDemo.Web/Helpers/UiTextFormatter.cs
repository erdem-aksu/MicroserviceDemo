using Microsoft.Extensions.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Reflection;

namespace MicroserviceDemo.Web.Helpers;

public static class UiTextFormatter
{
    public static string Format(object value, IStringLocalizer localizer)
    {
        if (value == null)
        {
            return null;
        }

        var type = TypeHelper.StripNullable(value.GetType());

        if (type.IsEnum)
        {
            var memberName = type.GetEnumName(value);

            var localizedMemberName = AbpInternalLocalizationHelper.LocalizeWithFallback(
                new[] { localizer },
                new[]
                {
                    $"Enum:{type.Name}.{memberName}",
                    $"{type.Name}.{memberName}",
                    memberName
                },
                memberName
            );

            return localizedMemberName;
        }

        if (type == typeof(bool))
        {
            var boolValue = (bool) value;
            var localizedMemberName = AbpInternalLocalizationHelper.LocalizeWithFallback(
                new[] { localizer },
                new[]
                {
                    $"Bool:{boolValue}",
                    boolValue.ToString()
                },
                boolValue.ToString()
            );

            return localizedMemberName;
        }

        return value.ToString();
    }
}