using System;
using System.Collections.Generic;

namespace store.core.Entities
{
    public class Invoice
    {

        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int InvoiceNo { get; set; }

        private List<InvoiceDetail> _InvoiceDetails = new List<InvoiceDetail>();
        public IReadOnlyList<InvoiceDetail> InvoiceDetails => _InvoiceDetails.AsReadOnly();
        //public List<City> Cities { get; set; }
        public void addInvoiceDetail(int qty, Item item)
        {

            var inputInvoiceDetail = new InvoiceDetail(qty, item, this);
            _InvoiceDetails.Add(inputInvoiceDetail);
            // _InvoiceDetails.
        }
        public void addInvoiceDetailFull(InvoiceDetail invoiceDetail)
        {

            _InvoiceDetails.Add(invoiceDetail);
            // _InvoiceDetails.
        }
    }

}
