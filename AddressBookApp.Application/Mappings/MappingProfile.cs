﻿﻿using AddressBookApp.Application.DTOs;
using AddressBookApp.Core.Models;
using AutoMapper;

namespace AddressBookApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Contact mappings
            CreateMap<Contact, ContactDto>();
            CreateMap<ContactCreateDto, Contact>();
            CreateMap<ContactUpdateDto, Contact>();

            // Address mappings
            CreateMap<Address, AddressDto>();
            CreateMap<AddressCreateDto, Address>();
            CreateMap<AddressUpdateDto, Address>();

            // PhoneNumber mappings
            CreateMap<PhoneNumber, PhoneNumberDto>();
            CreateMap<PhoneNumberCreateDto, PhoneNumber>();
            CreateMap<PhoneNumberUpdateDto, PhoneNumber>();

            // EmailAddress mappings
            CreateMap<EmailAddress, EmailAddressDto>();
            CreateMap<EmailAddressCreateDto, EmailAddress>();
            CreateMap<EmailAddressUpdateDto, EmailAddress>();

            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<RegisterDto, User>();

            // ApiClient mappings
            CreateMap<ApiClient, ApiClientDto>();
            CreateMap<ApiClientCreateDto, ApiClient>();

            // ApiUsage mappings
            CreateMap<ApiUsage, ApiUsageDto>()
                .ForMember(dest => dest.ApiClientName, opt => opt.MapFrom(src => src.ApiClient.Name));

            // DataExport mappings
            CreateMap<DataExport, DataExportDto>()
                .ForMember(dest => dest.ExportType, opt => opt.MapFrom(src => src.Type.ToString()));

            // ImportHistory mappings
            CreateMap<ImportHistory, ImportHistoryDto>();
        }
    }
}
