using PostService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.PostHistories
{
    public class PostHistoryRepository : IPostHistoryRepository
    {
        private readonly PostDbContext _context;

        public PostHistoryRepository(PostDbContext context)
        {
            _context = context;
        }

        public List<PostHistory> GetPostHistories()
        {
            return _context.PostHistories.ToList();
        }

        public List<PostHistory> GetPostHistoryByPostId(Guid id)
        {
            return _context.PostHistories.Where(e => e.PostId == id).ToList();
        }

        public void CreatePostHistory(PostHistory postHistory)
        {
            throw new NotImplementedException();
        }

        public void DeletePostHistory(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdatePostHistory(PostHistory postHistory)
        {
            throw new NotImplementedException();
        }
    }
}
