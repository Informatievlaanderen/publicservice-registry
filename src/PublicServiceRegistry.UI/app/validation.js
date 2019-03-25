import { Validator } from 'vee-validate';
import nl from './strings/nl';


export default function prepareValidator() {
  const validatorConfig = {
    errorBagName: 'errors', // change if property conflicts
    fieldsBagName: 'fieldsBag',
    delay: 0,
    locale: 'nl',
    dictionary: null,
    strict: true,
    classes: true,
    classNames: {
      touched: 'touched', // the control has been blurred
      untouched: 'untouched', // the control hasn't been blurred
      valid: 'valid', // model is valid
      invalid: 'input-field--error', // model is invalid
      pristine: 'pristine', // control has not been interacted with
      dirty: 'dirty', // control has been interacted with
    },
    events: 'input|blur',
    inject: true,
    validity: false,
    aria: true,
    i18n: null, // the vue-i18n plugin instance,
    i18nRootKey: 'validations', // the nested key under which the validation messsages will be located
  };

  Validator.localize('nl', nl);

  return validatorConfig;
}
