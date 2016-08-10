namespace Sleemon.Service
{
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using Sleemon.Data;
    using Sleemon.Core;

    public class MessageService : IMessageService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public MessageService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public IList<MessageViewModel> GetBroadcastMessage(int maxCount)
        {
            return this._invoicingEntities.spGetBroadcastMessage(maxCount)
                .Select(p => new MessageViewModel()
                {
                    Id = p.Id ?? 0,
                    Type = p.MessageType ?? 0,
                    Receivers = new Receiver()
                    {
                        ToUsers = p.ToUsers,
                        ToDepts = p.ToDepts
                    },
                    News = new News()
                    {
                        Title = p.Subject,
                        Description = p.Summary,
                        Url = p.LinkedId ?? 0,
                        PicUrl = p.AvatarPath
                    }
                })
                .ToList();
        }
    }
}
