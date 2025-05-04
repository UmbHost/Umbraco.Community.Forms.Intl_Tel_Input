const intlTelInputFieldPreviewManifest = {
    type: "formsFieldPreview",
    alias: "intlTelInput.Field.Preview",
    name: "Intl Tel Input Preview",
    element: () => import('./intltelinput-field-preview.js')
};

export const manifests = [intlTelInputFieldPreviewManifest];