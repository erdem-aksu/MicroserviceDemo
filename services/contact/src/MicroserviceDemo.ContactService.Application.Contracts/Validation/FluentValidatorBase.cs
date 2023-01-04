using System;
using FluentValidation;
using JetBrains.Annotations;
using MicroserviceDemo.ContactService.Localization;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace MicroserviceDemo.ContactService.Validation
{
    public abstract class FluentValidatorBase<TModel> : AbstractValidator<TModel>, IScopedDependency
    {
        protected IAbpLazyServiceProvider LazyServiceProvider { get; }

        protected IStringLocalizerFactory StringLocalizerFactory => LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();

        protected IStringLocalizer L
        {
            get
            {
                if (_localizer == null)
                {
                    _localizer = CreateLocalizer();
                }

                return _localizer;
            }
        }

        protected LocalizableString LS([NotNull] string name) => new(LocalizationResource, name);

        protected Type LocalizationResource
        {
            get => _localizationResource;
            set
            {
                _localizationResource = value;
                _localizer = null;
            }
        }

        private IStringLocalizer _localizer;

        private Type _localizationResource = typeof(ContactServiceResource);

        protected FluentValidatorBase(IAbpLazyServiceProvider lazyServiceProvider)
        {
            LazyServiceProvider = lazyServiceProvider;
        }

        protected virtual IStringLocalizer CreateLocalizer()
        {
            if (LocalizationResource != null)
            {
                return StringLocalizerFactory.Create(LocalizationResource);
            }

            var localizer = StringLocalizerFactory.CreateDefaultOrNull();
            if (localizer == null)
            {
                throw new AbpException($"Set {nameof(LocalizationResource)} or define the default localization resource type (by configuring the {nameof(AbpLocalizationOptions)}.{nameof(AbpLocalizationOptions.DefaultResourceType)}) to be able to use the {nameof(L)} object!");
            }

            return localizer;
        }
    }
}