import { BrotliDecode } from './decode.min.js';

window.addEventListener("message", async event => {
    console.debug("receiving message", event);

    await DotNet.invokeMethodAsync("Mittons.Azure.Devops.Extension", "OnMessageEvent", event.data);
});

window.sendRpcMessage = (message) => {
    console.debug("sending message", message);

    window.parent.postMessage(message, "*");
};

const loadFramework = () => new Promise((resolve, reject) => {
    try {
        const element = document.createElement('script');

        element.setAttribute('src', 'framework/blazor.webassembly.js');
        element.setAttribute('autostart', 'false');
        element.addEventListener('load', (event) => {
            resolve(event);
        });
        element.addEventListener('error', (event) => {
            reject(event);
        });

        document.body.appendChild(element);
    } catch (error) {
        reject(error);
    }
});

const loadBootResource = async (type, _name, defaultUri, _integrity) => {
    const response = await fetch(defaultUri + '.br', { cache: 'no-cache' });
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    const originalResponseBuffer = await response.arrayBuffer();
    const originalResponseArray = new Int8Array(originalResponseBuffer);
    const decompressedResponseArray = BrotliDecode(originalResponseArray);
    const contentType = type ===
        'dotnetwasm' ? 'application/wasm' : 'application/octet-stream';
    return new Response(decompressedResponseArray,
        { headers: { 'content-type': contentType } });
};

await loadFramework();
await Blazor.start({ loadBootResource });