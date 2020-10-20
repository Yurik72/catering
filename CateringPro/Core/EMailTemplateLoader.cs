using CateringPro.Core;
using CateringPro.Models;
using CateringPro.Repositories;
using CateringPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
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
        UserOrderWeek = 4,
        [TemplateLoader(typeof(CsvFlatExportLoader))]
        CsvFlatExport = 5,
        [TemplateLoader(typeof(ExcelExportLoader))]
        ExcelExport = 6
    }

    public class ExecutionModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int CompanyId { get; set; }

        public string UserFriendlyName { get; set; }

        public string UserChildFriendlyName { get; set; }

        public string ShortSQLDateFrom => DateFrom.ResetHMS().ShortSqlDate();
        public string ShortSQLDateTo => DateTo.ResetHMS().ShortSqlDate();

        
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
        public ExecutionModel ExecModel { get; protected set; }
        public abstract bool LoadModel(MassEmail em, EmailTemplateViewModel template, CompanyUser user);
        protected virtual void LoadBaseFeature(MassEmail em, EmailTemplateViewModel template, CompanyUser user)
        {
            ExecModel = GetExecutionModel(em, template, user);
            template.Subject = ReplaceMacro(em.Subject, ExecModel);
            template.Greeting = ReplaceMacro(em.Greetings, ExecModel);
        }
        protected virtual void  DateCycle(MassEmail em, EmailTemplateViewModel template,Action<MassEmail, EmailTemplateViewModel,DateTime> action)
        {
            DateTime dayfrom = DateTime.Today.AddDays(em.DayFrom);
            DateTime dayto = DateTime.Today.AddDays(em.DayTo);
            for (DateTime dt = dayfrom; dt <= dayto; dt = dt.AddDays(1))
            {
                action(em, template, dt);
               
            }
        }
        protected virtual ExecutionModel GetExecutionModel(MassEmail em, EmailTemplateViewModel template, CompanyUser user)
        {
            return new ExecutionModel()
            {
                DateFrom = DateTime.Today.AddDays(em.DayFrom),
                DateTo = DateTime.Today.AddDays(em.DayTo),
                CompanyId = _companyid,
                UserFriendlyName = user?.NameSurname,
                UserChildFriendlyName= user?.ChildNameSurname
            };
        }
         protected virtual string ReplaceMacro(string value, ExecutionModel exmodel)
        {
            try
            {
                return Regex.Replace(value, @"{(?<exp>[^}]+)}", match =>
                {
                    var p = Expression.Parameter(typeof(ExecutionModel), "exmodel");
                    var e = DynamicExpressionParser.ParseLambda(new[] { p }, null, match.Groups["exp"].Value);
                    // var e = DynamicExpression.ParseLambda(new[] { p }, null, match.Groups["exp"].Value);
                    return (e.Compile().DynamicInvoke(exmodel) ?? "").ToString();
                });
            }
            catch(Exception ex)
            {
                return value;
            }
        }
 
    }
    public class InfoTemplateLoader : EMailTemplateLoader
    {
        public InfoTemplateLoader(IMassEmailRepository mailRepo, int companyid) : base(mailRepo, companyid)
        {

        }
        public override bool LoadModel(MassEmail em, EmailTemplateViewModel template, CompanyUser user)
        {
            LoadBaseFeature(em, template, user);
            return true;
        }
    }
    public  class DayMenuTemplateLoader: EMailTemplateLoader
    {
        public DayMenuTemplateLoader(IMassEmailRepository mailRepo, int companyid):base(mailRepo, companyid)
        {

        }
        public override bool LoadModel(MassEmail em, EmailTemplateViewModel template, CompanyUser user)
        {
            LoadBaseFeature(em, template, user);
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
        public override bool LoadModel(MassEmail em, EmailTemplateViewModel template, CompanyUser user)
        {
            LoadBaseFeature(em, template, user);
            //template.Models.
            DateTime datefrom = DateTime.Now;
            datefrom = datefrom.AddDays(em.DayFrom);
            DateTime dateto = DateTime.Now;
            int add = em.DayFrom + em.DayTo;
            dateto = dateto.AddDays(add);
            if (datefrom.Ticks == 0)
            {
                datefrom = DateTime.Today;
            }
            if (dateto.Ticks == 0)
            {
                dateto = DateTime.Today.AddDays(3);
            }
            datefrom = datefrom.ResetHMS();
            dateto = dateto.ResetHMS();
            //this.DateCycle(em, template, (em, template, dt) => {
            //    template.Models.Add(dt, _mailRepo.ReportRepository.CompanyDayProduction(datefrom, dateto, _companyid));
            //});
            template.Models.Add(datefrom, _mailRepo.ReportRepository.CompanyDayProduction(datefrom, dateto, _companyid));

            return true;
        }
    }
    public class UserDayOrderTemplateLoader : EMailTemplateLoader
    {
        public UserDayOrderTemplateLoader(IMassEmailRepository mailRepo, int companyid) : base(mailRepo, companyid)
        {

        }
        public override bool LoadModel(MassEmail em, EmailTemplateViewModel template, CompanyUser user)
        {
            LoadBaseFeature(em, template, user);
            //this.DateCycle(em, template, (em, template, dt) => {
            //    template.Models.Add(dt, _mailRepo.ReportRepository.EmailWeekInvoice(dt, _companyid,user));
            //});
            DateTime dt = DateTime.Now;
            template.Models.Add(dt, _mailRepo.ReportRepository.EmailWeekInvoice(dt, _companyid, user));
            return true;
        }
    }
    public class CsvFlatExportLoader : EMailTemplateLoader
    {
        public CsvFlatExportLoader(IMassEmailRepository mailRepo, int companyid) : base(mailRepo, companyid)
        {

        }
        public override bool LoadModel(MassEmail em, EmailTemplateViewModel template, CompanyUser user)
        {
            LoadBaseFeature(em, template, user);
            //this.DateCycle(em, template, (em, template, dt) => {
            //    template.Models.Add(dt, _mailRepo.ReportRepository.EmailWeekInvoice(dt, _companyid,user));
            //});
          
            string sql = ReplaceMacro(em.SQLCommand, ExecModel);
            var attach = new EMailAttachment();
            attach.Content = _mailRepo.ProduceFlatCSV(sql).Result;
            attach.ContentType = "text/csv";
            attach.Name= ReplaceMacro(em.Name, ExecModel);
            attach.Name = Translit.cyr2lat(attach.Name) + ".csv";
            template.Attachments.Add(attach);
            template.JustAttachment = true;
           
            //template.Models.Add(dt, _mailRepo.ReportRepository.EmailWeekInvoice(dt, _companyid, user));
            return true;
        }

    }

public class ExcelExportLoader : EMailTemplateLoader
{
    public ExcelExportLoader(IMassEmailRepository mailRepo, int companyid) : base(mailRepo, companyid)
    {

    }
    public override bool LoadModel(MassEmail em, EmailTemplateViewModel template, CompanyUser user)
    {
        LoadBaseFeature(em, template, user);
        //this.DateCycle(em, template, (em, template, dt) => {
        //    template.Models.Add(dt, _mailRepo.ReportRepository.EmailWeekInvoice(dt, _companyid,user));
        //});

        //string sql = ReplaceMacro(em.SQLCommand, ExecModel);
        string reportname= em.SQLCommand;
        var attach = new EMailAttachment();
        var result = _mailRepo.ProduceExcel(reportname, ExecModel.DateFrom,ExecModel.DateTo, _companyid).GetAwaiter().GetResult();
        attach.Content = result.FileContents;
        attach.ContentType = result.ContentType;
        attach.Name = result.FileDownloadName;
       
        template.Attachments.Add(attach);
        template.JustAttachment = true;

        //template.Models.Add(dt, _mailRepo.ReportRepository.EmailWeekInvoice(dt, _companyid, user));
        return true;
    }

}

}
