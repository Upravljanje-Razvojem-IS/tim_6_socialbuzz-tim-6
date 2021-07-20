using PostService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Data.PostHistories
{
    public interface IPostHistoryRepository
    {
        List<PostHistory> GetPostHistories();
        List<PostHistory> GetPostHistoryByPostId(Guid id);
        void CreatePostHistory(PostHistory postHistory);
        void UpdatePostHistory(PostHistory postHistory);
        void DeletePostHistory(int id);
    }
}
