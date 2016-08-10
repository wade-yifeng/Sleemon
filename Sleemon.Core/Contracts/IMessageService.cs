namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public interface IMessageService
    {
        IList<MessageViewModel> GetBroadcastMessage(int maxCount);
    }
}
