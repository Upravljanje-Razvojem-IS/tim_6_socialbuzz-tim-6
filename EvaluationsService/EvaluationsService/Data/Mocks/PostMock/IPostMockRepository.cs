using EvaluationsService.Model.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationsService.Data.PostMock
{
    public interface IPostMockRepository
    {
        PostDto GetPostByID(int ID);
    }
}
