using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.Forms.Intl_Tel_Input.Configuration;
using Umbraco.Community.Forms.Intl_Tel_Input.FieldTypes;
using Umbraco.Forms.Core.Providers;

namespace Umbraco.Community.Forms.Intl_Tel_Input.Extensions
{
	public static class UmbracoBuilderExtensions
	{
        public static IUmbracoBuilder AddIntlTelInput(this IUmbracoBuilder builder)
        {
            builder.WithCollectionBuilder<FieldCollectionBuilder>().Add<IntlTelInputField>();
            builder.Services.Configure<IntlTelInputSettings>((IConfiguration)builder.Config.GetSection(Constants.IntlTelInput));
            return builder;
        }
    }
}
