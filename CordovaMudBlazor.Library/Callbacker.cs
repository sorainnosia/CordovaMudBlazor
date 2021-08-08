using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

namespace CordovaMudBlazor.Library
{
    public class CallbackerResponse
    {
        public string[] arguments { get; private set; }
        public CallbackerResponse(string[] arguments)
        {
            this.arguments = arguments;
        }
        public T GetArg<T>(int i)
        {
            return JsonSerializer.Deserialize<T>(arguments[i]);
        }
    }

    public class Callbacker
    {
        private IJSRuntime _js = null;
        private DotNetObjectReference<Callbacker> _this = null;
        private Dictionary<string, Action<string[]>> _callbacks = new Dictionary<string, Action<string[]>>();

        public Callbacker(IJSRuntime JSRuntime)
        {
            _js = JSRuntime;
            _this = DotNetObjectReference.Create(this);
        }

        [JSInvokable]
        public void _Callback(string callbackId, string[] arguments)
        {
            if (_callbacks.TryGetValue(callbackId, out Action<string[]> callback))
            {
                _callbacks.Remove(callbackId);
                callback(arguments);
            }
        }

        public Task<CallbackerResponse> InvokeJS(string cmd, params object[] args)
        {
            var t = new TaskCompletionSource<CallbackerResponse>();
            _InvokeJS((string[] arguments) => {
                t.TrySetResult(new CallbackerResponse(arguments));
            }, cmd, args);
            return t.Task;
        }

        public void InvokeJS(Action<CallbackerResponse> callback, string cmd, params object[] args)
        {
            _InvokeJS((string[] arguments) => {
                callback(new CallbackerResponse(arguments));
            }, cmd, args);
        }

        private void _InvokeJS(Action<string[]> callback, string cmd, object[] args)
        {
            string callbackId;
            do
            {
                callbackId = Guid.NewGuid().ToString();
            } while (_callbacks.ContainsKey(callbackId));
            _callbacks[callbackId] = callback;
            _js.InvokeVoidAsync("window._callbacker", _this, "_Callback", callbackId, cmd, JsonSerializer.Serialize(args));
        }
    }
}

