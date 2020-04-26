using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lok.Models;
using Lok.Data.Interface;
using AutoMapper;
using Lok.Extension;

namespace Lok.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LokSewaApiController : ControllerBase
    {
        private readonly IAuthinterface _auth;
       
        //private readonly IReligionRepository _Religion;
        //private readonly IEmploymentRepository _Employment;
        //private readonly IOccupationRepository _Occupation;
        //private readonly IVargaRepository _Varga;
        //private readonly IDistrictRepository _District;
        //private readonly IBoardNameRepository _BoardName;
        //private readonly IEducationLevelRepository _EducationLevel;
        //private readonly IFacultyRepository _Faculty;
        //private readonly ISewaRepository _Sewa;
        //private readonly IShreniTahaRepository _Shreni;
        //private readonly IAwasthaRepository _Awastha;
        private readonly IApplicationRepository _Applications;
        private readonly IAdvertisiment _Advertisement;
        private readonly IApplicantRepository _Applicant;

        private readonly IUnitOfWork _uow;
        public readonly IMapper _mapper;


        public LokSewaApiController(
            IApplicantRepository Applicant
            //,IReligionRepository Religion
            //                        , IEmploymentRepository Employment, IOccupationRepository Occupation,
            //                        IVargaRepository Varga, IDistrictRepository District, IBoardNameRepository BoardName, IEducationLevelRepository EducationLevel
            //                        , IFacultyRepository Faculty, ISewaRepository Sewa, IAwasthaRepository Awastha, IAuthinterface auth,
            //                        IShreniTahaRepository Shreni
            , IApplicationRepository Application
                                    ,IAdvertisiment Advertisement, IUnitOfWork uow, IMapper mapper)
        {
            _Applicant = Applicant;
            //_Religion = Religion;
            //_Employment = Employment;
            //_Occupation = Occupation;
            //_Varga = Varga;
            //_District = District;
            //_BoardName = BoardName;
            //_EducationLevel = EducationLevel;
            //_Faculty = Faculty;
            //_Sewa = Sewa;
            //_Shreni = Shreni;
            //_Awastha = Awastha;
            //_uow = uow;
            //_auth = auth;
            _Advertisement = Advertisement;
          _Applications = Application;
            _mapper = mapper;
        }

        // GET: api/LokSewaApi
        [Route("/api/LokSewaApi/GetEthnical/{id}")]
        [HttpPost]
        public async Task<IEnumerable<EthinicalGroup>> GetEthnical(string id)
        {
            Advertisiment ads = await _Advertisement.GetById(id);

            IEnumerable<EthinicalGroup> groups = ads.EthinicalGroups;

            return groups;
          
        }
        [Route("/api/LokSewaApi/GetApp/{id}")]
        [HttpPost]
        public async Task<Advertisiment> GetApplication(string id)
        {
            var identity = User.Identity;

            var Email = IdentityExtension.GetEmail(identity);
            Applicant applicant = await _Applicant.GetByEmail(Email);
           

            Applications apps = await _Applications.GetById(id);

            Advertisiment ads = await _Advertisement.GetById(apps.Advertisement);

            ads.EthinicalGroups = ads.EthinicalGroups.Where(m => apps.EthnicalGroup.Contains(m.Id.ToString())).ToList();

            return ads;

        }
        // GET: api/LokSewaApi/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LokSewaApi
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/LokSewaApi/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
