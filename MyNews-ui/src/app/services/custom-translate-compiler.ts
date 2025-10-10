import { TranslateCompiler } from '@ngx-translate/core';

export class CustomTranslateCompiler extends TranslateCompiler {
  compile(value: string, lang: string): string {
    return value;
  }

  compileTranslations(translations: any, lang: string): any {
    return translations;
  }
}