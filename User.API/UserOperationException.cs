using System;

namespace User.API
{
    public class UserOperationException : Exception
    {
        public UserOperationException() { }
        public UserOperationException(string message) : base(message) { }
        public UserOperationException(string message, Exception inner) : base(message, inner) { }
    }
}