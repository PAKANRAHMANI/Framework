﻿using System;

namespace Framework.Messages
{
    public class Message : IMessage
    {
        public Guid MessageId { get; protected set; }
        public DateTime PublishDateTime { get; protected set; }

        public Message()
        {
            this.MessageId = Guid.NewGuid();
            this.PublishDateTime = DateTime.Now;
        }
    }
}