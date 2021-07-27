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
        List<Reaction> GetReactionByPostId(Guid postId);
        List<Reaction> GetReactionByReactionTypeId(int reactionTypeId);
        void CreateReaction(Reaction reaction);
        public void UpdateReaction(Reaction reaction);
        public void DeleteReaction(Guid reactionID);
        bool CheckDoIFollowSeller(int userId, int sellerId);
        bool CheckDidIBlockSeller(int userId, int sellerId);
        public Reaction CheckDidIAlreadyReact(int userId, int postId);
        bool SaveChanges();
    }
}
