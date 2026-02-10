using System;

namespace E_Policy
{
    public class MessageException
    {
        public static Exception NoLogMessageException(string message)
        {
            throw new Exception(string.Format("{0}{1}", "[NOLOGMESSAGE]", message));
        }

        public static Exception HiddenMessageException(string message)
        {
            throw new Exception(string.Format("{0}{1}", "[HIDDENMESSAGE]", message));
        }
    }
}