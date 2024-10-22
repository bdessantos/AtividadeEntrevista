using System;

namespace FI.WebAtividadeEntrevista.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
