using AutoMapper;
using ReportingService.Database;
using ReportingService.DTOs;
using ReportingService.Entities;
using ReportingService.ExceptionReport;
using ReportingService.Interfaces;
using ReportingService.Logger;
using ReportingService.MockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingService.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _mapper;
        private readonly LoggerMock _logger;

        public ReportRepository(DatabaseContext db, IMapper mapper, LoggerMock logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }

        public List<ReportGet> GetAll()
        {
            var reports = _mapper.Map<List<ReportGet>>(_db.Reports.ToList());

            _logger.Log("Get all reports");

            return reports;
        }

        public ReportGet GetById(Guid id)
        {
            var reports = _mapper.Map<ReportGet>(_db.Reports.FirstOrDefault(report => report.Id == id));

            _logger.Log("Get report by id");

            return reports;
        }

        public ReportConfirm PostNew(ReportCreate newReport)
        {
            var user = MockedData.Posts.FirstOrDefault(post => post.Id == newReport.PostId);

            if (user == null)
                throw new ReportException("Post with given id does not exist", 404);

            Report report = new Report()
            {
                Id = Guid.NewGuid(),
                NumberOfComments = newReport.NumberOfComments,
                NumberOfLikes = newReport.NumberOfLikes,
                NumberOfMarks = newReport.NumberOfMarks,
                PostId = newReport.PostId
            };

            _db.Add(report);
            _db.SaveChanges();

            _logger.Log("Create new report");

            return _mapper.Map<ReportConfirm>(report);
        }

        public ReportConfirm Update(Guid id, ReportCreate updated)
        {
            var report = _db.Reports.FirstOrDefault(r => r.Id == id);

            if (report == null)
                throw new ReportException("Report with that ID does not exist", 404);

            var user = MockedData.Posts.FirstOrDefault(post => post.Id == updated.PostId);

            if (user == null)
                throw new ReportException("Post with given id does not exist", 404);

            report.NumberOfComments = updated.NumberOfComments;
            report.NumberOfLikes = updated.NumberOfLikes;
            report.NumberOfMarks = updated.NumberOfMarks;
            report.PostId = updated.PostId;

            _db.SaveChanges();

            _logger.Log("Update report");

            return _mapper.Map<ReportConfirm>(report);
        }

        public void Delete(Guid id)
        {
            var report = _db.Reports.FirstOrDefault(r => r.Id == id);

            if (report == null)
                throw new ReportException("Report with that ID does not exist", 404);

            _db.Reports.Remove(report);

            _logger.Log("Delete report by id");
            _db.SaveChanges();
        }

    }
}
