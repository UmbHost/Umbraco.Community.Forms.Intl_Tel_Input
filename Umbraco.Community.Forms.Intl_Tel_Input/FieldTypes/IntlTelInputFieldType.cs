using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Umbraco.Community.Forms.Intl_Tel_Input.Configuration;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Models;
using Umbraco.Forms.Core.Services;

namespace Umbraco.Community.Forms.Intl_Tel_Input.FieldTypes
{
    public class IntlTelInputField : FieldType
    {
        private readonly IntlTelInputSettings _config;
        public IntlTelInputField(IOptionsMonitor<IntlTelInputSettings> config)
        {
            _config = config.CurrentValue;
            Id = new Guid("a7dc31b0-651f-4f29-ada3-0244bde2a7bd");
            Name = "Intl-Tel-Input";
            Description = "Entering and validating international telephone numbers";
            Icon = "icon-phone";
            DataType = FieldDataType.String;
            SortOrder = 10;
            SupportsRegex = false;

            FieldTypeViewName = "FieldType.Intl-Tel-Input.cshtml";
            PreviewView = "intlTelInput.Field.Preview";
            MandatoryByDefault = true;
            HideLabel = true;
        }
        [Setting("Validation message", Description = "The message shown when an incorrect telephone number is entered", View = "Umb.PropertyEditorUi.TextBox")]
        public string ValidationMessage { get; set; }

        [Setting("Auto Placeholder", Description = "Set the input's placeholder to an example number for the selected country, and update it if the country changes N.B. If enabled this will replace the default placeholder set below", View = "Umb.PropertyEditorUi.Toggle")]
        public string AutoPlaceholder { get; set; }

        [Setting("Auto Placeholder Type", Description = "Set the input's placeholder the number type to use for the placeholder", View = "Umb.PropertyEditorUi.Dropdown", PreValues = "MOBILE,FIXED_LINE,FIXED_LINE_OR_MOBILE,TOLL_FREE,PREMIUM_RATE,SHARED_COST,VOIP,PERSONAL_NUMBER,PAGER,UAN,VOICEMAIL")]
        public string AutoPlaceholderType { get; set; }

        [Setting("Default Placeholder", Description = "Set the inputs default placeholder.", View = "Umb.PropertyEditorUi.TextBox")]
        public string Placeholder { get; set; }

        [Setting("Country based on IP address", Description = "Selects the user's country based on their IP address", View = "Umb.PropertyEditorUi.Toggle")]
        public string IPBasedCountry { get; set; }

        [Setting("Initial country", Description = "The default country selected in ISO2 format (e.g. GB)", View = "Umb.PropertyEditorUi.TextBox")]
        public string InitialCountry { get; set; }

        [Setting("Preferred countries", Description = "Specify the countries to appear at the top of the list in ISO2 format (Comma separated)", View = "Umb.PropertyEditorUi.TextBox")]
        public string PreferredCountries { get; set; }

        [Setting("Only countries", Description = "In the dropdown, display only the countries you specify in ISO2 format (Comma separated)", View = "Umb.PropertyEditorUi.TextBox")]
        public string OnlyCountries { get; set; }

