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
            CreateMap<IEnumerable<TrainingInfo>, IEnumerable<TrainingVM>>();

            CreateMap<EducationInfo, EducationVM>();
            CreateMap<IEnumerable<EducationVM>, IEnumerable<EducationInfo>>();
            CreateMap<EducationVM, EducationInfo>();

            CreateMap<GovernmentExperienceInfo, GovernmentExperienceVM>();
            CreateMap<GovernmentExperienceVM, GovernmentExperienceInfo>();

            CreateMap<IEnumerable<GovernmentExperienceInfo>, IEnumerable<GovernmentExperienceVM>>();
            CreateMap<IEnumerable<NonGovernmentExperienceInfo>, IEnumerable<NonGovernmentExperienceVM>>();
            CreateMap<NonGovernmentExperienceInfo ,NonGovernmentExperienceVM>();
            CreateMap<NonGovernmentExperienceVM, NonGovernmentExperienceInfo>();

            CreateMap<ProfessionalCouncil, ProfessionalCouncilVM>();
            CreateMap<ProfessionalCouncilVM, ProfessionalCouncil>();
            CreateMap<IEnumerable<ProfessionalCouncil>, IEnumerable<ProfessionalCouncilVM>>();

            
            CreateMap<RegistrationVM , PersonalInfo>();
            CreateMap<PersonalInfo ,RegistrationVM>();


            CreateMap<RegistrationVM, ExtraInfo>();
            CreateMap<ExtraInfo, RegistrationVM>();
            
            CreateMap<ResetPasswordVM, SecurityQA>();


        }
    }
}
