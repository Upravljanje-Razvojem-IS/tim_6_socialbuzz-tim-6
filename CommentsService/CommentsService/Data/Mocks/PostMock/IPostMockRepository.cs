using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommentingService.Model.Mock;

namespace CommentingService.Data.PostMock
{
    public interface IPostMockRepository
    {
        PostDto GetPostByID(int ID);
    }
}
