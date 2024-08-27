using MiniSpace.Services.Communication.Application.Dto;
using MiniSpace.Services.Communication.Core.Entities;
using MiniSpace.Services.Communication.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Communication.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        // Converts MessageDocument to Message entity
        public static Message AsEntity(this MessageDocument document)
        {
            var message = new Message(
                document.ChatId,
                document.SenderId,
                document.ReceiverId,
                document.Content,
                document.Type);

            if (document.Status == MessageStatus.Read)
            {
                message.MarkAsRead();
            }
            else if (document.Status == MessageStatus.Deleted)
            {
                message.MarkAsDeleted();
            }

            return message;
        }

        public static MessageDocument AsDocument(this Message entity)
        {
            return new MessageDocument
            {
                Id = entity.Id,
                ChatId = entity.ChatId,
                SenderId = entity.SenderId,
                ReceiverId = entity.ReceiverId,
                Content = entity.Content,
                Timestamp = entity.Timestamp,
                Type = entity.Type,
                Status = entity.Status
            };
        }
          public static Chat AsEntity(this ChatDocument document)
        {
            var chat = new Chat(document.ParticipantIds);

            foreach (var messageDocument in document.Messages)
            {
                chat.AddMessage(messageDocument.AsEntity());
            }

            return chat;
        }

        public static ChatDocument AsDocument(this Chat entity)
        {
            return new ChatDocument
            {
                Id = entity.Id,
                ParticipantIds = entity.ParticipantIds,
                Messages = entity.Messages.Select(m => m.AsDocument()).ToList()
            };
        }

         public static ChatDto AsDto(this Chat entity)
        {
            return new ChatDto
            {
                Id = entity.Id,
                ParticipantIds = entity.ParticipantIds,
                Messages = entity.Messages.Select(m => m.AsDto()).ToList()
            };
        }

        public static MessageDto AsDto(this Message entity)
        {
            return new MessageDto
            {
                Id = entity.Id,
                SenderId = entity.SenderId,
                Content = entity.Content,
                Timestamp = entity.Timestamp,
                MessageType = entity.Type.ToString(),
                Status = entity.Status.ToString()
            };
        }
    }
}
