using ReactionService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Data.ReactionTypes
{
    public class ReactionTypeRepository : IReactionTypeRepository
    {
        private readonly ReactionDbContext _context;

        public ReactionTypeRepository(ReactionDbContext context)
        {
            _context = context;
        }

        public List<ReactionType> GetReactionTypes()
        {
            return _context.ReactionTypes.ToList();
        }

        public ReactionType GetReactionTypeById(int reactionTypeId)
        {
            return _context.ReactionTypes.FirstOrDefault(e => e.ReactionTypeId == reactionTypeId);
        }

        public void CreateReactionType(ReactionType reactionType)
        {
            _context.ReactionTypes.Add(reactionType);
        }

        public void UpdateReactionType(ReactionType oldReactionType, ReactionType newReactionType)
        {
            oldReactionType.TypeName = newReactionType.TypeName;
        }

        public void DeleteReactionType(int reactionTypeId)
        {
            var reactionType = GetReactionTypeById(reactionTypeId);
            _context.Remove(reactionType);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
