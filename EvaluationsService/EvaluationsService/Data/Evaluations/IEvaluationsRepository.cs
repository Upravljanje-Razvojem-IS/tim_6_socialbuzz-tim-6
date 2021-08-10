using EvaluationsService.Model.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaluationsService.Data
{
    public interface IEvaluationsRepository
    {
        List<Evaluation> GetAllEvaluations();

        List<Evaluation> GetAllEvaluationsOrderedByMark();

        List<Evaluation> GetEvaluationsByMark(int mark);

        List<Evaluation> GetEvaluationsByPostID(int postID);

        List<Evaluation> GetEvaluationsOnPostByMark(int postID, int mark);

        List<Evaluation> GetEvaluationsByAccountID(int accountID);

        Evaluation GetEvaluationByID(Guid evaluationID);

        void CreateEvaluation(Evaluation evaluation);

        void UpdateEvaluation(Evaluation evaluation);

        void DeleteEvaluation(Guid evaluationID);

        bool SaveChanges();

        bool CheckIfAccountAlredyEvaluatedPost(int postID, int accountID);
    }
}
