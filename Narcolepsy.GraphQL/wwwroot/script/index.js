/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ var __webpack_modules__ = ({

/***/ "./src/index.ts":
/*!**********************!*\
  !*** ./src/index.ts ***!
  \**********************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   initializeGraphQL: () => (/* binding */ initializeGraphQL),\n/* harmony export */   setSchema: () => (/* binding */ setSchema)\n/* harmony export */ });\n/* harmony import */ var graphql__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! graphql */ \"./node_modules/graphql/utilities/getIntrospectionQuery.mjs\");\n/* harmony import */ var _monaco_graphql_src_monaco_contribution__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./monaco-graphql/src/monaco.contribution */ \"./src/monaco-graphql/src/monaco.contribution.ts\");\n\n\nasync function setSchema(content) {\n    const q = (0,graphql__WEBPACK_IMPORTED_MODULE_1__.getIntrospectionQuery)();\n    const f = await fetch(\"https://anchor.zeet.co/graphql\", {\n        body: q,\n        headers: {\n            \"content-type\": \"application/json\"\n        },\n        method: \"POST\"\n    });\n    const res = await f.json();\n    console.log(res);\n    // @ts-ignore\n    monaco.languages.graphql.api.setSchemaConfig([\n        {\n            introspectionJSON: JSON.parse(content).data,\n        },\n    ]);\n}\nasync function initializeGraphQL() {\n    const oldGet = window.MonacoEnvironment?.getWorker;\n    if (!window.MonacoEnvironment)\n        window.MonacoEnvironment = {};\n    window.MonacoEnvironment.getWorker = (_workerId, label) => {\n        if (label === \"graphql\") {\n            return new Worker(\"_content/Narcolepsy.GraphQL/script/graphqlWorker.js\");\n        }\n        return oldGet ? oldGet(_workerId, label) : new Worker(\"editor.worker.js\");\n    };\n}\n\n\n//# sourceURL=webpack://narcolepsy-graphql/./src/index.ts?");

/***/ }),

