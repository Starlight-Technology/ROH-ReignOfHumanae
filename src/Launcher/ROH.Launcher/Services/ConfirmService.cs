using System;
using System.Threading.Tasks;

namespace ROH.Launcher.Services
{
    public class ConfirmRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        internal TaskCompletionSource<bool> Tcs { get; } = new TaskCompletionSource<bool>();
    }

    public class ConfirmService
    {
        public event Action<ConfirmRequest>? OnConfirmRequested;

        public Task<bool> RequestConfirm(string title, string message)
        {
            if (OnConfirmRequested is null)
                return Task.FromResult(false);

            var req = new ConfirmRequest { Title = title, Message = message };
            OnConfirmRequested?.Invoke(req);
            return req.Tcs.Task;
        }
    }
}
