using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingService.MockData
{
    public static class MockedData
    {
        public static List<PostMock> Posts = new List<PostMock>()
        {
            new PostMock()
            {
                Id = Guid.Parse("b05cf711-309b-4806-942a-3b077d013b05"),
                PostName = "Post1",
                Description = "Description of post1",
                Price = 100
            },
            new PostMock()
            {
                Id = Guid.Parse("15dc191f-a3b8-4023-a7ff-9b2cd8fec43c"),
                PostName = "Post2",
                Description = "Description of post2",
                Price = 200
            },
            new PostMock()
            {
                Id = Guid.Parse("166e9562-fad3-4387-ab02-85d23c46315e"),
                PostName = "Post2",
                Description = "Description of post2",
                Price = 300
            }
        };
    }
}
