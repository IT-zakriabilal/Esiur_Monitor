using Esiur.Core;
using Esiur.Data;
using Esiur.Security.Authority;
using Esiur.Security.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esiur.Examples.Monitor.Model
{
    class Membership : IMembership
    {
        public bool GuestsAllowed => true;

        public AsyncReply<byte[]> GetPassword(string username, string domain)
        {
            return new AsyncReply<byte[]>(DC.ToBytes("secret"));
        }

        public AsyncReply<byte[]> GetToken(ulong tokenIndex, string domain)
        {
            throw new NotImplementedException();
        }

        public AsyncReply<bool> Login(Session session)
        {
            Console.WriteLine($"User {session.RemoteAuthentication.Username} logged in");
            return new AsyncReply<bool>(true);
        }

        public AsyncReply<bool> Logout(Session session)
        {
            Console.WriteLine($"User {session.RemoteAuthentication.Username} logged out");
            return new AsyncReply<bool>(true);
        }

        public AsyncReply<string> TokenExists(ulong tokenIndex, string domain)
        {
            throw new NotImplementedException();
        }

        public AsyncReply<bool> UserExists(string username, string domain)
        {
            return new AsyncReply<bool>(true);
        }
    }
}
