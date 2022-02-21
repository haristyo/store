using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.Specification;
using store.core.Entities;

namespace store.core.Spesifications
{
    public class ItemSpesification : Specification<Item>
    {
        public ItemSpesification()
        {
        }
        public int? IdEquals { get; set; }
        public string? NameContains { get; set; }
        public string? NameEquals { get; set; }
        public int? PriceMax { get; set; }
        public int? PriceMin { get; set; }
        public Specification<Item> Build()
        {
            if (IdEquals.HasValue)
            {
                Query.Where(f=>f.Id== IdEquals.Value);
            }
            if (PriceMax.HasValue)
            {
                Query.Where(f => f.Price <= PriceMax.Value);
            }
            if (PriceMin.HasValue)
            {
                Query.Where(f => f.Price >= PriceMin.Value);
            }
            if (!String.IsNullOrEmpty(NameContains))
            {
                Query.Where(f => f.Name !=null && f.Name.ToLower().Contains(NameContains.ToLower()));
            }
            if (!String.IsNullOrEmpty(NameEquals))
            {
                Query.Where(f => f.Name != null && f.Name.ToLower() == NameEquals.ToLower());
            }
            return this;
        }

    }

    public class InvoiceSpesification : Specification<Invoice>
    {
        public InvoiceSpesification()
        {
        }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? InvcoiceNoEquals { get; set; }
        public int? Id { get; set; }
        public Specification<Invoice> Build()
        {
            if (DateStart.HasValue)
            {
                Query.Where(f => f.InvoiceDate >= DateStart.Value);
            }
            if (DateEnd.HasValue)
            {
                Query.Where(f => f.InvoiceDate <= DateEnd.Value);
            }

            if (InvcoiceNoEquals.HasValue)
            {
                Query.Where(f=>f.InvoiceNo == InvcoiceNoEquals.Value);
            }
            if (Id.HasValue)
            {
                Query.Where(f => f.Id == Id.Value);
            }

            return this;
        }

    }
    public class InvoiceDetailSpesification : Specification<InvoiceDetail>
    {
        public InvoiceDetailSpesification()
        {
        }
        
       
        public string? ItemName { get; set; }
        public int? QtyEquals { get; set; }
        public int? QtyMax { get; set; }
        public int? QtyMin { get; set; }
        public int? IdEquals { get; set; }
        public int? InvoiceIdEquals { get; set; }
        public Specification<InvoiceDetail> Build()
        {
            if (!String.IsNullOrEmpty(ItemName))
            {
                Query.Where(f => f.Item.Name != null && f.Item.Name.ToLower() == ItemName.ToLower());
            }
            if (QtyEquals.HasValue)
            {
                Query.Where(f => f.Qty >= QtyEquals.Value);
            }
            if (QtyMax.HasValue)
            {
                Query.Where(f => f.Qty <= QtyMax.Value);
            }

            if (QtyMin.HasValue)
            {
                Query.Where(f => f.Qty == QtyMin.Value);
            }
            if (IdEquals.HasValue)
            {
                Query.Where(f => f.Id == IdEquals.Value);
            }
            if (InvoiceIdEquals.HasValue)
            {
                Query.Where(f=>f.InvoiceID = InvoiceIdEquals.Value)
            }

            return this;
        }

    }
}
