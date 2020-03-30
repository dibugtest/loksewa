using Lok.Data.Interface;
using Lok.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Data.Repository
{
    public class ApplicantRepository : Repository<Applicant>, IApplicantRepository
    {
      
        public ApplicantRepository(IMangoContext context) : base(context)
        {

        }
        public virtual async Task<Applicant> GetByEmail(string id)
        {
            IMongoCollection<Applicant> DbSet = Context.GetCollection<Applicant>(typeof(Applicant).Name);
            var data = await DbSet.FindAsync(Builders<Applicant>.Filter.Eq("PersonalInformation.Email", ObjectId.Parse(id)));
            return data.SingleOrDefault();
        }
        public virtual void UpdateEducationInfo(EducationInfo obj, string id)
        {
          
            DbSet.UpdateOneAsync(Builders<Applicant>.Filter.Eq("_id", ObjectId.Parse(id)), Builders<Applicant>.Update.Push("EducationInfos", obj));
            
        }
        public virtual void UpdateTrainingInfo(TrainingInfo obj, string id)
        {
           
            DbSet.UpdateOneAsync(Builders<Applicant>.Filter.Eq("_id", ObjectId.Parse(id)), Builders<Applicant>.Update.Push("TrainingInfos", obj));

        }

        public virtual void UpdateProfessionalCouncil(ProfessionalCouncil obj, string id)
        {
            DbSet.UpdateOneAsync(Builders<Applicant>.Filter.Eq("_id", ObjectId.Parse(id)), Builders<Applicant>.Update.Push("ProfessionalCouncils", obj));

        }

        public virtual void UpdateGovernmentInfo(GovernmentExperienceInfo obj, string id)
        {
            DbSet.UpdateOneAsync(Builders<Applicant>.Filter.Eq("_id", ObjectId.Parse(id)), Builders<Applicant>.Update.Push("GovernmentInfos", obj));

        }

        public virtual void UpdateNonGovernmentInfo(NonGovernmentExperienceInfo obj, string id)
        {
            DbSet.UpdateOneAsync(Builders<Applicant>.Filter.Eq("_id", ObjectId.Parse(id)), Builders<Applicant>.Update.Push("NonGovernmentInfos", obj));

        }

    }
}
