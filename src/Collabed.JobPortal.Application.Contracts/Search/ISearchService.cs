using System.Threading;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Search
{
    public interface ISearchService
    {
        Task<MapSearchResult> GetLocationCoordinatesAsync(string searchTerm, CancellationToken cancellationToken);
    }
}
