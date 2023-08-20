/**
 *  Copyright (c) 2021 GraphQL Contributors.
 *
 *  This source code is licensed under the MIT license found in the
 *  LICENSE file in the root directory of this source tree.
 */

export {
  modeConfigurationDefault,
  SchemaEntry,
  formattingDefaults,
  MonacoGraphQLAPI,
  MonacoGraphQLAPIOptions,
  diagnosticSettingDefault,
} from "./api";

import type * as monacoEditor from "monaco-editor";
declare const monaco: typeof monacoEditor;
import { initializeMode, LANGUAGE_ID } from "./initializeMode";

export * from "./typings";

export { LANGUAGE_ID };

// here is the only place where we
// initialize the mode `onLanguage`
monaco.languages.onLanguage(LANGUAGE_ID, () => {
  const api = initializeMode();

  (monaco.languages as any).graphql = { api };
});
