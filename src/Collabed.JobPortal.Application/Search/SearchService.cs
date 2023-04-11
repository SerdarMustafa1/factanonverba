using Azure;
using Azure.Maps.Search;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Collabed.JobPortal.Search
{
    public class SearchService : ISearchService, ITransientDependency
    {
        private readonly MapsSearchOptions _options;
        private readonly MapsSearchClient _mapsSearchClient;

        public SearchService(IOptions<MapsSearchOptions> options)
        {
            _options = options.Value;
            var credential = new AzureKeyCredential(_options.SubscriptionKey);
            _mapsSearchClient = new MapsSearchClient(credential);
        }

        public async Task<MapSearchResult> GetLocationCoordinatesAsync(string searchTerm, CancellationToken cancellationToken)
        {
            var result = new MapSearchResult();

            var options = new FuzzySearchOptions
            {
                CountryFilter = new[] { "GB" },
                Top = 1,
            };
            var searchResult = await _mapsSearchClient.FuzzySearchAsync(searchTerm, options, cancellationToken);

            if (searchResult?.Value.TotalResults > 0)
            {
                result.ResultsFound = true;
                var position = searchResult.Value.Results.FirstOrDefault().Position;
                result.Latitude = (decimal)position.Latitude;
                result.Longitude = (decimal)position.Longitude;
            }

            return result;
        }
    }
}
