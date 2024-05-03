using MiniSpace.Services.Comments.Core.Exceptions;
using System;
using System.Collections.Generic;


namespace MiniSpace.Services.Comments.Core.Entities
{
    public class Comment : AggregateRoot
    {
        public Guid PostId { get; private set; }
        public Guid StudentId { get; private set; }
        public List<Guid> Likes { get; private set; }
        public Guid ParentId { get; private set; }
        public string TextContent { get; private set; }
        public DateTime PublishDate { get; private set; }

        public Comment(Guid id, Guid postId, Guid studentId, List<Guid> likes,
        Guid parentId, string textContent, DateTime publishDate)
        {
            Id = id;
            PostId = postId;
            StudentId = studentId;
            Likes = likes;
            ParentId = parentId;
            TextContent = textContent;
            PublishDate = publishDate;
        }

        public void Like(Guid studentId)
        {
            if(Likes.Contains(studentId)) Likes.Remove(studentId);
            else Likes.Add(studentId);
        }

        public static Comment Create(AggregateId id, Guid postId, Guid studentId, List<Guid> likes,
        Guid parentId, string textContent, DateTime publishDate)
        {
            CheckContent(id, textContent);

            return new Comment(id, postId, studentId, likes, parentId, textContent, publishDate);
        }

        public void Update(string textContent)
        {
            CheckContent(Id, textContent);

            TextContent = textContent;
        }

        private static void CheckContent(AggregateId id, string textContent)
        {
            if (string.IsNullOrWhiteSpace(textContent))
            {
                throw new InvalidCommentContentException(id);
            }
        }
    }
}