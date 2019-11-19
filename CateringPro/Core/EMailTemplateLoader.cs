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
        DayMenu = 2

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
            DateTime daydate = DateTime.Today.AddDays(1);
            template.Models.Add(daydate, _mailRepo.ReportRepository.CompanyComplexMenu(daydate, daydate, _companyid));
            return true;
        }
    }
}
