using ReactionService.Models.DTOs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Data.BlockingMock
{
    public class BlockingMockRepository : IBlockingMockRepository
    {
        public static List<BlockingDto> Blockings { get; set; } = new List<BlockingDto>();

        public BlockingMockRepository()
        {
            FillData();
        }


        private static void FillData()
        {
            Blockings.AddRange(new List<BlockingDto>
            {
                new BlockingDto
                {
                    BlockingId = Guid.Parse("efbb198e-3d23-4eab-b515-8e71e60d8959"),
                    BlockerId = Guid.Parse("f2f88bcd-d0a2-4fe7-a23f-df97a59731cd"),
                    BlockedId = Guid.Parse("59ed7d80-39c9-42b8-a822-70ddd295914a")
                }
            });
        }

        public bool CheckDidIBlockSeller(Guid userId, Guid sellerId)
        {
            foreach (BlockingDto blocking in Blockings)
            {
                if (blocking.BlockerId == userId && blocking.BlockedId == sellerId) // I blocked the seller
                {
                    return true;
                }
                else if (blocking.BlockedId == userId && blocking.BlockerId == sellerId) // Seller blocked me
                {
                    return true;
                }
            }
            return false;
        }

        public List<Guid> GetUsersIBlocked(Guid userId)
        {
            List<Guid> blockedUsersId = new List<Guid>();

            foreach (BlockingDto blocking in Blockings)
            {
                if (blocking.BlockerId == userId) // I blocked the seller
                {
                    blockedUsersId.Add(blocking.BlockedId);
                }
            }
            return blockedUsersId;
        }
    }
}
