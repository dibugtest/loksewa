using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.ViewModel
{
    public class PreviewVM
    {
        public PersonalVM Personal { get; set; }
        public ExtraVM Extra { get; set; }
        public ContactVM Contact { get; set; }
        public UploadVM Upload { get; set; }
        public IEnumerable<EducationVM> Educations { get; set; }
        public IEnumerable<TrainingVM> Trainings { get; set; }
        public IEnumerable<ProfessionalCouncilVM> Councils { get; set; }
        public IEnumerable<GovernmentExperienceVM> Governments { get; set; }
        public IEnumerable<NonGovernmentExperienceVM> NonGovernments { get; set; }
    }
}
