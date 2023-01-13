﻿using AutoMapper;
using Collabed.JobPortal.Jobs;

namespace Collabed.JobPortal;

public class JobPortalApplicationAutoMapperProfile : Profile
{
    public JobPortalApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Jobs.Job, JobDto>();
        CreateMap<JobDto, CreateUpdateJobDto>();
    }
}
