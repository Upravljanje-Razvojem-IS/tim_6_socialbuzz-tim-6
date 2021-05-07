using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommentingService.Model.Enteties;
using CommentsService.Data.Mocks.BlockMock;

namespace CommentingService.Data
{
    public class CommentingRepository : ICommentingRepository
    {
        private readonly DBContext context;
        private readonly IBlockMockRepository blockRepository;

        public CommentingRepository(DBContext DBcontext, IBlockMockRepository blockRepository)
        {
            context = DBcontext;
            this.blockRepository = blockRepository;
        }

        public void CreateComment(Comment comment)
        {
            context.Comments.Add(comment);
        }

        public void DeleteComment(Guid commentID)
        {
            var comment = GetCommentByID(commentID);
            context.Remove(comment);
        }

        public List<Comment> GetAllComments()
        {
            return context.Comments.ToList();
        }

        public Comment GetCommentByID(Guid commentID)
        {
            return context.Comments.FirstOrDefault(e => e.CommentID == commentID);
        }

        public List<Comment> GetCommentsByPostID(int postID)
        {
            var query = from comment in context.Comments
                        where comment.PostID == postID
                        select comment;

            return query.ToList();
        }

        public List<Comment> GetCommentsByAccountID(int accountID)
        {
            var query = from comment in context.Comments
                        where comment.AccountID == accountID
                        select comment;

            return query.ToList();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateComment(Comment comment)
        {
            context.Update(comment);
        }

        public bool CheckIfUserBlocked(int accountID, int blockedAccountID)
        {
            return blockRepository.CheckIfUserBlocked(accountID, blockedAccountID);
        }
    }
}
