using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiniSpace.Services.Posts.Application.Dto;

namespace MiniSpace.Services.Posts.Application.Services
{
    public interface IPostRecommendationService
    {
        Task<IEnumerable<(PostDto Post, double Score)>> RankPostsByUserInterestAsync(Guid userId, 
        IEnumerable<PostDto> posts, IDictionary<string, double> userInterests);
    }
}
