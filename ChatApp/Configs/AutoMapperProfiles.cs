using AutoMapper;
using ChatApp.Models;
using ChatApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ChatRoom, ChatRoomDTO>()
                .ForMember(dest => dest.ChatRoomId,
                opt => opt.MapFrom(src => src.ChatRoomId))
                .ForMember(dest => dest.ChatRoomName,
                opt => opt.MapFrom(src => src.ChatRoomName))
                .ForMember(dest => dest.ChatUsers,
                opt => opt.MapFrom(src => src.ChatUsers))
                .ForMember(dest => dest.CreatedBy,
                opt => opt.MapFrom(src => src.CreatedBy));
        }
    }
}
