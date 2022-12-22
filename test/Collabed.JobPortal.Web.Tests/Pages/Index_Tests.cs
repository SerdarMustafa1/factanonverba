using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Collabed.JobPortal.Pages;

public class Index_Tests : JobPortalWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
