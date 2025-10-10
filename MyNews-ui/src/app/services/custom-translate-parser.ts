import { TranslateParser } from '@ngx-translate/core';

export class CustomTranslateParser extends TranslateParser {
    interpolate(expr: string | Function, params?: any): string {
        if (typeof expr === 'function') {
            return expr(params);
        }
        return expr;
    }

    getValue(target: any, key: string): any {
        return target ? target[key] : undefined;
    }
}