using System;

namespace Project.Domain.Exceptions
{

    [System.Serializable]
    public class ProjectDomainException : Exception
    {
        public ProjectDomainException() { }
        public ProjectDomainException(string message) : base(message) { }
        public ProjectDomainException(string message, Exception inner) : base(message, inner) { }
    }
}
