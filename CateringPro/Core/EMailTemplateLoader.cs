using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TemplateLoaderAttribute : Attribute  
    {
        private readonly Type _loaderType;
        public TemplateLoaderAttribute(Type loaderType)
        {
            
            _loaderType = loaderType;
        }

        public virtual Type LoaderType { get => _loaderType; }
 

    }

    public enum EmailTemplateType : int
    {
        [TemplateLoader(typeof(InfoTemplateLoader))]
        Info = 1,
        [TemplateLoader(typeof(DayMenuTemplateLoader))]
        DayMenu = 2,
        [TemplateLoader(typeof(DayProductionTemplateLoader))]
        DayProduction=3,
        [TemplateLoader(typeof(UserDayOrderTemplateLoader))]
        UserOrderWeek = 4
    }
    public abstract class  EMailTemplateLoader
    {
        protected readonly IMassEmailRepository _mailRepo;
        protected readonly int _companyid;
        public EMailTemplateLoader(IMassEmailRepository mailRepo, int companyid)
        {
            _mailRepo = mailRepo;
            _companyid = companyid;
        }
        public abstract bool LoadModel(MassEmail em, EmailTemplateViewModel template);
        protected virtual void  DateCycle(MassEmail em, EmailTemplateViewModel template,Action<MassEmail, EmailTemplateViewModel,DateTime> action)
        {
            DateTime dayfrom = DateTime.Today.AddDays(em.DayFrom);
            DateTime dayto = DateTime.Today.AddDays(em.DayTo);
            for (DateTime dt = dayfrom; dt <= dayto; dt = dt.AddDays(1))
            {
                action(em, template, dt);
               
            }
        }
    }
    public class InfoTemplateLoader : EMailTemplateLoader
    {
        public InfoTemplateLoader(IMassEmailRepository mailRepo, int companyid) : base(mailRepo, companyid)
        {

        }
        public override bool LoadModel(MassEmail em, EmailTemplateViewModel template)
        {
            return true;
        }
    }
    public  class DayMenuTemplateLoader: EMailTemplateLoader
    {
        public DayMenuTemplateLoader(IMassEmailRepository mailRepo, int companyid):base(mailRepo, companyid)
        {

        }
        public override bool LoadModel(MassEmail em, EmailTemplateViewModel template)
        {
            this.DateCycle(em,template,(em, template, dt) => {
                template.Models.Add(dt, _mailRepo.ReportRepository.CompanyComplexMenu(dt, dt, _companyid));
            });
            /*
            DateTime dayfrom = DateTime.Today.AddDays(em.DayFrom);
            DateTime dayto = DateTime.Today.AddDays(em.DayTo);
            for (DateTime dt = dayfrom; dt<= dayto; dt = dt.AddDays(1)) {
                template.Models.Add(dt, _mailRepo.ReportRepository.CompanyComplexMenu(dt, dt, _companyid));
            }*/
            return true;
        }
    }
    public class DayProductionTemplateLoader : EMailTemplateLoader
    {
        public DayProductionTemplateLoader(IMassEmailRepository mailRepo, int companyid) : base(mailRepo, companyid)
        {

        }
        public override bool LoadModel(MassEmail em, EmailTemplateViewModel template)
        {
            this.DateCycle(em, template, (em, template, dt) => {
                template.Models.Add(dt, _mailRepo.ReportRepository.CompanyDayProduction(dt, _companyid));
            });

            return true;
        }
    }
    public class UserDayOrderTemplateLoader : EMailTemplateLoader
    {
        public UserDayOrderTemplateLoader(IMassEmailRepository mailRepo, int companyid) : base(mailRepo, companyid)
        {

        }
        public override bool LoadModel(MassEmail em, EmailTemplateViewModel template)
        {
            this.DateCycle(em, template, (em, template, dt) => {
                template.Models.Add(dt, _mailRepo.ReportRepository.CompanyDayProduction(dt, _companyid));
            });

            return true;
        }
    }
}
