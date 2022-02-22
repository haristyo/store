using System;
using System.Collections.Generic;
using AutoMapper;
using store.core.Entities;

namespace store.Profiles.DTO
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int InvoiceNo { get; set; }
        public List<InvoiceDetailDTO> InvoiceDetails { get; set; }

    }
    public class ItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Code { get; set; }
    }
    public class InvoiceDetailDTO
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
    }
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Invoice, InvoiceDTO>().ReverseMap();
            CreateMap<Item, ItemDTO>().ReverseMap();

            CreateMap<InvoiceDetail, InvoiceDetailDTO>()
            .ForMember(DTO => DTO.ItemPrice, opt => opt.MapFrom(e => e.Item.Price))
            .ForMember(DTO => DTO.ItemName, opt => opt.MapFrom(e => e.Item.Name))
            .ReverseMap();
        }
    }
}