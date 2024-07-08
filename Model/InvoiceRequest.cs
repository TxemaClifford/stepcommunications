using System;
using System.Collections.Generic;

namespace InvoiceGenerator.Models
{
    public class InvoiceRequest
    {
        public List<Order>? Request { get; set; }
    }

    public class Order
    {
        public long order_id { get; set; }
        public string? sales_person { get; set; }
        public DateTime order_confirmed_date { get; set; }
        public string? currency_name { get; set; }
        public string? special_instructions { get; set; }
        public string? invoice_advertiser { get; set; }
        public string? invoice_company_name { get; set; }
        public string? invoice_address1 { get; set; }
        public string? invoice_address2 { get; set; }
        public string? invoice_address3 { get; set; }
        public string? invoice_city { get; set; }
        public string? invoice_state_county { get; set; }
        public string? invoice_post_code { get; set; }
        public string? invoice_country_name { get; set; }
        public string? invoice_contact_name { get; set; }
        public string? invoice_contact_email_address { get; set; }
        public List<Item>? items { get; set; }

        public decimal TotalAmount => items?.Sum(item => item.net_price) ?? 0;
        public string FormattedTotalAmount => items?.Sum(item => item.net_price).ToString("F2") ?? "0.00";
    }

    public class Item
    {
        public long order_item_id { get; set; }
        public string? product_name { get; set; }
        public string? purchase_order { get; set; }
        public string? item { get; set; }
        public string? month_name { get; set; }
        public int year { get; set; }
        public decimal gross_price { get; set; }
        public decimal net_price { get; set; }
    }

    
}
