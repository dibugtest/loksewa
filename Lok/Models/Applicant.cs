using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lok.Models
{
    public class Applicant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public PersonalInfo PersonalInformation { get; set; }
        public ExtraInfo ExtraInformation { get; set; }
        public ContactInfo ContactInformation { get; set; }
        public IEnumerable<EducationInfo> EducationInfos { get; set; }
        public IEnumerable<TrainingInfo> TrainingInfos { get; set; }
        public IEnumerable<ProfessionalCouncil> ProfessionalCouncils { get; set; }
        public IEnumerable<GovernmentExperienceInfo> GovernmentInfos { get; set; }
        public IEnumerable<NonGovernmentExperienceInfo> NonGovernmentInfos { get; set; }
        public SecurityQA QuestionAnswer { get; set; }
        public Passwords Password { get; set; }
        public Upload Uploads { get; set; }
        public bool Verified { get; set; }
        public bool EmailVerification { get; set; }
        public bool MailSent { get; set; }
        public string RandomPassword { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EditedDate { get; set; }
        public string CreatedBy { get; set; }
    }
        public class PersonalInfo
        {
      
        public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string FirstNameNep { get; set; }
            public string MiddleNameNep { get; set; }
            public string LastNameNep { get; set; }
            public DateTime DOB { get; set; }
            public string DOBNep { get; set; }
            public string Sex { get; set; }
            public string FatherFirstName { get; set; }
            public string FatherMiddleName { get; set; }
            public string FatherLastName { get; set; }
            public string MotherFirstName { get; set; }
            public string MotherMiddleName { get; set; }
            public string MotherLastName { get; set; }
            public string GrandFatherFirstName { get; set; }
            public string GrandFatherMiddleName { get; set; }
            public string GrandFatherLastName { get; set; }
            public string SpouseFirstName { get; set; }
            public string SpouseMiddleName { get; set; }
            public string SpouseLastName { get; set; }
            public string CitizenshipNo { get; set; }
            public string CitizenshipValidDistrict { get; set; }
            public string CitizenshipValidDate { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public bool Verified { get; set; }
    }
    
    public class ExtraInfo {
       
        public string Cast { get; set; }
        public string Religion { get; set; }
        public string OtherReligion { get; set; }
        public string MaritalStatus { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentStatusOther { get; set; }
        public string MotherTongue { get; set; }
        public string DisabilityStatus { get; set; }
        public string Disability { get; set; }
        public string FathersLiteracy { get; set; }
        public string MothersLiteracy { get; set; }
        public string FamilyOccupation { get; set; }
        public string OtherOccupation { get; set; }
        public string Goegraphy { get; set; }
        public string NickName { get; set; }
        public string GroupName { get; set; }
        public string GroupNameOther { get; set; }
    }
    public class ContactInfo {
        public string CId { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string MunicipalityType { get; set; }
        public string MunicipalityName { get; set; }
        public string WardNo { get; set; }
        public string Tole { get; set; }
        public string Marga { get; set; }
        public string HouseNo { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }

    }
    public class EducationInfo {
        public string EId { get; set; }
        public string BoardName { get; set; }
        public string Level { get; set; }
        public string Faculty { get; set; }
        public string DivisionPercentage { get; set; }
        public string MainSubject { get; set; }
        public string DegreeName { get; set; }
        public string EducationType { get; set; }
        public string CompletedDate { get; set; }
        public string DateType { get; set; }
        public string FileName { get; set; }
        public string EquivalentFileName { get; set; }
    }
    public class TrainingInfo {
        public string TId { get; set; }
        public string OrganizationName { get; set; }
        public string TrainingName { get; set; }
        public string DivisionPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartDateNep { get; set; }
        public string EndDateNep { get; set; }
        public string FileName { get; set; }
    }
    public class ProfessionalCouncil {
        public string PId { get; set; }
        public string ProviderName { get; set; }
        public string Type { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime ValidateFrom { get; set; }
        public DateTime RenewDate { get; set; }
        public DateTime Validity { get; set; }
        public string ValidateFromNep { get; set; }
        public string RenewDateNep { get; set; }
        public string FileName { get; set; }
    }
    public class NonGovernmentExperienceInfo {
        public string GId { get; set; }
        public string OfficeName { get; set; }
            public string Post { get; set; }
            public string Level { get; set; }
            public string JobType { get; set; }
            public DateTime JobStartDate { get; set; }
            public DateTime JobEndDate { get; set; }
        public string FileName { get; set; }
    }
    public class GovernmentExperienceInfo {
        public string GId { get; set; }

        public string OfficeAddress { get; set; }
        public string OfficeName { get; set; }
        public string Post { get; set; }
        public string Sewa { get; set; }
        public string Samuha { get; set; }
        public string UpaSamuha { get; set; }
        public string TahaShreni { get; set; }
        public string Remarks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Awastha { get; set; }
        public string JobType { get; set; }
        public string FileName { get; set; }
    }
    public class Upload {
        public string Photograph { get; set; }
        public string Signature { get; set; }
        public string Citizenship { get; set; }
        public string InclusionGroup { get; set; }
       // public string ExperienceDocument { get; set; }
    }
    public class Passwords {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
    public class SecurityQA {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
    //public class Payment {
    //    public string AdvertisementCode { get; set; }
    //    public DateTime UploadedDate { get; set; }
    //    public string FileName { get; set; }
    //    public DateTime ExpiredDate { get; set; }
    //    public string PaymentStatus { get; set; }
    //}
}
