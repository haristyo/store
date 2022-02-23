using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace store.core.Entities
{

    public class Item
    {
        // public Item()
        // {
        //     this.InvoiceDetail = new HashSet<InvoiceDetail>();
        // }
        public int Id { get; set; }
        // public virtual InvoiceDetail? InvoiceDetail { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string Code { get; set; }
    }

}
