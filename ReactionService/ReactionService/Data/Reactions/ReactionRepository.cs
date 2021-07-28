using Microsoft.EntityFrameworkCore;
using ReactionService.Data.BlockingMock;
using ReactionService.Data.FollowingMock;
using ReactionService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Data.Reactions
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly ReactionDbContext _context;
        private readonly IBlockingMockRepository _blockingMockRepository;
        private readonly IFollowingMockRepository _followingMockRepository;

        public ReactionRepository(ReactionDbContext context, IBlockingMockRepository blockingMockRepository, IFollowingMockRepository followingMockRepository)
        {
            _context = context;
            _blockingMockRepository = blockingMockRepository;
            _followingMockRepository = followingMockRepository;
        }

        public List<Reaction> GetReactions()
        {
            return _context.Reactions.ToList();
        }

        public Reaction GetReactionById(Guid reactionId)
        {
            return _context.Reactions.Include(e => e.ReactionType).FirstOrDefault(e => e.ReactionId == reactionId);
        }

        public List<Reaction> GetReactionByReactionTypeId(int reactionTypeId)
        {
            return _context.Reactions.Include(e => e.ReactionType).Where(e => e.ReactionTypeId == reactionTypeId).ToList();
        }

        public List<Reaction> GetReactionByPostId(Guid postId, Guid userId)
        {
            var reactions = from reaction in _context.Reactions
                        where !(from users in _blockingMockRepository.GetBlockedUsers(userId)
                                select users).Contains(reaction.AccountId)
                        where reaction.PostId == postId
                        select reaction;

            return reactions.Include(e => e.ReactionType).ToList();
        }

        public void CreateReaction(Reaction reaction)
        {
            throw new NotImplementedException();
        }

        public void UpdateReaction(Reaction reaction)
        {
            throw new NotImplementedException();
        }

        public void DeleteReaction(Guid reactionID)
        {
            throw new NotImplementedException();
        }

        public Reaction CheckDidIAlreadyReact(Guid userId, Guid postId)
        {
            throw new NotImplementedException();
        }

        public bool CheckDidIBlockSeller(Guid userId, Guid sellerId)
        {
            return _blockingMockRepository.CheckDidIBlockSeller(userId, sellerId);
        }

        public bool CheckDoIFollowSeller(Guid userId, Guid sellerId)
        {
            return _followingMockRepository.CheckDoIFollowSeller(userId, sellerId);
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
