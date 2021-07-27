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

        public ReactionRepository(ReactionDbContext context)
        {
            _context = context;
        }

        public List<Reaction> GetReactions()
        {
            throw new NotImplementedException();
        }

        public Reaction GetReactionById(Guid reactionId)
        {
            throw new NotImplementedException();
        }

        public List<Reaction> GetReactionByReactionTypeId(int reactionTypeId)
        {
            return _context.Reactions.Where(e => e.ReactionTypeId == reactionTypeId).ToList();
        }

        public List<Reaction> GetReactionByPostId(int postId)
        {
            throw new NotImplementedException();
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

        public Reaction CheckDidIAlreadyReact(int userId, int postId)
        {
            throw new NotImplementedException();
        }

        public bool CheckDidIBlockSeller(int userId, int sellerId)
        {
            throw new NotImplementedException();
        }

        public bool CheckDoIFollowSeller(int userId, int sellerId)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
