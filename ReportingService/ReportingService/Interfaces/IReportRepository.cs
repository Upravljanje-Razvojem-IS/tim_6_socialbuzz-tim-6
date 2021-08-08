using ReportingService.DTOs;
using System;
using System.Collections.Generic;

namespace ReportingService.Interfaces
{
    public interface IReportRepository
    {
        List<ReportGet> GetAll();
        ReportGet GetById(Guid id);
        ReportConfirm PostNew(ReportCreate newReport);
        ReportConfirm Update(Guid id, ReportCreate updated);
        void Delete(Guid id);
    }
}
