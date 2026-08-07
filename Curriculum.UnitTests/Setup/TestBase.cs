using Curriculum.Infrastructure.Persistence;
using Curriculum.UnitTests.Fakes;

namespace Curriculum.UnitTests.Setup;

public class TestBase : IDisposable
{
    protected ICurriculumData CurriculumDataMock = NSubstitute.Substitute.For<ICurriculumData>();
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}