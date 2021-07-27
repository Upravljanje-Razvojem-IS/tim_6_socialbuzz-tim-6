using Microsoft.EntityFrameworkCore;
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

        public List<Reaction> GetReactionByPostId(Guid postId)
        {
            return _context.Reactions.Include(e => e.ReactionType).Where(e => e.PostId == postId).ToList();
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
