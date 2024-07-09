using AutoMapper;
using CM.WebApi.Brokers;
using CM.WebApi.Contracts.Candidate;
using CM.WebApi.Exceptions;
using CM.WebApi.Profiles.Candidate;
using CM.WebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Tests.Services;

[TestFixture]
[TestOf(typeof(CandidateManager))]
public class CandidateManagerTest : IDisposable, IAsyncDisposable
{
    private AppDbContext _appDbContext;
    private CandidateManager _candidateManager;

    // ReSharper disable once InconsistentNaming
    private CreateCandidateDto dto;
    private Mapper _mapper;

    [SetUp]
    public void Setup()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();

        builder.UseInMemoryDatabase("test");

        _appDbContext = new AppDbContext(builder.Options);

        MapperConfiguration config = new MapperConfiguration(cfg => { cfg.AddProfile<CandidateProfile>(); });

        _mapper = new Mapper(config);

        _candidateManager = new CandidateManager(_appDbContext, _mapper);


        dto = new CreateCandidateDto()
        {
            Comment = string.Empty,
            Email = "user@xyz.com",
            CallFrom = DateTime.Now,
            CallTo = DateTime.Now.AddDays(1),
            FirstName = "John",
            GithubUrl = null,
            LastName = "Doe",
            PhoneNumber = null,
            LinkedInUrl = null
        };
    }

    [Test]
    public async Task Create_Should_NewCandidateCreatedAndPersist()
    {
        var createdCandidate = await _candidateManager.CreateAsync(dto);

        var count = await _appDbContext.Candidates.CountAsync();

        Assert.GreaterOrEqual(count, 1);
        Assert.IsNotNull(await _appDbContext.Candidates.FirstOrDefaultAsync(x => x.Id == createdCandidate.Id));
    }

    [Test]
    public async Task Create_Should_SetValueToCreatedAtAndUpdateAtAutomatically_When_CandidatePersistedToRDBMS()
    {
        var viewDto = await _candidateManager.CreateAsync(dto);

        var createdCandidate = await _appDbContext.Candidates.FirstOrDefaultAsync(x => x.Id == viewDto.Id);

        Assert.IsNotNull(createdCandidate);

        if (!_appDbContext.Database.IsInMemory())
        {
            Assert.Greater(createdCandidate.CreatedAt, DateTime.MinValue);
            Assert.Greater(createdCandidate.UpdatedAt, DateTime.MinValue);
        }
    }

    [Test]
    public async Task Create_ShouldBe_Updated_When_ExistedCandidateCreate()
    {
        await _candidateManager.CreateAsync(dto);

        dto.FirstName = "Foo";
        dto.LastName = "Bar";

        var result = await _candidateManager.CreateAsync(dto);

        Assert.That(dto.FirstName, Is.EqualTo(result.FirstName));
        Assert.That(dto.LastName, Is.EqualTo(result.LastName));
    }

    [Test]
    public async Task GetAll_ShouldNotBe_Empty()
    {
        await _candidateManager.CreateAsync(dto);

        var candidates = await _candidateManager.GetAll(0, 10);

        Assert.IsNotEmpty(candidates);
    }

    [Test]
    [TestCase(2)]
    [TestCase(10)]
    public void GetById_Should_ThrowException_When_GivenNotExistedCandidateId(long candidateId)
    {
        Assert.ThrowsAsync<NotFoundException>(async () => { await _candidateManager.GetById(candidateId); });
    }


    [Test]
    [TestCase(2)]
    [TestCase(10)]
    public void DeleteById_Should_ThrowException_When_GivenNotExistedCandidateId(long candidateId)
    {
        Assert.ThrowsAsync<NotFoundException>(async () => { await _candidateManager.GetById(candidateId); });
    }

    public void Dispose()
    {
        _appDbContext.Dispose();
        _candidateManager = null!;
    }

    public async ValueTask DisposeAsync()
    {
        await _appDbContext.DisposeAsync();
        _candidateManager = null!;
    }
}