let dotNetChannelHelper = {
    invokeMethodAsync: async () => { }
};

const sendRpcMessage = (message) => {
    console.debug('sending rpc message', message);

    window.parent.postMessage(message, '*');
};

const receiveRpcMessage = async (event) => {
    console.debug('receiving rpc message', event);

    await dotNetChannelHelper.invokeMethodAsync("ReceiveRpcMessage", event.data);
};

const addRpcMessageListener = (helper) => {
    dotNetChannelHelper = helper;

    window.addEventListener('message', receiveRpcMessage);
};

const removeRpcMessageListener = () => {
    window.removeEventListener('message', receiveRpcMessage);
};

export { sendRpcMessage, addRpcMessageListener, removeRpcMessageListener };