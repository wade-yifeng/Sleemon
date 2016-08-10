namespace Sleemon.Common
{
    public enum StatusCode
    {
        Success = 10000,

        Failed = 10001,

        InvalidArgument = 10002,

        ProsSuccess = 20000,

        CancelProsSuccess = 20001,

        ConsSuccess = 20002,

        CancelConsSuccess = 20003,

        ProsFailed = 20004,

        CancelProsFailed = 20005,

        ConsFailed = 20006,

        CancelConsFailed = 20007,
        
        SignedIn = 30000,

        BeyondMaxParticipantLimit = 40001,

        DuplicateUploadStorePatrol = 50001
    }
}
