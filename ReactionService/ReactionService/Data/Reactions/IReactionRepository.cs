using ReactionService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionService.Data.Reactions
{
    public interface IReactionRepository
    {
        List<Reaction> GetReactions();
        public Reaction GetReactionById(Guid reactionId);
        List<Reaction> GetReactionByPostId(Guid postId, Guid userId);
        List<Reaction> GetReactionByReactionTypeId(int reactionTypeId);
        void CreateReaction(Reaction reaction);
        public void UpdateReaction(Reaction oldReaction, Reaction newReaction);
        public void DeleteReaction(Guid reactionID);
        bool CheckDoIFollowSeller(Guid userId, Guid sellerId);
        bool CheckDidIBlockSeller(Guid userId, Guid sellerId);
        public Reaction CheckDidIAlreadyReact(Guid userId, Guid postId);
        bool SaveChanges();
    }
}
