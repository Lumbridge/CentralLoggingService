using System;

namespace CLS.Infrastructure.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetExceptionMessages(this Exception e, string messages = "")
        {
            if (e == null) return string.Empty;
            if (messages == "") messages = e.Message;
            if (e.InnerException != null)
                messages += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
            return messages;
        }
    }
}