/***/ "./src/monaco-graphql/src/api.ts":
/*!***************************************!*\
  !*** ./src/monaco-graphql/src/api.ts ***!
  \***************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   MonacoGraphQLAPI: () => (/* binding */ MonacoGraphQLAPI),\n/* harmony export */   completionSettingDefault: () => (/* binding */ completionSettingDefault),\n/* harmony export */   create: () => (/* binding */ create),\n/* harmony export */   diagnosticSettingDefault: () => (/* binding */ diagnosticSettingDefault),\n/* harmony export */   formattingDefaults: () => (/* binding */ formattingDefaults),\n/* harmony export */   modeConfigurationDefault: () => (/* binding */ modeConfigurationDefault)\n/* harmony export */ });\n/**\n *  Copyright (c) 2021 GraphQL Contributors.\n *\n *  This source code is licensed under the MIT license found in the\n *  LICENSE file in the root directory of this source tree.\n */\nclass MonacoGraphQLAPI {\n    _onDidChange = new monaco.Emitter();\n    _formattingOptions;\n    _modeConfiguration;\n    _diagnosticSettings;\n    _completionSettings;\n    _schemas = null;\n    _schemasById = Object.create(null);\n    _languageId;\n    _externalFragmentDefinitions = null;\n    constructor({ languageId, schemas, modeConfiguration, formattingOptions, diagnosticSettings, completionSettings, }) {\n        this._languageId = languageId;\n        if (schemas) {\n            this.setSchemaConfig(schemas);\n        }\n        this._modeConfiguration = modeConfiguration ?? modeConfigurationDefault;\n        this._completionSettings = completionSettings ?? completionSettingDefault;\n        this._diagnosticSettings = diagnosticSettings ?? diagnosticSettingDefault;\n        this._formattingOptions = formattingOptions ?? formattingDefaults;\n    }\n    get onDidChange() {\n        return this._onDidChange.event;\n    }\n    get languageId() {\n        return this._languageId;\n    }\n    get modeConfiguration() {\n        return this._modeConfiguration;\n    }\n    get schemas() {\n        return this._schemas;\n    }\n    schemasById() {\n        return this._schemasById;\n    }\n    get formattingOptions() {\n        return this._formattingOptions;\n    }\n    get diagnosticSettings() {\n        return this._diagnosticSettings;\n    }\n    get completionSettings() {\n        return this._completionSettings;\n    }\n    get externalFragmentDefinitions() {\n        return this._externalFragmentDefinitions;\n    }\n    /**\n     * override all schema config.\n     *\n     * @param schemas {SchemaConfig[]}\n     */\n    setSchemaConfig(schemas) {\n        this._schemas = schemas || null;\n        this._schemasById = schemas.reduce((result, schema) => {\n            result[schema.uri] = schema;\n            return result;\n        }, Object.create(null));\n        this._onDidChange.fire(this);\n    }\n    setExternalFragmentDefinitions(externalFragmentDefinitions) {\n        this._externalFragmentDefinitions = externalFragmentDefinitions;\n    }\n    setModeConfiguration(modeConfiguration) {\n        this._modeConfiguration = modeConfiguration || Object.create(null);\n        this._onDidChange.fire(this);\n    }\n    setFormattingOptions(formattingOptions) {\n        this._formattingOptions = formattingOptions || Object.create(null);\n        this._onDidChange.fire(this);\n    }\n    setDiagnosticSettings(diagnosticSettings) {\n        this._diagnosticSettings = diagnosticSettings || Object.create(null);\n        this._onDidChange.fire(this);\n    }\n    setCompletionSettings(completionSettings) {\n        this._completionSettings = completionSettings || Object.create(null);\n        this._onDidChange.fire(this);\n    }\n}\nfunction create(languageId, config) {\n    if (!config) {\n        return new MonacoGraphQLAPI({\n            languageId,\n            schemas: [],\n            formattingOptions: formattingDefaults,\n            modeConfiguration: modeConfigurationDefault,\n            diagnosticSettings: diagnosticSettingDefault,\n            completionSettings: completionSettingDefault,\n        });\n    }\n    const { schemas, formattingOptions, modeConfiguration, diagnosticSettings, completionSettings, } = config;\n    return new MonacoGraphQLAPI({\n        languageId,\n        schemas,\n        formattingOptions: {\n            ...formattingDefaults,\n            ...formattingOptions,\n            prettierConfig: {\n                ...formattingDefaults.prettierConfig,\n                ...formattingOptions?.prettierConfig,\n            },\n        },\n        modeConfiguration: {\n            ...modeConfigurationDefault,\n            ...modeConfiguration,\n        },\n        diagnosticSettings: {\n            ...diagnosticSettingDefault,\n            ...diagnosticSettings,\n        },\n        completionSettings: {\n            ...completionSettingDefault,\n            ...completionSettings,\n        },\n    });\n}\nconst modeConfigurationDefault = {\n    documentFormattingEdits: true,\n    documentRangeFormattingEdits: false,\n    completionItems: true,\n    hovers: true,\n    documentSymbols: false,\n    tokens: false,\n    colors: false,\n    foldingRanges: false,\n    diagnostics: true,\n    selectionRanges: false,\n};\nconst formattingDefaults = {\n    prettierConfig: {\n        // rationale? a11y.\n        // https://adamtuttle.codes/blog/2021/tabs-vs-spaces-its-an-accessibility-issue/\n        tabWidth: 2,\n    },\n};\nconst diagnosticSettingDefault = {\n    jsonDiagnosticSettings: {\n        schemaValidation: \"error\",\n    },\n};\nconst completionSettingDefault = {\n    __experimental__fillLeafsOnComplete: false,\n};\n\n\n//# sourceURL=webpack://narcolepsy-graphql/./src/monaco-graphql/src/api.ts?");

/***/ }),

