using AutoMapper;
using CM.WebApi.Contracts.Candidate;
using CM.WebApi.Extensions.AutoMapper;

namespace CM.WebApi.Profiles.Candidate;

public class CandidateProfile : Profile
{
    public CandidateProfile()
    {
        this.CreateMap<Models.Candidate, CandidateViewDto>();

        this.CreateMap<CreateCandidateDto, Models.Candidate>();

        this.CreateMap<Models.Candidate, Models.Candidate>()
            .IgnoreAuditable();
    }
}
