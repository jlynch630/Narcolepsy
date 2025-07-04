﻿function injectScripts(scripts) {
	return Promise.all(scripts.split("\0").map(injectScript));
}

function injectScript(path) {
	const script = document.createElement("script");
	script.type = "text/javascript";
	script.src = path;
	return new Promise(r => {
		script.onload = () => {
			r();
		};
		document.head.appendChild(script);
	})
}

function injectStyle(path) {
	const link = document.createElement("link");
	link.rel = "stylesheet";
	link.href = path;

	document.head.appendChild(link);
}

function scrollToBottom(el) {
	el.scrollTop = el.scrollHeight;
}