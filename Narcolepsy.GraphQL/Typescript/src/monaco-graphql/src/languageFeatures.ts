/**
 *  Copyright (c) 2021 GraphQL Contributors.
 *
 *  This source code is licensed under the MIT license found in the
 *  LICENSE file in the root directory of this source tree.
 */

import { GraphQLWorker } from "./GraphQLWorker";
import type { MonacoGraphQLAPI } from "./api";
import type * as monacoEditor from "monaco-editor";
declare const monaco: typeof monacoEditor;
import { CompletionItemKind as lsCompletionItemKind } from "graphql-language-service";
import { getModelLanguageId, GraphQLWorkerCompletionItem } from "./utils";

export interface WorkerAccessor {
  (...more: monacoEditor.Uri[]): monacoEditor.Thenable<GraphQLWorker>;
}

// --- completion ------

export class DiagnosticsAdapter {
  private _disposables: monacoEditor.IDisposable[] = [];
  private _listener: { [uri: string]: monacoEditor.IDisposable } =
    Object.create(null);

  constructor(
    private defaults: MonacoGraphQLAPI,
    private _worker: WorkerAccessor
  ) {
    this._worker = _worker;
    let onChangeTimeout: ReturnType<typeof setTimeout>;
    const onModelAdd = (model: monacoEditor.editor.IModel): void => {
      const modeId = getModelLanguageId(model);
      if (modeId !== this.defaults.languageId) {
        // it is tempting to load json models we cared about here
        // into the webworker, however setDiagnosticOptions() needs
        // to be called here from main process anyway, and the worker
        // is already generating json schema itself!
        return;
      }
      const modelUri = model.uri.toString();
      // if the config changes, this adapter will be re-instantiated, so we only need to check this once
      const jsonValidationForModel =
        defaults.diagnosticSettings?.validateVariablesJSON &&
        defaults.diagnosticSettings.validateVariablesJSON[modelUri];
      // once on adding a model, this is also fired when schema or other config changes
      onChangeTimeout = setTimeout(() => {
        void this._doValidate(model.uri, modeId, jsonValidationForModel);
      }, 400);

      this._listener[modelUri] = model.onDidChangeContent(() => {
        clearTimeout(onChangeTimeout);
        onChangeTimeout = setTimeout(() => {
          void this._doValidate(model.uri, modeId, jsonValidationForModel);
        }, 400);
      });
    };

    const onModelRemoved = (model: monacoEditor.editor.IModel): void => {
      monaco.editor.setModelMarkers(model, this.defaults.languageId, []);
      const uriStr = model.uri.toString();
      const listener = this._listener[uriStr];

      if (listener) {
        listener.dispose();
        delete this._listener[uriStr];
      }
    };

    this._disposables.push(
      monaco.editor.onDidCreateModel(onModelAdd),
      {
        dispose() {
          clearTimeout(onChangeTimeout);
        },
      },
      monaco.editor.onWillDisposeModel((model) => {
        onModelRemoved(model);
      }),
      monaco.editor.onDidChangeModelLanguage((event) => {
        onModelRemoved(event.model);
        onModelAdd(event.model);
      }),
      {
        dispose: () => {
          for (const key in this._listener) {
            this._listener[key].dispose();
          }
        },
      },
      defaults.onDidChange(() => {
        for (const model of monaco.editor.getModels()) {
          if (getModelLanguageId(model) === this.defaults.languageId) {
            onModelRemoved(model);
            onModelAdd(model);
          }
        }
      })
    );
    for (const model of monaco.editor.getModels()) {
      if (getModelLanguageId(model) === this.defaults.languageId) {
        onModelAdd(model);
      }
    }
  }

  public dispose(): void {
    for (const d of this._disposables) {
      d?.dispose();
    }
    this._disposables = [];
  }

  private async _doValidate(
    resource: monacoEditor.Uri,
    languageId: string,
    variablesUris?: string[]
  ) {
    const worker = await this._worker(resource);

    // to handle an edge case bug that happens when
    // typing before the schema is present
    if (!worker) {
      return;
    }

    const diagnostics = await worker.doValidation(resource.toString());
    monaco.editor.setModelMarkers(
      monaco.editor.getModel(resource)!,
      languageId,
      diagnostics
    );

    if (variablesUris) {
      // only import the json mode if users configure it
      await import("monaco-editor/esm/vs/language/json/monaco.contribution.js");

      if (variablesUris.length < 1) {
        throw new Error("no variables URI strings provided to validate");
      }
      const jsonSchema = await worker.doGetVariablesJSONSchema(
        resource.toString()
      );
      if (!jsonSchema) {
        return;
      }

      const schemaUri = monaco.Uri.file(
        variablesUris[0].replace(".json", "-schema.json")
      ).toString();
      const configResult = {
        uri: schemaUri,
        schema: jsonSchema,
        fileMatch: variablesUris,
      };
      // TODO: export from api somehow?
      monaco.languages.json.jsonDefaults.setDiagnosticsOptions({
        schemaValidation: "error",
        validate: true,
        ...this.defaults?.diagnosticSettings?.jsonDiagnosticSettings,
        schemas: [configResult],
        enableSchemaRequest: false,
      });
    }
  }
}

