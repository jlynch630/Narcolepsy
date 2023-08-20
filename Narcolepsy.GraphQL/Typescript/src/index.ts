declare const monaco: typeof import("monaco-editor");
import { getIntrospectionQuery } from "graphql";
import "./monaco-graphql/src/monaco.contribution";

export async function setSchema(content: string) {
    // @ts-ignore
    monaco.languages.graphql.api.setSchemaConfig([
        {
            introspectionJSON: JSON.parse(content).data,
        },
    ]);
}

export async function initializeGraphQL() {
    const oldGet = window.MonacoEnvironment?.getWorker;
    if (!window.MonacoEnvironment) window.MonacoEnvironment = {};

    window.MonacoEnvironment.getWorker = (_workerId: string, label: string) => {
        if (label === "graphql") {
            return new Worker("_content/Narcolepsy.GraphQL/script/graphqlWorker.js");
        }

        return oldGet ? oldGet(_workerId, label) : new Worker("editor.worker.js");
    };
}
