using EvaluationsService.Data;
using EvaluationsService.Model.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationsService.Data
{
    public class EvaluationsRepository : IEvaluationsRepository
    {
        private readonly DBContext context;

        public EvaluationsRepository(DBContext DBcontext)
        {
            context = DBcontext;
        }

        public void CreateEvaluation(Evaluation evaluation)
        {
            context.Evaluations.Add(evaluation);
        }

        public void DeleteEvaluation(Guid evaluationID)
        {
            var evaluation = GetEvaluationByID(evaluationID);
            context.Remove(evaluation);
        }

        public List<Evaluation> GetAllEvaluations()
        {
            return context.Evaluations.ToList();
        }

        public List<Evaluation> GetAllEvaluationsOrderedByMark()
        {
            return context.Evaluations.OrderBy(c => c.Mark).ToList();
        }

        public Evaluation GetEvaluationByID(Guid evaluationID)
        {
            return context.Evaluations.FirstOrDefault(e => e.EvaluationID == evaluationID);
        }

        public List<Evaluation> GetEvaluationsByAccountID(int accountID)
        {
            var query = from evaluation in context.Evaluations
                        where evaluation.AccountID == accountID
                        select evaluation;

            return query.ToList();
        }

        public List<Evaluation> GetEvaluationsByPostID(int postID)
        {
            var query = from evaluation in context.Evaluations
                        where evaluation.PostID == postID
                        select evaluation;

            return query.OrderBy(c => c.Mark).ToList();
        }

        public List<Evaluation> GetEvaluationsOnPostByMark(int postID, int mark)
        {
            var query = from evaluation in context.Evaluations
                        where (evaluation.PostID == postID && evaluation.Mark == mark)
                        select evaluation;

            return query.ToList();
        }

        public List<Evaluation> GetEvaluationsByMark(int mark)
        {
            var query = from evaluation in context.Evaluations
                        where evaluation.Mark == mark
                        select evaluation;

            return query.ToList();
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateEvaluation(Evaluation evaluation)
        {
            context.Update(evaluation);
        }

        public bool CheckIfAccountAlredyEvaluatedPost(int postID, int accountID)
        {
            var Posts = GetEvaluationsByPostID(postID);
            var Evaluated = Posts.Find(e => e.AccountID == accountID);
            if (Evaluated == null)
            {
                return false;
            }
            return true;
        }
    }
}