const mKind = monaco.languages.CompletionItemKind;

const kindMap: Record<
  lsCompletionItemKind,
  monacoEditor.languages.CompletionItemKind
> = {
  [lsCompletionItemKind.Text]: mKind.Text,
  [lsCompletionItemKind.Method]: mKind.Method,
  [lsCompletionItemKind.Function]: mKind.Function,
  [lsCompletionItemKind.Constructor]: mKind.Constructor,
  [lsCompletionItemKind.Field]: mKind.Field,
  [lsCompletionItemKind.Variable]: mKind.Variable,
  [lsCompletionItemKind.Class]: mKind.Class,
  [lsCompletionItemKind.Interface]: mKind.Interface,
  [lsCompletionItemKind.Module]: mKind.Module,
  [lsCompletionItemKind.Property]: mKind.Property,
  [lsCompletionItemKind.Unit]: mKind.Unit,
  [lsCompletionItemKind.Value]: mKind.Value,
  [lsCompletionItemKind.Enum]: mKind.Enum,
  [lsCompletionItemKind.Keyword]: mKind.Keyword,
  [lsCompletionItemKind.Snippet]: mKind.Snippet,
  [lsCompletionItemKind.Color]: mKind.Color,
  [lsCompletionItemKind.File]: mKind.File,
  [lsCompletionItemKind.Reference]: mKind.Reference,
  [lsCompletionItemKind.Folder]: mKind.Folder,
  [lsCompletionItemKind.EnumMember]: mKind.EnumMember,
  [lsCompletionItemKind.Constant]: mKind.Constant,
  [lsCompletionItemKind.Struct]: mKind.Struct,
  [lsCompletionItemKind.Event]: mKind.Event,
  [lsCompletionItemKind.Operator]: mKind.Operator,
  [lsCompletionItemKind.TypeParameter]: mKind.TypeParameter,
};

export function toCompletionItemKind(
  kind: lsCompletionItemKind
): monacoEditor.languages.CompletionItemKind {
  return kind in kindMap ? kindMap[kind] : mKind.Text;
}

export function toCompletion(
  entry: GraphQLWorkerCompletionItem
): monacoEditor.languages.CompletionItem {
  const suggestions: monacoEditor.languages.CompletionItem = {
    // @ts-ignore
    range: entry.range,
    kind: toCompletionItemKind(entry.kind!),
    label: entry.label,
    insertText: entry.insertText ?? entry.label,
    insertTextRules: entry.insertText
      ? monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet
      : undefined,
    sortText: entry.sortText,
    filterText: entry.filterText,
    documentation: entry.documentation,
    detail: entry.detail,
    command: entry.command,
  };
  return suggestions;
}

export class CompletionAdapter
  implements monacoEditor.languages.CompletionItemProvider
{
  constructor(private _worker: WorkerAccessor) {
    this._worker = _worker;
  }

  public get triggerCharacters(): string[] {
    // removing /n character for now until we can
    // re-introduce the behavior in a programmatic,
    // context-aware fashion
    return [":", "$", " ", "(", "@"];
  }

  async provideCompletionItems(
    model: monacoEditor.editor.IReadOnlyModel,
    position: monacoEditor.Position,
    _context: monacoEditor.languages.CompletionContext,
    _token: monacoEditor.CancellationToken
  ): Promise<monacoEditor.languages.CompletionList> {
    try {
      const resource = model.uri;
      const worker = await this._worker(model.uri);
      const completionItems = await worker.doComplete(
        resource.toString(),
        position
      );
      return {
        incomplete: true,
        suggestions: completionItems.map(toCompletion),
      };
    } catch (err) {
      // eslint-disable-next-line no-console
      console.error("Error fetching completion items", err);
      return { suggestions: [] };
    }
  }
}

export class DocumentFormattingAdapter
  implements monacoEditor.languages.DocumentFormattingEditProvider
{
  constructor(private _worker: WorkerAccessor) {
    this._worker = _worker;
  }

  async provideDocumentFormattingEdits(
    document: monacoEditor.editor.ITextModel,
    _options: monacoEditor.languages.FormattingOptions,
    _token: monacoEditor.CancellationToken
  ) {
    const worker = await this._worker(document.uri);

    const formatted = await worker.doFormat(document.uri.toString());
    if (!formatted) {
      return [];
    }
    return [
      {
        range: document.getFullModelRange(),
        text: formatted,
      },
    ];
  }
}

export class HoverAdapter implements monacoEditor.languages.HoverProvider {
  constructor(private _worker: WorkerAccessor) {}

  async provideHover(
    model: monacoEditor.editor.IReadOnlyModel,
    position: monacoEditor.Position,
    _token: monacoEditor.CancellationToken
  ): Promise<monacoEditor.languages.Hover> {
    const resource = model.uri;
    const worker = await this._worker(model.uri);
    const hoverItem = await worker.doHover(resource.toString(), position);

    if (hoverItem) {
      return {
        range: hoverItem.range,
        contents: [{ value: hoverItem.content as string }],
      };
    }

    return {
      contents: [],
    };
  }

  dispose() {}
}
