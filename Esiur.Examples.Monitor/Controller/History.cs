using Esiur.Core;
using Esiur.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esiur.Examples.Monitor.Controller
{
    public class History : IResource
    {
        public Instance Instance { get; set; }

        public event DestroyedEvent OnDestroy;

        public void Destroy()
        {
            OnDestroy?.Invoke(this);
        }

        public AsyncReply<bool> Trigger(ResourceTrigger trigger)
        {
            if (trigger == ResourceTrigger.Initialize)           
                Console.WriteLine("History initialized");

            return new AsyncReply<bool>(true);
        }
    }
}
