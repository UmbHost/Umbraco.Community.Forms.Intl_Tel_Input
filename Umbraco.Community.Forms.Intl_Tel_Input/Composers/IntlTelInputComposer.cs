using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Community.Forms.Intl_Tel_Input.Configuration;
using Umbraco.Community.Forms.Intl_Tel_Input.Extensions;

namespace Umbraco.Community.Forms.Intl_Tel_Input.Composers
{
	public class IntlTelInputComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.Services.AddSingleton<IntlTelInputSettings>();
            builder.AddIntlTelInput();
        }
	}
}