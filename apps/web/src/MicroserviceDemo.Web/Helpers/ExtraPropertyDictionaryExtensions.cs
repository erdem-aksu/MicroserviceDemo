using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Reflection;

namespace MicroserviceDemo.Web.Helpers
{
    public static class ExtraPropertyDictionaryExtensions
    {
        public static bool HasProperty(this ExtraPropertyDictionary source, string name)
        {
            return source.ContainsKey(name);
        }

        public static object GetProperty(this ExtraPropertyDictionary source, string name, Type type = null, object defaultValue = null)
        {
            var value = source?.GetOrDefault(name);

            return type == null ? value ?? defaultValue : ChangeType(value, type, defaultValue);
        }

        public static TProperty GetPropertyAs<TProperty>(this ExtraPropertyDictionary source, string name, TProperty defaultValue = default)
        {
            var value = source?.GetOrDefault(name);

            return ChangeTypeAs(value, defaultValue);
        }

        public static object SetProperty(this ExtraPropertyDictionary source, string name, object value, Type type)
        {
            Check.NotNull(name, nameof(name));

            return source[name] = ChangeType(value, type);
        }

        public static TProperty SetProperty<TProperty>(this ExtraPropertyDictionary source, string name, TProperty value)
        {
            return (TProperty) SetProperty(source, name, value, typeof(TProperty));
        }

        public static ExtraPropertyDictionary RemoveProperty(this ExtraPropertyDictionary source, string name)
        {
            source.Remove(name);

            return source;
        }

        public static object ChangeType(object value, Type type, object defaultValue = null)
        {
            if (value == null)
            {
                return defaultValue;
            }

            if (TypeHelper.IsPrimitiveExtended(type, includeEnums: true))
            {
                var conversionType = type;
                if (TypeHelper.IsNullable(conversionType))
                {
                    conversionType = conversionType.GetFirstGenericArgumentIfNullable();
                }

                if (conversionType == typeof(Guid))
                {
                    return TypeDescriptor.GetConverter(conversionType).ConvertFromInvariantString(value.ToString());
                }

                if (conversionType.IsEnum)
                {
                    return TypeDescriptor.GetConverter(conversionType).ConvertFromInvariantString(value.ToString());
                }

                return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
            }

            throw new AbpException("ChangeType does not support non-primitive types. Use non-generic GetProperty method and handle type casting manually.");
        }

        public static TValue ChangeTypeAs<TValue>(object value, TValue defaultValue = default)
        {
            return (TValue) ChangeType(value, typeof(TValue), defaultValue);
        }
    }
}