using AutoMapper;
using ReportingService.Database;
using ReportingService.DTOs;
using ReportingService.Entities;
using ReportingService.Interfaces;
using ReportingService.MockData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReportingService.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly DatabaseContext _db;
        private readonly IMapper _mapper;

        public ReportRepository(DatabaseContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public List<ReportGet> GetAll()
        {
            var reports = _mapper.Map<List<ReportGet>>(_db.Reports.ToList());

            return reports;
        }

        public ReportGet GetById(Guid id)
        {
            var reports = _mapper.Map<ReportGet>(_db.Reports.FirstOrDefault(report => report.Id == id));

            return reports;
        }

        public ReportConfirm PostNew(ReportCreate newReport)
        {
            var user = MockedData.Posts.FirstOrDefault(post => post.Id == newReport.PostId);

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

            return _mapper.Map<ReportConfirm>(report);
        }

        public ReportConfirm Update(Guid id, ReportCreate updated)
        {
            var report = _db.Reports.FirstOrDefault(r => r.Id == id);

            var user = MockedData.Posts.FirstOrDefault(post => post.Id == updated.PostId);

            report.NumberOfComments = updated.NumberOfComments;
            report.NumberOfLikes = updated.NumberOfLikes;
            report.NumberOfMarks = updated.NumberOfMarks;
            report.PostId = updated.PostId;

            _db.SaveChanges();

            return _mapper.Map<ReportConfirm>(report);
        }

        public void Delete(Guid id)
        {
            var report = _db.Reports.FirstOrDefault(r => r.Id == id);

            _db.Reports.Remove(report);
            _db.SaveChanges();
        }

    }
}
