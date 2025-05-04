import { UmbElementMixin as p } from "@umbraco-cms/backoffice/element-api";
import { LitElement as o, html as u, css as a, customElement as c } from "@umbraco-cms/backoffice/external/lit";
var v = Object.getOwnPropertyDescriptor, g = (n, r, s, i) => {
  for (var e = i > 1 ? void 0 : i ? v(r, s) : r, l = n.length - 1, m; l >= 0; l--)
    (m = n[l]) && (e = m(e) || e);
  return e;
};
const f = "intltelinput-field-preview";
let t = class extends p(o) {
  render() {
    return u`<img src="/App_Plugins/UmbracoCommunityFormsIntlTelInput/images/intl-tel-input.png" style="max-height: 39px;" />`;
  }
};
t.styles = a`
  `;
t = g([
  c(f)
], t);
const x = t;
export {
  x as default,
  t as intelTelInputFieldPreviewElement
};
//# sourceMappingURL=intltelinput-field-preview-8gj3IgqC.js.map
