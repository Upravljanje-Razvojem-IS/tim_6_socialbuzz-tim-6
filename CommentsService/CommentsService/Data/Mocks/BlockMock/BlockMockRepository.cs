using CommentsService.Model.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentsService.Data.Mocks.BlockMock
{
    public class BlockMockRepository : IBlockMockRepository
    {
        public static List<BlockDto> Blocks { get; set; } = new List<BlockDto>();

        public BlockMockRepository()
        {
            FillData();
        }

        private void FillData()
        {
            Blocks.AddRange(new List<BlockDto>
            {
                new BlockDto
                {
                    BlockerID = 1,
                    BlockedID = 2
                },
                new BlockDto
                {
                    BlockerID = 1,
                    BlockedID = 3
                },
                new BlockDto
                {
                    BlockerID = 2,
                    BlockedID = 4
                }
            });
        }

        public bool CheckIfUserBlocked(int accountID, int blockedAccountID)
        {
            var query = from blocked in Blocks
                        select blocked;

            foreach (var v in query)
            {
                if (v.BlockedID == accountID && v.BlockerID == blockedAccountID)
                {
                    return true;
                }
                else if (v.BlockerID == accountID && v.BlockedID == blockedAccountID)
                {
                    return true;
                }
            }

            return false;
        }

        public List<int> GetListOfBlockedAccounts(int accountID)
        {
            var query = from block in Blocks
                        where block.BlockerID == accountID
                        select block.BlockedID;

            return query.ToList();
        }
    }
}
