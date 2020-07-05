﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RCNClinicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class visitController : ControllerBase
    {
        private IRepositoryAsync<tblVisit> repository;
        public visitController(IRepositoryAsync<tblVisit> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll(string LName,string fromdate, string todate)
        {

            fromdate = fromdate.Replace('_', '/');
            todate = todate.Replace('_', '/');
            ContextDb db = new ContextDb();
            IEnumerable<GetVisitListResult> lst;
            if (!string.IsNullOrEmpty(LName))
            {
                lst = db.tblVisits.Join(db.tblReceptions.Where(c => c.tbl_patient.LastName.Contains(LName)), c => c.IdReception, d => d.Id, (c, d) => new GetVisitListResult
                {
                    Tel = d.tbl_patient.Tel,
                    service = d.tbl_Service.Name,
                    ReceptionId = c.IdReception,
                    VisitCurrentId = c.Id,
                    patientId = d.IdPatient,
                    DateCurrent = c.VisitDate,
                    IdWaitingVisitCurrent = c.IdWaiting,
                    FullName = d.tbl_patient.Name + " " + d.tbl_patient.LastName,
                    DossierNumberPermanent = d.tbl_patient.DossierNumberPermanent
                });
            }
            else
            {
                if (string.IsNullOrEmpty(fromdate))
                {
                    lst = db.tblVisits.Join(db.tblReceptions, c => c.IdReception, d => d.Id, (c, d) => new GetVisitListResult
                    {
                        Tel = d.tbl_patient.Tel,
                        service = d.tbl_Service.Name,
                        ReceptionId = c.IdReception,
                        patientId = d.IdPatient,
                        VisitCurrentId = c.Id,
                        IdWaitingVisitCurrent = c.IdWaiting,
                        DateCurrent = c.VisitDate,
                        FullName = d.tbl_patient.Name + " " + d.tbl_patient.LastName,
                        DossierNumberPermanent = d.tbl_patient.DossierNumberPermanent
                    });
                }
                else
                {
                    DateTime from = MYHelper.DiffDate(fromdate, true);
                    DateTime to = MYHelper.DiffDate(todate, false);
                    lst = db.tblVisits.Where(c => c.VisitDate >= from && c.VisitDate <= to).Join(db.tblReceptions, c => c.IdReception, d => d.Id, (c, d) => new GetVisitListResult
                    {
                        Tel = d.tbl_patient.Tel,
                        service = d.tbl_Service.Name,
                        ReceptionId = c.IdReception,
                        VisitCurrentId = c.Id,
                        patientId = d.IdPatient,
                        DateCurrent = c.VisitDate,
                        IdWaitingVisitCurrent = c.IdWaiting,
                        FullName = d.tbl_patient.Name + " " + d.tbl_patient.LastName,
                        DossierNumberPermanent = d.tbl_patient.DossierNumberPermanent
                    });
                }
            }
            lst = lst.GroupBy(d => d.ReceptionId).Select(d => new GetVisitListResult
            {
                Tel = d.FirstOrDefault().Tel,
                service = d.FirstOrDefault().service,
                ReceptionId = d.Key,
                DateCurrent = d.OrderBy(z => z.DateCurrent).FirstOrDefault().DateCurrent,
                VisitCurrentId = d.OrderBy(z => z.DateCurrent).FirstOrDefault().VisitCurrentId,
                patientId = d.FirstOrDefault().patientId,
                IdWaitingVisitCurrent = d.OrderBy(z => z.DateCurrent).FirstOrDefault().IdWaitingVisitCurrent,
                FullName = d.FirstOrDefault().FullName,
                DossierNumberPermanent = d.FirstOrDefault().DossierNumberPermanent
            });
            lst = lst.Distinct().ToList();
            foreach (var item in lst)
            {
                var lstvisit = db.tblVisits.Where(c => c.IdReception == item.ReceptionId).OrderBy(c => c.VisitDate).ToList();//.Select(c => c.PersianDate);
                for (int i = 1; i <= lstvisit.Count(); i++)
                {
                    tblVisit visit = lstvisit.Skip((i - 1)).Take(1).FirstOrDefault();
                    switch (i)
                    {
                        case 1:
                            item.Date1 =MYHelper.PersianDate(visit.VisitDate);
                            item.IdWaiting1 = visit.IdWaiting;
                            break;
                        case 2:
                            item.Date2 = MYHelper.PersianDate(visit.VisitDate);
                            item.IdWaiting2 = visit.IdWaiting;
                            break;
                        case 3:
                            item.Date3 = MYHelper.PersianDate(visit.VisitDate);
                            item.IdWaiting3 = visit.IdWaiting;
                            break;
                        case 4:
                            item.Date4 = MYHelper.PersianDate(visit.VisitDate);
                            item.IdWaiting4 = visit.IdWaiting;
                            break;
                        case 5:
                            item.Date5 = MYHelper.PersianDate(visit.VisitDate);
                            item.IdWaiting5 = visit.IdWaiting;
                            break;
                        case 6:
                            item.Date6 = MYHelper.PersianDate(visit.VisitDate);
                            item.IdWaiting6 = visit.IdWaiting;
                            break;
                        case 7:
                            item.Date7 = MYHelper.PersianDate(visit.VisitDate);
                            item.IdWaiting7 = visit.IdWaiting;
                            break;
                        case 8:
                            item.Date8 = MYHelper.PersianDate(visit.VisitDate);
                            item.IdWaiting8 = visit.IdWaiting;
                            break;
                        case 9:
                            item.Date9 = MYHelper.PersianDate(visit.VisitDate);
                            item.IdWaiting9 = visit.IdWaiting;
                            break;
                        case 10:
                            item.Date10 = MYHelper.PersianDate(visit.VisitDate);
                            item.IdWaiting10 = visit.IdWaiting;
                            break;

                        default:
                            break;
                    }
                }
            }
            return Ok(lst);
        }

        [HttpGet("getReportVisit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll(string fromdate, string todate, int? idService)
        {
            fromdate = fromdate.Replace('_', '/');
            todate = todate.Replace('_', '/');
            DateTime from = MYHelper.DiffDate(fromdate, true);
            DateTime to = MYHelper.DiffDate(todate, false);
            ContextDb db = new ContextDb();
            var qq = repository.GetAll(c =>
             c.VisitDate >= from &&
             c.VisitDate <= to &&
             (idService.HasValue ? (c.tblReception.IdService == idService) : true)
            ).Result;
            List<reportvisit_result> result = new List<reportvisit_result>();
            int i = 0;
            foreach (var item in qq)
            {
                i++;
                var reception = db.tblReceptions.Find(item.IdReception);
                var service = db.tbl_Service.Find(reception.IdService);
                var patient = db.tbl_patient.Find(reception.IdPatient);
                result.Add(new reportvisit_result
                {
                    Comment = item.Comment,
                    Dossier = patient.DossierNumberPermanent,
                    FullName = patient.Name + ' ' + patient.LastName,
                    Payment = item.FreePrices,
                    PersianDate = MYHelper.PersianDate(item.VisitDate),
                    service = service.Name,
                    Tel = patient.Tel,
                    total = item.TotalMeeting,
                    Row = i
                });
            }

            return Ok(result);
        }

        [HttpGet("getreportChart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll(string year, int idService = 0, int iddoctor = 0)
        {
            DateTime from = MYHelper.DiffDate(year + "/01/01", true);
            DateTime to = MYHelper.DiffDate(year + "/12/29", false);
            var qq = repository.GetAll(c =>
             c.VisitDate >= from &&
             c.VisitDate <= to &&
             (idService > 0 ? (c.tblReception.IdService == idService) : true) &&
             (iddoctor > 0 ? (c.tblReception.IdDoctor == iddoctor) : true)
            ).Result;

            List<chartvisit_result> result = new List<chartvisit_result>();
            for (int i = 1; i <= 12; i++)
            {
                string month = MYHelper.getMonthName(i);
                DateTime fromdate = MYHelper.DiffDate(year + "/" + i.ToString().PadLeft(2, '0') + "/01", true);
                DateTime todate = MYHelper.DiffDate(year + "/" + i.ToString().PadLeft(2, '0') + "/" + (i == 12 ? "29" : "30"), false);
                var query = qq.Where(c => c.VisitDate >= fromdate && c.VisitDate <= todate).ToList();
                int isdo = query.Where(c => c.IdWaiting == 5).Count();
                int isnotdo = query.Where(c => c.IdWaiting == 7 || c.IdWaiting == 8).Count();

                result.Add(new chartvisit_result
                {
                    IsDo = isdo,
                    IsNotDo = isnotdo,
                    Month = month
                });
            }

            return Ok(result);
        }


        [HttpGet("{idreception}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<tblVisit>> GetAll(long idreception)
        {
            return await repository.GetAll(c => c.IdReception == idreception);

        }


        [HttpGet("{idreception}/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<tblVisit>> Get(long idreception, DateTime date)
        {
            var tblVisit = await repository.Get(c => c.IdReception == idreception && c.VisitDate == date);

            if (tblVisit == null)
            {
                return NotFound();
            }
            tblVisit.FarsiDate = MYHelper.PersianDate(tblVisit.VisitDate);
            return tblVisit;
        }

        [HttpPut("{id}")]
        //[HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> Put(long id, tblVisit model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            try
            {
                model.VisitDate = MYHelper.GetDate(model.FarsiDate);
                await repository.Update(model);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> Post(tblVisit model)
        {
            try
            {
                model.VisitDate = MYHelper.GetDate(model.FarsiDate);
                await repository.Add(model);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpDelete("{id}")]
        //[HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JsonR>> Delete(long id)
        {


            JsonR result;
            var tblVisit = await repository.Get(id);
            if (tblVisit == null)
            {
                result = new JsonR
                {
                    Title = "error",
                    Message = "چنین آیتمی وجود ندارد"
                };
            }
            else
            {
                try
                {
                    ContextDb db = new ContextDb();

                    if (db.tblPictures.Any(c => c.IdVisit == id))
                    {

                        result = new JsonR
                        {
                            Title = "error",
                            Message = "برای این جلسه تصاویر ثبت شده است و امکان حذف نمیباشد"
                        };
                    }
                    else if (tblVisit.IdWaiting == 5)
                    {

                        result = new JsonR
                        {
                            Title = "error",
                            Message = "این جلسه برگزار گردیده است و امکان حذف نمیباشد"
                        };
                    }
                    else
                    {
                        db.tbl_SMS.RemoveRange(db.tbl_SMS.Where(c => c.IdVisit == id));
                        await repository.Delete(id);
                        result = new JsonR
                        {
                            Title = "success",
                            Message = "حذف با موفقیت انجام گردید"
                        };
                    }
                    return result;
                }
                catch (Exception err)
                {
                    result = new JsonR
                    {
                        Title = "error",
                        Message = err.Message
                    };
                }
            }

            return result;
        }


    }
}