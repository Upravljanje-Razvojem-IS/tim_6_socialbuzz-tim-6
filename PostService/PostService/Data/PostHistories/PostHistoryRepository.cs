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


        public PostHistory GetPostHistoryById(int id)
        {
            return _context.PostHistories.FirstOrDefault(e => e.PostHistoryId == id);
        }

        public void CreatePostHistory(PostHistory postHistory)
        {
            postHistory.DateFrom = DateTime.Now;
            //setujem DateTo na trenutni datum i vreme kako bih oznacio da tada prestaje vazenje cene koja je do tad vazila
            //za post sa tim id-jem, i da se sad primenjuje nova cena ciji je DateFrom trenutni datum i vreme
            var histories = GetPostHistoryByPostId(postHistory.PostId);
            foreach (PostHistory ph in histories)
            {
                if (ph.DateTo == null)
                {
                    ph.DateTo = DateTime.Now;
                }
            }
            _context.PostHistories.Add(postHistory);
        }

        public void DeletePostHistory(int id)
        {
            var postHistory = GetPostHistoryById(id);
            _context.Remove(postHistory);
        }

        public void UpdatePostHistory(PostHistory oldPostHistory, PostHistory newPostHistory)
        {
            oldPostHistory.Price = newPostHistory.Price;
            oldPostHistory.DateFrom = newPostHistory.DateFrom;
            oldPostHistory.DateTo = newPostHistory.DateTo;
            oldPostHistory.PostId = newPostHistory.PostId;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
