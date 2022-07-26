using Microsoft.JSInterop;

namespace Mittons.Azure.Devops.Extension.Xdm;

public static class MessageEventListener
{
    [JSInvokable("OnMessageEvent")]
    public static void OnMessageEvent(string data)
        => Channel.OnMessageEvent(data);
}
