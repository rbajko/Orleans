using System.Threading.Tasks;
using Orleans;
using System.Collections.Generic;
using Orleans.Providers;
using System;
using GrainInterfaces;

namespace GrainCollection
{
    public class CacheGrainState
    {
        public List<string> Emails = new List<string>();
    }

    [StorageProvider(ProviderName = "AzureBlobStore")]
    public class CacheGrain : Grain<CacheGrainState>, ICacheGrain
    {
        private bool stateChanged = false;

        // method adds new email to emails list in CacheGrainState 
        public Task<string> AddEmail(string email)
        {
            if (!State.Emails.Contains(email))
            {
                stateChanged = true;
                State.Emails.Add(email);
                return Task.FromResult("Created");
            }

            return Task.FromResult("Conflict");
        }

        // method checks if email is already present in emails list in CacheGrainState 
        public Task<string> GetEmail(string email)
        {
            if (State.Emails.Contains(email))
            {
                return Task.FromResult("OK");
            }

            return Task.FromResult("NotFound");
        }

        // method reads data from azure blob storage at the initialization of the grain
        public override Task OnActivateAsync()
        {
            RegisterTimer(StoreData, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
            return base.OnActivateAsync();
        }

        // method stores data in azure blob storage
        async Task StoreData(object obj)
        {
            if (!stateChanged)
            {
                return;
            }

            stateChanged = false;
            await base.WriteStateAsync();

            Console.WriteLine("Data stored!");
        }
    }
}