namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Common;
    using Sleemon.Data;

    public interface ITrainingService
    {
        IList<UserTrainingPreviewModel> GetUserTrainingList(bool isAll, string userId, int pageIndex, int pageSize);

        UserTrainingDetailModel GetUserTrainingDetail(bool isAll, string userId, int trainingId);

        IList<UserViewModel> GetTrainingParticipants(int trainingId);

        ResultBase JoinTraining(string userId, int trainingId);

        IList<TrainingListModel> GetTrainingList(TrainingSearchContext search);

        TrainingDetailModel GetTrainingDetailById(int trainingId);

        ResultBase SaveTrainingDetail(TrainingDetailModel training);

        ResultBase DeleteTrainingById(int trainingId);

        ResultBase UpdateTrainingUsersJoinState(int trainingId, IDictionary<JoinStatus, IList<string>> joinStatusUsers);
    }
}