/***/ "./src/monaco-graphql/src/initialize.ts":
/*!**********************************************!*\
  !*** ./src/monaco-graphql/src/initialize.ts ***!
  \**********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   LANGUAGE_ID: () => (/* binding */ LANGUAGE_ID),\n/* harmony export */   initializeMode: () => (/* binding */ initializeMode)\n/* harmony export */ });\n/* harmony import */ var _api__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./api */ \"./src/monaco-graphql/src/api.ts\");\n/**\n *  Copyright (c) 2021 GraphQL Contributors.\n *\n *  This source code is licensed under the MIT license found in the\n *  LICENSE file in the root directory of this source tree.\n */\n\nconst LANGUAGE_ID = \"graphql\";\nlet api;\n/**\n * Initialize the mode & worker synchronously with provided configuration\n *\n * @param config\n * @returns {MonacoGraphQLAPI}\n */\nfunction initializeMode(config) {\n    if (!api) {\n        api = (0,_api__WEBPACK_IMPORTED_MODULE_0__.create)(LANGUAGE_ID, config);\n        monaco.languages.graphql = { api };\n        // export to the global monaco API\n        // eslint-disable-next-line promise/prefer-await-to-then -- ignore to leave initializeMode sync\n        void getMode().then((mode) => mode.setupMode(api));\n    }\n    return api;\n}\nfunction getMode() {\n    return Promise.all(/*! import() */[__webpack_require__.e(\"vendors-node_modules_graphql-language-service_esm_index_js-node_modules_graphql_utilities_bui-44c47b\"), __webpack_require__.e(\"src_monaco-graphql_src_graphqlMode_ts\")]).then(__webpack_require__.bind(__webpack_require__, /*! ./graphqlMode */ \"./src/monaco-graphql/src/graphqlMode.ts\"));\n}\n\n\n//# sourceURL=webpack://narcolepsy-graphql/./src/monaco-graphql/src/initialize.ts?");

/***/ }),

/***/ "./src/monaco-graphql/src/initializeMode.ts":
/*!**************************************************!*\
  !*** ./src/monaco-graphql/src/initializeMode.ts ***!
  \**************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   LANGUAGE_ID: () => (/* reexport safe */ _initialize__WEBPACK_IMPORTED_MODULE_0__.LANGUAGE_ID),\n/* harmony export */   initializeMode: () => (/* reexport safe */ _initialize__WEBPACK_IMPORTED_MODULE_0__.initializeMode)\n/* harmony export */ });\n/* harmony import */ var _initialize__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./initialize */ \"./src/monaco-graphql/src/initialize.ts\");\n\n\n\n//# sourceURL=webpack://narcolepsy-graphql/./src/monaco-graphql/src/initializeMode.ts?");

/***/ }),

/***/ "./src/monaco-graphql/src/monaco.contribution.ts":
/*!*******************************************************!*\
  !*** ./src/monaco-graphql/src/monaco.contribution.ts ***!
  \*******************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   LANGUAGE_ID: () => (/* reexport safe */ _initializeMode__WEBPACK_IMPORTED_MODULE_1__.LANGUAGE_ID),\n/* harmony export */   MonacoGraphQLAPI: () => (/* reexport safe */ _api__WEBPACK_IMPORTED_MODULE_0__.MonacoGraphQLAPI),\n/* harmony export */   diagnosticSettingDefault: () => (/* reexport safe */ _api__WEBPACK_IMPORTED_MODULE_0__.diagnosticSettingDefault),\n/* harmony export */   formattingDefaults: () => (/* reexport safe */ _api__WEBPACK_IMPORTED_MODULE_0__.formattingDefaults),\n/* harmony export */   modeConfigurationDefault: () => (/* reexport safe */ _api__WEBPACK_IMPORTED_MODULE_0__.modeConfigurationDefault)\n/* harmony export */ });\n/* harmony import */ var _api__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./api */ \"./src/monaco-graphql/src/api.ts\");\n/* harmony import */ var _initializeMode__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./initializeMode */ \"./src/monaco-graphql/src/initializeMode.ts\");\n/* harmony import */ var _typings__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./typings */ \"./src/monaco-graphql/src/typings/index.ts\");\n/**\n *  Copyright (c) 2021 GraphQL Contributors.\n *\n *  This source code is licensed under the MIT license found in the\n *  LICENSE file in the root directory of this source tree.\n */\n\n\n\n\n// here is the only place where we\n// initialize the mode `onLanguage`\nmonaco.languages.onLanguage(_initializeMode__WEBPACK_IMPORTED_MODULE_1__.LANGUAGE_ID, () => {\n    const api = (0,_initializeMode__WEBPACK_IMPORTED_MODULE_1__.initializeMode)();\n    monaco.languages.graphql = { api };\n});\n\n\n//# sourceURL=webpack://narcolepsy-graphql/./src/monaco-graphql/src/monaco.contribution.ts?");

