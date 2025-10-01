if (window.jQuery && jQuery.validator) {
    jQuery.validator.addMethod("validatephonenumber", function(value, element, params) {
        var fieldId = element.getAttribute('data-fieldid');
        var hiddenInput = document.getElementById(fieldId);

        if (!hiddenInput) {
            console.warn("Hidden phone input not found for fieldId:", fieldId);
            return true; // fail gracefully
        }

        const isRequired = element.hasAttribute('data-val-required') || element.required;
        if (!value && !isRequired) {
            return true; // reset validation
        }

        var iti = window.intlTelInputUtils;
        var cc = element.getAttribute('data-intl-input-cc');

        if (!iti) {
            console.warn("intlTelInputUtils not loaded");
            return true; // fail gracefully
        }

        let isValid = iti.isValidNumber(hiddenInput.value, cc);

        if (!isValid) {
            return false; // Just return false
        }
        return true;
    });

    // Register the unobtrusive adapter
    jQuery.validator.unobtrusive.adapters.addBool("validatephonenumber");

} else {
    function addCustomPhoneValidator() {
        const uf = window.umbracoFormsValidationService;
        if (!uf) return setTimeout(addCustomPhoneValidator, 50);

        // Only add if it doesn't already exist
        if (!uf.providers['validatephonenumber']) {
            uf.addProvider('validatephonenumber', function(value, element, params) {
                var fieldId = element.getAttribute('data-fieldid');
                var hiddenInput = document.getElementById(fieldId);

                if (!hiddenInput) {
                    console.warn("Hidden phone input not found for fieldId:", fieldId);
                    return true; // fail gracefully
                }

                const isRequired = element.hasAttribute('data-val-required') || element.required;
                if (!value && !isRequired) {
                    return true; // reset validation
                }

                var iti = window.intlTelInputUtils;
                var cc = element.getAttribute('data-intl-input-cc');

                if (!iti) {
                    console.warn("intlTelInputUtils not loaded");
                    return true; // fail gracefully
                }

                let isValid = iti.isValidNumber(hiddenInput.value, cc);

                if (!isValid) {
                    return false; // Just return false
                }
                return true;
            });

            // Wrap it like Umbraco does for hidden elements
            uf.providers['validatephonenumber'] = (function(orig) {
                return function(u, f, m) {
                    return f.offsetParent === null ? true : orig(u, f, m);
                }
            })(uf.providers['validatephonenumber']);
        }
    }

    addCustomPhoneValidator();
}
