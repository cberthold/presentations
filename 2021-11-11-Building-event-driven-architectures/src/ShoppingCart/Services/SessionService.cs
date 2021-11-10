using Microsoft.AspNetCore.Http;
using ShoppingCart.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public class SessionService : ISessionService
    {
        public const string CONTEXT_KEY = "CONTEXT_KEY_CartId";

        private readonly IHttpContextAccessor contextAccessor;

        public SessionService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public async Task<Guid> AddOrLoadSessionId()
        {
            var session = await GetSession();
            if (string.IsNullOrEmpty(session.GetString(CONTEXT_KEY)))
            {
                await ResetSessionId();
            }
            return Guid.Parse(session.GetString(CONTEXT_KEY));
        }

        public async Task<Guid> ResetSessionId()
        {
            var newId = Guid.NewGuid();
            var session = await GetSession();
            session.SetString(CONTEXT_KEY, newId.ToString("N"));
            await session.CommitAsync();
            return newId;
        }

        private Task<ISession> GetSession()
        {
            var session = contextAccessor.HttpContext.Session;
            //await session.LoadAsync();
            return Task.FromResult(session);
        }
    }
}
