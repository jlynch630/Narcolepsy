import type { editor } from 'monaco-editor';

export async function createEditor(element: HTMLElement, language: string, isReadOnly: boolean, initialValue?: string) {
	const monaco = await import('monaco-editor');
	return monaco.editor.create(element, {
		value: initialValue,
		language: language,
		readOnly: isReadOnly,
		automaticLayout: true,
		minimap: {
			enabled: false
		},
		wordWrap: "on"
	});
}

export async function createModel(text: string, language: string) {
	return (await import('monaco-editor')).editor.createModel(text, language);
}

export async function colorize(text: string, language: string) {
	const monaco = await import('monaco-editor');

	// json is broken
	// see https://github.com/microsoft/monaco-editor/issues/3105
	return monaco.editor.colorize(text, language, null);
}

export async function format(text: string, extension?: string) {
	const prettier = await import("prettier");
	try {
		return prettier.format(text, {
			filepath: extension ? `file.${extension}` : null,
			tabWidth: 4,
			useTabs: true,
			quoteProps: "preserve",
			plugins: [
				await import("prettier/parser-babel"),
				await import("prettier/parser-html"),
				await import("prettier/parser-postcss"),
				// @ts-ignore
				await import("@prettier/plugin-xml"),
			]
		});
	} catch {
		return text;
	}
}

export function blur(el: HTMLElement) {
	el.blur();
}

export function listenToEditorChanges(editor: editor.IStandaloneCodeEditor, ref: IDotNetEditor) {
	editor.onDidChangeModelContent(() => {
		ref.invokeMethodAsync("InvokeModelContentChanged", editor.getValue({ lineEnding: "\r\n", preserveBOM: false })).catch(e => {
			console.error("Error invoking model content change", e)
		});
	});
}

interface IDotNetEditor {
	invokeMethodAsync(name: string, ...parameters: any[]): Promise<void>;
}