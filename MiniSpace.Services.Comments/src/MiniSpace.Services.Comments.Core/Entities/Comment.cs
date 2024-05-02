using MiniSpace.Services.Comments.Core.Exceptions;


namespace MiniSpace.Services.Comments.Core.Entities
{
    public class Comment : AggregateRoot
    {
        public Guid PostId { get; private set; }
        public Guid StudentId { get; private set; }
        public List<Guid> Likes { get; private set; }
        public Guid ParentId { get; private set; }
        public string Comment { get; private set; }
        public DateTime PublishDate { get; private set; }

        public Comment(Guid id, Guid postId, Guid studentId, List<Guid> likes,
        Guid parentId, string comment, DateTime publishDate)
        {
            Id = id;
            PostId = postId;
            StudentId = studentId;
            Likes = likes;
            ParentId = parentId;
            Comment = comment;
            PublishDate = publishDate;
        }

        public static Comment Create(AggregateId id, Guid postId, Guid studentId, List<Guid> likes,
        Guid parentId, string comment, DateTime publishDate)
        {
            CheckContent(id, comment);

            return new Comment(id, postId, studentId, likes, parentId, comment, publishDate);
        }

        public void Update(string textContent)
        {
            CheckContent(Id, textContent);

            Comment = textContent;
        }

        private static void CheckContent(AggregateId id, string textContent)
        {
            if (string.IsNullOrWhiteSpace(textContent) && string.IsNullOrWhiteSpace(mediaContent))
            {
                throw new InvalidCommentContentException(id);
            }
        }
    }
}