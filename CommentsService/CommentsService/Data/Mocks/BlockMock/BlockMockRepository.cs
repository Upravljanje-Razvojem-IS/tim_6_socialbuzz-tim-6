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
                    blockerID = 1,
                    blockedID = 2
                },
                new BlockDto
                {
                    blockerID = 1,
                    blockedID = 3
                },
                new BlockDto
                {
                    blockerID = 2,
                    blockedID = 4
                }
            });
        }

        public bool CheckIfUserBlocked(int accountID, int blockedAccountID)
        {
            var query = from blocked in Blocks
                        select blocked;

            foreach (var v in query)
            {
                if (v.blockedID == accountID && v.blockerID == blockedAccountID)
                {
                    return true;
                }
                else if (v.blockerID == accountID && v.blockedID == blockedAccountID)
                {
                    return true;
                }
            }

            return false;
        }

        public List<int> GetListOfBlockedAccounts(int accountID)
        {
            var query = from block in Blocks
                        where block.blockerID == accountID
                        select block.blockedID;

            return query.ToList();
        }
    }
}
