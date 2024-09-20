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
        // Use the existing ID and other properties directly from the document
        return new Message(
            document.Id,            // Use the existing ID from the document
            document.ChatId,
            document.SenderId,
            document.ReceiverId,
            document.Content,
            document.Timestamp,
            document.Type,
            document.Status          // Ensure the status is loaded correctly
        );
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
        var messages = document.Messages.Select(m => m.AsEntity()).ToList();
        return new Chat(document.Id, document.ParticipantIds, messages);
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
            Id = entity.Id,              // Ensure the ID is correctly mapped
            ChatId = entity.ChatId,
            SenderId = entity.SenderId,
            Content = entity.Content,
            Timestamp = entity.Timestamp,
            MessageType = entity.Type.ToString(),
            Status = entity.Status.ToString()
        };
    }
}

    }
