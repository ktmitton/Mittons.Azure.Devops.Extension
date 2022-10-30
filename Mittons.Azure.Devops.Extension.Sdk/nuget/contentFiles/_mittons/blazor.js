import { BrotliDecode } from './decode.min.js';

const loadFramework = () => new Promise((resolve, reject) => {
    try {
        const element = document.createElement('script');

        element.setAttribute('src', '_framework/blazor.webassembly.js');
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


const loadBootResource = (type, _name, defaultUri, _integrity) => {
    if (type !== 'dotnetjs') {
        return (async function () {
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
        })();
    }
};

const setupWrappers = () => {
    // NOTE: We call window.importWrapper instead of just calling import directly because the dotnet
    //       javascript for some reason has a "cache" of import which manually converts ./<file>.js
    //       from the relative path to an explicit path. Since the base ref is never set, this
    //       does not behave as expected.
    window.importWrapper = (x) => import(x);
};

setupWrappers();
await loadFramework();
await Blazor.start({ loadBootResource });