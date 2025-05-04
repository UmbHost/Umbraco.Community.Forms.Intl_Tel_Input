import { UmbElementMixin } from "@umbraco-cms/backoffice/element-api";
import {
    LitElement,
    css,
    customElement,
    html
} from "@umbraco-cms/backoffice/external/lit";

const elementName = "intltelinput-field-preview";

@customElement(elementName)
export class intelTelInputFieldPreviewElement extends UmbElementMixin(LitElement) {

    render() {
        return html`<img src="/App_Plugins/UmbracoCommunityFormsIntlTelInput/images/intl-tel-input.png" style="max-height: 39px;" />`;
    }

    static styles = css`
  `;
}

export default intelTelInputFieldPreviewElement;

declare global {
    interface HTMLElementTagNameMap {
        [elementName]: intelTelInputFieldPreviewElement;
    }
}