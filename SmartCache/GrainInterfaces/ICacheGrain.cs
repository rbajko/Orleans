using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface ICacheGrain : IGrainWithStringKey
    {
        Task<string> AddEmail(string email);

        Task<string> GetEmail(string email);
    }
}