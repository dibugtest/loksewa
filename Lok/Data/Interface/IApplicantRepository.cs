using Lok.Models;
using Lok.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Interface
{
    public interface IApplicantRepository : IRepository<Applicant>
    {
        Task<Applicant> GetByEmail(string Email);
        void UpdateEducationInfo(EducationInfo edu, string id,string EId);
        void UpdateTrainingInfo(TrainingInfo train, string id,string TId);
        void UpdateProfessionalCouncil(ProfessionalCouncil obj, string id,string PId);
        void UpdateGovernmentInfo(GovernmentExperienceInfo obj, string id);
        void UpdateNonGovernmentInfo(NonGovernmentExperienceInfo obj, string id);
    }
}
