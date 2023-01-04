using FluentValidation;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Data;

namespace MicroserviceDemo.Web.Shared.ObjectExtending
{
    public partial class ExtensionProperties<TEntityType> where TEntityType : IHasExtraProperties
    {
        [Parameter]
        public TEntityType Entity { get; set; }

        [Parameter]
        public IValidator<TEntityType> Validator { get; set; }
    }
}