        public override string RequiredJavascriptInitialization(Field field)
        {
            var ipBasedCountry = false;
            string ipInfoKey = null;
            string initialCountry = null;
            if (field.Settings.ContainsKey("InitialCountry"))
            {
                initialCountry = field.Settings["InitialCountry"];
            }
            if (field.Settings.ContainsKey("IPBasedCountry") && !string.IsNullOrEmpty(field.Settings["IPBasedCountry"]))
            {
                ipBasedCountry = Convert.ToBoolean(field.Settings["IPBasedCountry"]);
                if (ipBasedCountry)
                {
                    initialCountry = "auto";
                }
                ipInfoKey = _config.IPinfoKey;
            }

            var autoPlaceholder = false;
            if (field.Settings.ContainsKey("AutoPlaceholder") && !string.IsNullOrEmpty(field.Settings["AutoPlaceholder"]))
            {
                autoPlaceholder = Convert.ToBoolean(field.Settings["AutoPlaceholder"]);
            }

            string placeholderType = null;
            if (field.Settings.ContainsKey("AutoPlaceholderType"))
            {
                placeholderType = field.Settings["AutoPlaceholderType"];
            }

            string preferredCountries = "null";
            if (field.Settings.ContainsKey("PreferredCountries"))
            {
                var pfSettingValue = field.Settings["PreferredCountries"];
                if (!string.IsNullOrWhiteSpace(pfSettingValue) && !string.IsNullOrEmpty(pfSettingValue))
                {
                    var pfc = field.Settings["PreferredCountries"].Split(',');
                    if (pfc.Any())
                    {
                        preferredCountries = JsonConvert.SerializeObject(pfc);
                    }
                }
            }

            string onlyCountries = "null";
            if (field.Settings.ContainsKey("OnlyCountries"))
            {
                var ocSettingValue = field.Settings["OnlyCountries"];
                if (!string.IsNullOrWhiteSpace(ocSettingValue) && !string.IsNullOrEmpty(ocSettingValue))
                {
                    var oc = field.Settings["OnlyCountries"].Split(',');
                    if (oc.Any())
                    {
                        onlyCountries = JsonConvert.SerializeObject(oc);
                    }
                }
            }
            return $"ourUmbracoFormsIntlTelInput('t{field.Id:N}'," +
                   $"{ipBasedCountry.ToString().ToLower()}," +
                   $"'{initialCountry?.ToUpper()}'," +
                   $"{autoPlaceholder.ToString().ToLower()}," +
                   $"'{ipInfoKey}'," +
                   $"'{placeholderType}'," +
                   $"{preferredCountries}," +
                   $"{onlyCountries});";
        }

        public override IEnumerable<string> RequiredCssFiles(Field field)

        {
            var cssFiles = base.RequiredCssFiles(field).ToList();

            cssFiles.Add($"{Constants.PluginCssRoot}/intlTelInput.min.css");
            cssFiles.Add($"{Constants.PluginCssRoot}/our.umbraco.forms.intl-tel-input.css");

            return cssFiles;
        }

        public override IEnumerable<string> RequiredJavascriptFiles(Field field)
        {
            var javascriptFiles = base.RequiredJavascriptFiles(field).ToList();

            javascriptFiles.Add($"{Constants.PluginScriptRoot}/intlTelInput.min.js");
            javascriptFiles.Add($"{Constants.PluginScriptRoot}/our.umbraco.forms.intl-tel-input.js");

            if (field.Settings.ContainsKey("IPBasedCountry") && field.Settings["IPBasedCountry"] == "True"
                || field.Settings.ContainsKey("AutoPlaceholder") && field.Settings["AutoPlaceholder"] == "True")
            {
                javascriptFiles.Add($"{Constants.PluginScriptRoot}/utils.js");
            }

            javascriptFiles.Add($"{Constants.PluginScriptRoot}/validate.phonenumber.js");

            return javascriptFiles;
        }

        public override List<Exception> ValidateSettings()
        {
            var errors = new List<Exception>();
            if ((string.IsNullOrEmpty(InitialCountry) || string.IsNullOrWhiteSpace(InitialCountry)) && string.Equals(IPBasedCountry, "false", StringComparison.InvariantCultureIgnoreCase))
            {
                errors.Add(new Exception("Please enter a value for Initial Country"));
            }

            return errors.Count > 0 ? errors : base.ValidateSettings();
        }

        public override IEnumerable<string> ValidateField(Form form, Field field, IEnumerable<object> postedValues, HttpContext context,
            IPlaceholderParsingService placeholderParsingService, IFieldTypeStorage fieldTypeStorage, List<string> errors)
        {
            var invalidFields = new List<string>();
            if (postedValues.Any())
            {
                try
                {
                    var submittedNumber = postedValues.First().ToString();
                    var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                    var phoneNumber = phoneNumberUtil.Parse(submittedNumber, field.Settings["InitialCountry"]);
                    var isValid = phoneNumberUtil.IsValidNumber(phoneNumber);

                    if (!isValid)
                    {
                        invalidFields.Add("The number entered is not valid");
                        return invalidFields;
                    }
                }
                catch (Exception e)
                {
                    invalidFields.Add(e.Message);
                    return invalidFields;
                }
            }

            return base.ValidateField(form, field, postedValues, context, placeholderParsingService, fieldTypeStorage, errors);
        }
    }
}