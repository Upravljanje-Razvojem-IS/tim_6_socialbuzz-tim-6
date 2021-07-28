using ReactionService.Models.DTOs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Data.FollowingMock
{
    public class FollowingMockRepository : IFollowingMockRepository
    {
        public static List<FollowingDto> Followings { get; set; } = new List<FollowingDto>();

        public FollowingMockRepository()
        {
            FillData();
        }


        private static void FillData()
        {
            Followings.AddRange(new List<FollowingDto>
            {
                new FollowingDto
                {
                    FollowingId = 1,
                    FollowerId = Guid.Parse("f2f88bcd-d0a2-4fe7-a23f-df97a59731cd"),
                    FollowedId = Guid.Parse("42b70088-9dbd-4b19-8fc7-16414e94a8a6")
                },
                new FollowingDto
                {
                    FollowingId = 2,
                    FollowerId = Guid.Parse("59ed7d80-39c9-42b8-a822-70ddd295914a"),
                    FollowedId = Guid.Parse("42b70088-9dbd-4b19-8fc7-16414e94a8a6")
                },
                new FollowingDto
                {
                    FollowingId = 3,
                    FollowerId = Guid.Parse("42b70088-9dbd-4b19-8fc7-16414e94a8a6"),
                    FollowedId = Guid.Parse("59ed7d80-39c9-42b8-a822-70ddd295914a")
                }
            });
        }

        public bool CheckDoIFollowSeller(Guid userId, Guid sellerId)
        {
            foreach (FollowingDto following in Followings)
            {
                if (following.FollowerId == userId && following.FollowedId == sellerId)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Guid> GetUsersIFollow(Guid userId)
        {
            List<Guid> followedUsers = new List<Guid>();

            foreach (FollowingDto following in Followings)
            {
                if (following.FollowerId == userId)
                {
                    followedUsers.Add(following.FollowedId);
                }
            }
            return followedUsers; ;
        }
    }
}
