﻿using System.Text;
using Newtonsoft.Json.Linq;
using EventBus.Core;
using EventBus.Core.Internal.Model;
using FeiniuBus;
using System;

namespace EventBus.Subscribe.Infrastructure
{
    public class DefaultMessageDecoder : IMessageDecoder
    {
        public ReceivedMessage Decode(MessageContext context)
        {
            var json = Encoding.UTF8.GetString(context.Content);
            var jobject = JObject.Parse(json);

            try
            {
                var returnValue = new ReceivedMessage
                {
                    Id = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0),
                    MessageId = jobject["MetaData"]["MessageID"].Value<long>(),
                    TransactId = jobject["MetaData"]["TransactID"].Value<long>(),
                    Group = context.Queue,
                    RouteKey = context.Topic,
                    MetaData = jobject["MetaData"].ToString(),
                    Content = jobject["Content"].ToString(),
                    Retries = 0,
                    ReceivedTime = DateTime.Now,
                    ExpiredTime = DateTime.Now.AddDays(1),
                    State = Core.State.MessageState.Processing
                };
                return returnValue;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
