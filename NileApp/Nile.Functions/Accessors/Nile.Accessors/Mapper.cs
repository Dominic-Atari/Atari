//global using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Nile.Common.InternalDTOs;
using Nile.Utilities;
using Nile.Utilities.AzureSdk;
using EF = Nile.Database.Entities;


namespace Nile.Accessors
{
    /// <summary>
    /// Custom mapper wrapper around AutoMapper with project-specific configuration
    /// </summary>
    internal class Mapper : IMapper
    {
        private readonly ILogger<Mapper> _logger;
        private readonly IDateUtility _dateUtility;
        public IConfigurationProvider Configuration { get; }
        
        private AutoMapper.IMapper AutoMapper { get; set; }

        public Mapper(ILogger<Mapper> logger, 
            IDateUtility dateUtility, 
            ILoggerFactory loggerFactory)
        {
            _logger = logger;
            _dateUtility = dateUtility;

            // Pass ILoggerFactory as second parameter.
            var config = new MapperConfiguration(cfg =>
            {
                AddUserMappings(cfg);
                AddPostMappings(cfg);
            }, loggerFactory);

            Configuration = config;
            Configuration = config;
            AutoMapper = config.CreateMapper();
        }

        // Mapping configuration methods
        private void AddUserMappings(IMapperConfigurationExpression cfg)
        {
            // Entity to Response DTO
            cfg.CreateMap<EF.User, UserResponseBase>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => new[] { src.UserId }))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            cfg.CreateMap<EF.User, StoreUserResponseBase>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => new[] { src.UserId }))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            // Request DTO to Entity (Create)
            cfg.CreateMap<CreateUserRequest, EF.User>(MemberList.Source)
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => _dateUtility.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => _dateUtility.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Set in accessor
                .ForSourceMember(src => src.ExternalAuthId, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Limit, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.PagingToken, opt => opt.DoNotValidate());

            // Request DTO to Entity (Update)
            cfg.CreateMap<UserRequestBase, EF.User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => _dateUtility.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }

        private void AddPostMappings(IMapperConfigurationExpression cfg)
        {
            // // Post mappings when you implement posts
            // cfg.CreateMap<PostRequestBase, Post>()
            //     .ForMember(dest => dest.PostId, opt => opt.Ignore())
            //     .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => _dateUtility.GetCurrentUtcDateTime()))
            //     .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => _dateUtility.GetCurrentUtcDateTime()))
            //     .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));
            //
            // cfg.CreateMap<Post, PostResponseBase>()
            //     .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id.ToString()));
        }
        
        // IMapper implementation
        public void Map(object source, object destination)
        {
            if (source == null || destination == null)
            {
                _logger.LogWarning("Attempted to map null objects");
                return;
            }

            try
            {
                AutoMapper.Map(source, destination);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error mapping {source.GetType().Name} to {destination.GetType().Name}");
                throw;
            }
        }

        // Generic mapping
        public T Map<T>(object source)
        {
            if (source == null)
            {
                _logger.LogWarning("Attempted to map null source");
                return default!;
            }

            try
            {
                return AutoMapper.Map<T>(source);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error mapping {source.GetType().Name} to {typeof(T).Name}");
                throw;
            }
        }
    }
}
