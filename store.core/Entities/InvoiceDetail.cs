using System.ComponentModel.DataAnnotations.Schema;

namespace store.core.Entities
{
    public class InvoiceDetail
    {
        public InvoiceDetail() { }
        // (invoiceDetail, item, this)
        public InvoiceDetail(int qty, Item item, Invoice invoice)
        {
            Qty = qty;
            if (item != null)
            {
                ItemId = item.Id;
                Item = item;
                Price = item.Price * qty;
            }
            if (invoice != null)
            {
                InvoiceID = invoice.Id;
                Invoice = invoice;
            }
        }

        public int Id { get; set; }
        // [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item? Item { get; set; }
        // [ForeignKey("Invoice")]
        public int? InvoiceID { get; set; }
        public virtual Invoice? Invoice { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
    }

}
