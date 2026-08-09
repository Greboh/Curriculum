using Curriculum.Infrastructure.Persistence;
using NSubstitute.ClearExtensions;

namespace Curriculum.UnitTests.Setup;

public class TestBase : IDisposable
{
    protected ICurriculumData CurriculumDataMock = NSubstitute.Substitute.For<ICurriculumData>();
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        ClearMocks();
    }

    private void ClearMocks()
    {
        CurriculumDataMock.ClearSubstitute();
    }
}