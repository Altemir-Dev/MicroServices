using System;

namespace BancoBari.Contracts
{
    public class Contracts
    {
        public record HelloWorld(Guid Id, string Identifier, string Message, DateTimeOffset DateRequest);
        public record Hi(Guid Id, string Identifier, string Message, DateTimeOffset DateRequest);
    }
}