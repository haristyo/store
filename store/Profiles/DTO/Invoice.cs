using System;
using System.Collections.Generic;
using AutoMapper;
using store.core.Entities;

namespace store.Profiles.DTO
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? InvoiceNo { get; set; }
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
        public string? ItemName { get; set; }
        public double? ItemPrice { get; set; }
        public int Qty { get; set; }
        public double? Price { get; set; }
    }
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Invoice, InvoiceDTO>();
            //CreateMap<InvoiceDTO, Invoice>().ForMember(f => f.InvoiceDetails, opt => opt.Ignore()); //tambahin di controller lalu di looping


            CreateMap<Item, ItemDTO>().ReverseMap();
            //CreateMap<InvoiceDetail, InvoiceDetailDTO>().ReverseMap();

            CreateMap<InvoiceDTO, Invoice>()
                .ForMember(f => f.InvoiceDetails, opt => opt.Ignore())
                .AfterMap(
                    (invoiceDTO, invoice, context) =>
                    {
                        foreach (InvoiceDetailDTO invoiceDetailDTO in invoiceDTO.InvoiceDetails)
                        {
                            invoice.addInvoiceDetailFull(context.Mapper.Map<InvoiceDetailDTO, InvoiceDetail>(invoiceDetailDTO));
                        }
                    }
                );
            CreateMap<InvoiceDetail, InvoiceDetailDTO>()
                .ForMember(invoiceDetailDTO => invoiceDetailDTO.ItemName, invoiceDetail => invoiceDetail.MapFrom(e => e.Item.Name))
                .ForMember(invoiceDetailDTO => invoiceDetailDTO.ItemPrice, invoiceDetail => invoiceDetail.MapFrom(e => e.Item.Price))
                .ForMember(invoiceDetailDTO => invoiceDetailDTO.Price, invoiceDetail => invoiceDetail.MapFrom(e => e.Item.Price * e.Qty))
                ;
            CreateMap<InvoiceDetailDTO, InvoiceDetail>()
                .ForMember(invoiceDetail => invoiceDetail.ItemId, invoiceDetailDTO => invoiceDetailDTO.MapFrom(e => e.ItemId))
                .ForMember(invoiceDetail => invoiceDetail.Price, invoiceDetailDTO => invoiceDetailDTO.MapFrom(e => e.Price))
                .ForMember(invoiceDetail => invoiceDetail.Qty, invoiceDetailDTO => invoiceDetailDTO.MapFrom(e => e.Qty))
                ;

        }
    }
}