using Base.Csi.Sms.MsgContracts;
using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmsService.EventBus
{
    public class SendMobileCodeCommandConsumer : IConsumer<SendMobileCodeCommand>
    {
        private readonly IMemoryCache _cache;

        public SendMobileCodeCommandConsumer(
            IMemoryCache cache
            )
        {
            _cache = cache;
        }

        public Task Consume(ConsumeContext<SendMobileCodeCommand> context)
        {
            foreach (var item in context.Message.PhoneNumbers)
            {
                _cache.Set(item, context.Message.Code);
            }
            return Task.CompletedTask;
        }
    }
}
