using AutoMapper;
using Gnappo.Portfolio.Application.Features.Articles.Queries.GetArticleDetail;
using Gnappo.Portfolio.Application.Features.Blog.Queries.GetBlogPostDetail;
using Gnappo.Portfolio.Application.Features.Blog.Queries.GetBlogPosts;
using Gnappo.Portfolio.Application.Features.Storage.Queries;
using Gnappo.Portfolio.Application.Features.Topics.Queries;
using Gnappo.Portfolio.Domain.common;
using Gnappo.Portfolio.Domain.Models;

namespace Gnappo.Portfolio.Application.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<ArticleDetail, ArticleDetailDto>().ReverseMap();
            CreateMap<BlogPost, BlogPostDto>().ReverseMap();
            CreateMap<BlogPostDetail, BlogPostDetailDto>().ReverseMap();
            CreateMap<ContentFile, ContentUrlDto>().ReverseMap();
            CreateMap<TopicInfo, TopicInfoDto>().ReverseMap();
        }
    }
}
