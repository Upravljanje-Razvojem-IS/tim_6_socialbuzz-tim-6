using ReactionService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Data.ReactionTypes
{
    public interface IReactionTypeRepository
    {
        List<ReactionType> GetReactionTypes();

        ReactionType GetReactionTypeById(int reactionTypeId);

        void CreateReactionType(ReactionType reactionType);

        void UpdateReactionType(ReactionType reactionType);

        void DeleteReactionType(int reactionTypeId);

        public bool SaveChanges();
    }
}
