using Lok.Models;
using Lok.ViewModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonalInfo, PersonalVM>();
            CreateMap<PersonalVM, PersonalInfo>();

            CreateMap<ExtraInfo, ExtraVM>();
            CreateMap<ExtraVM, ExtraInfo>();

            CreateMap<ContactInfo, ContactVM>();
            CreateMap<ContactVM, ContactInfo>();

            CreateMap<TrainingInfo, TrainingVM>();
            CreateMap<TrainingVM, TrainingInfo>();

            CreateMap<EducationInfo, EducationVM>();
            CreateMap<EducationVM, EducationInfo>();

            CreateMap<GovernmentExperienceInfo, GovernmentExperienceVM>();
            CreateMap<GovernmentExperienceVM, GovernmentExperienceInfo>();

            CreateMap<NonGovernmentExperienceInfo ,NonGovernmentExperienceVM>();
            CreateMap<NonGovernmentExperienceVM, NonGovernmentExperienceInfo>();

            CreateMap<ProfessionalCouncil, ProfessionalCouncilVM>();
            CreateMap<ProfessionalCouncilVM, ProfessionalCouncil>();

            
            CreateMap<RegistrationVM , PersonalInfo>();
            CreateMap<PersonalInfo ,RegistrationVM>();


            CreateMap<RegistrationVM, ExtraInfo>();
            CreateMap<ExtraInfo, RegistrationVM>();

        }
    }
}
