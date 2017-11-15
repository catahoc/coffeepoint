using System;
using System.Threading;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;

namespace coffeepoint.app.Model
{
    public class SingleAccessService
    {
        private readonly object safe = new object();

        public IDisposable StartSession()
        {
            Monitor.Enter(safe);
            return new Disposable(() => Monitor.Exit(safe));
        }
    }
}