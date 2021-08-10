using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommentingService.Model.Enteties;

namespace CommentingService.Data
{
    public interface ICommentingRepository
    {
        List<Comment> GetAllComments();

        List<Comment> GetCommentsByPostID(int postID);

        List<Comment> GetCommentsByAccountID(int accountID);

        Comment GetCommentByID(Guid commentID);

        void CreateComment(Comment comment);

        void UpdateComment(Comment comment);

        void DeleteComment(Guid commentID);

        bool SaveChanges();

        bool CheckIfUserBlocked(int accountID, int blockedAccountID);

    }
}