/***/ }),

/***/ "./src/monaco-graphql/src/typings/index.ts":
/*!*************************************************!*\
  !*** ./src/monaco-graphql/src/typings/index.ts ***!
  \*************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n\n\n\n//# sourceURL=webpack://narcolepsy-graphql/./src/monaco-graphql/src/typings/index.ts?");

/***/ }),

/***/ "./node_modules/graphql/utilities/getIntrospectionQuery.mjs":
/*!******************************************************************!*\
  !*** ./node_modules/graphql/utilities/getIntrospectionQuery.mjs ***!
  \******************************************************************/
/***/ ((__unused_webpack___webpack_module__, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   getIntrospectionQuery: () => (/* binding */ getIntrospectionQuery)\n/* harmony export */ });\n/**\n * Produce the GraphQL query recommended for a full schema introspection.\n * Accepts optional IntrospectionOptions.\n */\nfunction getIntrospectionQuery(options) {\n  const optionsWithDefault = {\n    descriptions: true,\n    specifiedByUrl: false,\n    directiveIsRepeatable: false,\n    schemaDescription: false,\n    inputValueDeprecation: false,\n    ...options,\n  };\n  const descriptions = optionsWithDefault.descriptions ? 'description' : '';\n  const specifiedByUrl = optionsWithDefault.specifiedByUrl\n    ? 'specifiedByURL'\n    : '';\n  const directiveIsRepeatable = optionsWithDefault.directiveIsRepeatable\n    ? 'isRepeatable'\n    : '';\n  const schemaDescription = optionsWithDefault.schemaDescription\n    ? descriptions\n    : '';\n\n  function inputDeprecation(str) {\n    return optionsWithDefault.inputValueDeprecation ? str : '';\n  }\n\n  return `\n    query IntrospectionQuery {\n      __schema {\n        ${schemaDescription}\n        queryType { name }\n        mutationType { name }\n        subscriptionType { name }\n        types {\n          ...FullType\n        }\n        directives {\n          name\n          ${descriptions}\n          ${directiveIsRepeatable}\n          locations\n          args${inputDeprecation('(includeDeprecated: true)')} {\n            ...InputValue\n          }\n        }\n      }\n    }\n\n    fragment FullType on __Type {\n      kind\n      name\n      ${descriptions}\n      ${specifiedByUrl}\n      fields(includeDeprecated: true) {\n        name\n        ${descriptions}\n        args${inputDeprecation('(includeDeprecated: true)')} {\n          ...InputValue\n        }\n        type {\n          ...TypeRef\n        }\n        isDeprecated\n        deprecationReason\n      }\n      inputFields${inputDeprecation('(includeDeprecated: true)')} {\n        ...InputValue\n      }\n      interfaces {\n        ...TypeRef\n      }\n      enumValues(includeDeprecated: true) {\n        name\n        ${descriptions}\n        isDeprecated\n        deprecationReason\n      }\n      possibleTypes {\n        ...TypeRef\n      }\n    }\n\n    fragment InputValue on __InputValue {\n      name\n      ${descriptions}\n      type { ...TypeRef }\n      defaultValue\n      ${inputDeprecation('isDeprecated')}\n      ${inputDeprecation('deprecationReason')}\n    }\n\n    fragment TypeRef on __Type {\n      kind\n      name\n      ofType {\n        kind\n        name\n        ofType {\n          kind\n          name\n          ofType {\n            kind\n            name\n            ofType {\n              kind\n              name\n              ofType {\n                kind\n                name\n                ofType {\n                  kind\n                  name\n                  ofType {\n                    kind\n                    name\n                  }\n                }\n              }\n            }\n          }\n        }\n      }\n    }\n  `;\n}\n\n\n//# sourceURL=webpack://narcolepsy-graphql/./node_modules/graphql/utilities/getIntrospectionQuery.mjs?");

