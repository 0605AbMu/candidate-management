using AutoMapper;
using AutoMapper.QueryableExtensions;
using CM.WebApi.Brokers;
using CM.WebApi.Contracts.Candidate;
using CM.WebApi.Exceptions;
using CM.WebApi.Models;
using CM.WebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CM.WebApi.Services;

public class CandidateManager : ICandidateManager
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public CandidateManager(AppDbContext dbContext, IMapper mapper)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
    }

    public async Task<CandidateViewDto> CreateAsync(CreateCandidateDto dto)
    {
        var existedCandidate =
            await this._dbContext.Candidates.FirstOrDefaultAsync(x => x.Email.ToUpper() == dto.Email.ToUpper());

        var candidate = this._mapper.Map<Candidate>(dto);

        if (existedCandidate is not null)
        {
            var updatedCandidate = this._mapper.Map(candidate, existedCandidate);
            candidate = this._dbContext.Update(updatedCandidate).Entity;
        }
        else
        {
            candidate = this._dbContext.Candidates.Add(candidate).Entity;
        }

        await this._dbContext.SaveChangesAsync();

        return this._mapper.Map<CandidateViewDto>(candidate);
    }

    public async Task<IReadOnlyList<CandidateViewDto>> GetAll(int skip, int take)
    {
        return await this._dbContext.Candidates
            .OrderBy(x => x.Id)
            .Skip(skip)
            .Take(take)
            .ProjectTo<CandidateViewDto>(this._mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<CandidateViewDto> GetById(long id)
    {
        var candidate = await this._dbContext.Candidates
            .FirstOrDefaultAsync(x => x.Id == id);

        NotFoundException.ThrowIsNull(candidate);

        return this._mapper.Map<CandidateViewDto>(candidate);
    }

    public async Task<CandidateViewDto> DeleteById(long id)
    {
        var candidate = await this._dbContext.Candidates
            .FirstOrDefaultAsync(x => x.Id == id);

        NotFoundException.ThrowIsNull(candidate);

        this._dbContext.Candidates.Remove(candidate!);

        await this._dbContext.SaveChangesAsync();

        return this._mapper.Map<CandidateViewDto>(candidate);
    }
}
