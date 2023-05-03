﻿using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;

namespace DatingApp.API.Helper
{
	public class AutoMapperHelper : Profile
	{
		public AutoMapperHelper()
		{
			CreateMap<AppUser, MemberDto>()
				.ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
				.ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

			CreateMap<Photo, PhotoDto>();
		}
	}
}