/***/ })

/******/ });
/************************************************************************/
/******/ // The module cache
/******/ var __webpack_module_cache__ = {};
/******/ 
/******/ // The require function
/******/ function __webpack_require__(moduleId) {
/******/ 	// Check if module is in cache
/******/ 	var cachedModule = __webpack_module_cache__[moduleId];
/******/ 	if (cachedModule !== undefined) {
/******/ 		return cachedModule.exports;
/******/ 	}
/******/ 	// Create a new module (and put it into the cache)
/******/ 	var module = __webpack_module_cache__[moduleId] = {
/******/ 		id: moduleId,
/******/ 		// no module.loaded needed
/******/ 		exports: {}
/******/ 	};
/******/ 
/******/ 	// Execute the module function
/******/ 	__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 
/******/ 	// Return the exports of the module
/******/ 	return module.exports;
/******/ }
/******/ 
/******/ // expose the modules object (__webpack_modules__)
/******/ __webpack_require__.m = __webpack_modules__;
/******/ 
/************************************************************************/
/******/ /* webpack/runtime/amd options */
/******/ (() => {
/******/ 	__webpack_require__.amdO = {};
/******/ })();
/******/ 
/******/ /* webpack/runtime/compat get default export */
/******/ (() => {
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = (module) => {
/******/ 		var getter = module && module.__esModule ?
/******/ 			() => (module['default']) :
/******/ 			() => (module);
/******/ 		__webpack_require__.d(getter, { a: getter });
/******/ 		return getter;
/******/ 	};
/******/ })();
/******/ 
/******/ /* webpack/runtime/define property getters */
/******/ (() => {
/******/ 	// define getter functions for harmony exports
/******/ 	__webpack_require__.d = (exports, definition) => {
/******/ 		for(var key in definition) {
/******/ 			if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 				Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 			}
/******/ 		}
/******/ 	};
/******/ })();
/******/ 
/******/ /* webpack/runtime/ensure chunk */
/******/ (() => {
/******/ 	__webpack_require__.f = {};
/******/ 	// This file contains only the entry chunk.
/******/ 	// The chunk loading function for additional chunks
/******/ 	__webpack_require__.e = (chunkId) => {
/******/ 		return Promise.all(Object.keys(__webpack_require__.f).reduce((promises, key) => {
/******/ 			__webpack_require__.f[key](chunkId, promises);
/******/ 			return promises;
/******/ 		}, []));
/******/ 	};
/******/ })();
/******/ 
/******/ /* webpack/runtime/get javascript chunk filename */
/******/ (() => {
/******/ 	// This function allow to reference async chunks
/******/ 	__webpack_require__.u = (chunkId) => {
/******/ 		// return url for filenames based on template
/******/ 		return "" + chunkId + ".js";
/******/ 	};
/******/ })();
/******/ 
/******/ /* webpack/runtime/global */
/******/ (() => {
/******/ 	__webpack_require__.g = (function() {
/******/ 		if (typeof globalThis === 'object') return globalThis;
/******/ 		try {
/******/ 			return this || new Function('return this')();
/******/ 		} catch (e) {
/******/ 			if (typeof window === 'object') return window;
/******/ 		}
/******/ 	})();
/******/ })();
/******/ 
/******/ /* webpack/runtime/hasOwnProperty shorthand */
/******/ (() => {
/******/ 	__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ })();
/******/ 
/******/ /* webpack/runtime/load script */
/******/ (() => {
/******/ 	var inProgress = {};
/******/ 	var dataWebpackPrefix = "narcolepsy-graphql:";
/******/ 	// loadScript function to load a script via script tag
/******/ 	__webpack_require__.l = (url, done, key, chunkId) => {
/******/ 		if(inProgress[url]) { inProgress[url].push(done); return; }
/******/ 		var script, needAttach;
/******/ 		if(key !== undefined) {
/******/ 			var scripts = document.getElementsByTagName("script");
/******/ 			for(var i = 0; i < scripts.length; i++) {
/******/ 				var s = scripts[i];
/******/ 				if(s.getAttribute("src") == url || s.getAttribute("data-webpack") == dataWebpackPrefix + key) { script = s; break; }
/******/ 			}
/******/ 		}
/******/ 		if(!script) {
/******/ 			needAttach = true;
/******/ 			script = document.createElement('script');
/******/ 			script.type = "module";
/******/ 			script.charset = 'utf-8';
/******/ 			script.timeout = 120;
/******/ 			if (__webpack_require__.nc) {
/******/ 				script.setAttribute("nonce", __webpack_require__.nc);
/******/ 			}
/******/ 			script.setAttribute("data-webpack", dataWebpackPrefix + key);
/******/ 	
/******/ 			script.src = url;
/******/ 		}
/******/ 		inProgress[url] = [done];
/******/ 		var onScriptComplete = (prev, event) => {
/******/ 			// avoid mem leaks in IE.
/******/ 			script.onerror = script.onload = null;
/******/ 			clearTimeout(timeout);
/******/ 			var doneFns = inProgress[url];
/******/ 			delete inProgress[url];
/******/ 			script.parentNode && script.parentNode.removeChild(script);
/******/ 			doneFns && doneFns.forEach((fn) => (fn(event)));
/******/ 			if(prev) return prev(event);
/******/ 		}
/******/ 		var timeout = setTimeout(onScriptComplete.bind(null, undefined, { type: 'timeout', target: script }), 120000);
/******/ 		script.onerror = onScriptComplete.bind(null, script.onerror);
/******/ 		script.onload = onScriptComplete.bind(null, script.onload);
/******/ 		needAttach && document.head.appendChild(script);
/******/ 	};
/******/ })();
/******/ 
/******/ /* webpack/runtime/make namespace object */
/******/ (() => {
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = (exports) => {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/ })();
/******/ 
/******/ /* webpack/runtime/publicPath */
/******/ (() => {
/******/ 	__webpack_require__.p = "/_content/Narcolepsy.GraphQL/script/";
/******/ })();
/******/ 
/******/ /* webpack/runtime/jsonp chunk loading */
/******/ (() => {
/******/ 	// no baseURI
/******/ 	
/******/ 	// object to store loaded and loading chunks
/******/ 	// undefined = chunk not loaded, null = chunk preloaded/prefetched
/******/ 	// [resolve, reject, Promise] = chunk loading, 0 = chunk loaded
/******/ 	var installedChunks = {
/******/ 		"index": 0
/******/ 	};
/******/ 	
/******/ 	__webpack_require__.f.j = (chunkId, promises) => {
/******/ 			// JSONP chunk loading for javascript
/******/ 			var installedChunkData = __webpack_require__.o(installedChunks, chunkId) ? installedChunks[chunkId] : undefined;
/******/ 			if(installedChunkData !== 0) { // 0 means "already installed".
/******/ 	
/******/ 				// a Promise means "currently loading".
/******/ 				if(installedChunkData) {
/******/ 					promises.push(installedChunkData[2]);
/******/ 				} else {
/******/ 					if(true) { // all chunks have JS
/******/ 						// setup Promise in chunk cache
/******/ 						var promise = new Promise((resolve, reject) => (installedChunkData = installedChunks[chunkId] = [resolve, reject]));
/******/ 						promises.push(installedChunkData[2] = promise);
/******/ 	
/******/ 						// start chunk loading
/******/ 						var url = __webpack_require__.p + __webpack_require__.u(chunkId);
/******/ 						// create error before stack unwound to get useful stacktrace later
/******/ 						var error = new Error();
/******/ 						var loadingEnded = (event) => {
/******/ 							if(__webpack_require__.o(installedChunks, chunkId)) {
/******/ 								installedChunkData = installedChunks[chunkId];
/******/ 								if(installedChunkData !== 0) installedChunks[chunkId] = undefined;
/******/ 								if(installedChunkData) {
/******/ 									var errorType = event && (event.type === 'load' ? 'missing' : event.type);
/******/ 									var realSrc = event && event.target && event.target.src;
/******/ 									error.message = 'Loading chunk ' + chunkId + ' failed.\n(' + errorType + ': ' + realSrc + ')';
/******/ 									error.name = 'ChunkLoadError';
/******/ 									error.type = errorType;
/******/ 									error.request = realSrc;
/******/ 									installedChunkData[1](error);
/******/ 								}
/******/ 							}
/******/ 						};
/******/ 						__webpack_require__.l(url, loadingEnded, "chunk-" + chunkId, chunkId);
/******/ 					}
/******/ 				}
/******/ 			}
/******/ 	};
/******/ 	
/******/ 	// no prefetching
/******/ 	
/******/ 	// no preloaded
/******/ 	
/******/ 	// no HMR
/******/ 	
/******/ 	// no HMR manifest
/******/ 	
/******/ 	// no on chunks loaded
/******/ 	
/******/ 	// install a JSONP callback for chunk loading
/******/ 	var webpackJsonpCallback = (parentChunkLoadingFunction, data) => {
/******/ 		var [chunkIds, moreModules, runtime] = data;
/******/ 		// add "moreModules" to the modules object,
/******/ 		// then flag all "chunkIds" as loaded and fire callback
/******/ 		var moduleId, chunkId, i = 0;
/******/ 		if(chunkIds.some((id) => (installedChunks[id] !== 0))) {
/******/ 			for(moduleId in moreModules) {
/******/ 				if(__webpack_require__.o(moreModules, moduleId)) {
/******/ 					__webpack_require__.m[moduleId] = moreModules[moduleId];
/******/ 				}
/******/ 			}
/******/ 			if(runtime) var result = runtime(__webpack_require__);
/******/ 		}
/******/ 		if(parentChunkLoadingFunction) parentChunkLoadingFunction(data);
/******/ 		for(;i < chunkIds.length; i++) {
/******/ 			chunkId = chunkIds[i];
/******/ 			if(__webpack_require__.o(installedChunks, chunkId) && installedChunks[chunkId]) {
/******/ 				installedChunks[chunkId][0]();
/******/ 			}
/******/ 			installedChunks[chunkId] = 0;
/******/ 		}
/******/ 	
/******/ 	}
/******/ 	
/******/ 	var chunkLoadingGlobal = self["webpackChunknarcolepsy_graphql"] = self["webpackChunknarcolepsy_graphql"] || [];
/******/ 	chunkLoadingGlobal.forEach(webpackJsonpCallback.bind(null, 0));
/******/ 	chunkLoadingGlobal.push = webpackJsonpCallback.bind(null, chunkLoadingGlobal.push.bind(chunkLoadingGlobal));
/******/ })();
/******/ 
/******/ /* webpack/runtime/nonce */
/******/ (() => {
/******/ 	__webpack_require__.nc = undefined;
/******/ })();
/******/ 
/************************************************************************/
/******/ 
/******/ // startup
/******/ // Load entry module and return exports
/******/ // This entry module can't be inlined because the eval devtool is used.
/******/ var __webpack_exports__ = __webpack_require__("./src/index.ts");
/******/ var __webpack_exports__initializeGraphQL = __webpack_exports__.initializeGraphQL;
/******/ var __webpack_exports__setSchema = __webpack_exports__.setSchema;
/******/ export { __webpack_exports__initializeGraphQL as initializeGraphQL, __webpack_exports__setSchema as setSchema };
/******/ 
