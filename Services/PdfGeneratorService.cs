using DinkToPdf;
using DinkToPdf.Contracts;
using InvoiceGenerator.Models;
using Newtonsoft.Json;
using System.IO;

namespace InvoiceGenerator.Services
{
    public class PdfGeneratorService
    {
        private readonly IConverter _converter;

        public PdfGeneratorService(IConverter converter)
        {
            _converter = converter;
        }

        public byte[] GeneratePdfFromJson(string json)
        {
            var invoiceRequest = JsonConvert.DeserializeObject<InvoiceRequest>(json);
            var htmlContent = GenerateHtmlContent(invoiceRequest);

            var pdfDoc = new HtmlToPdfDocument
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = {
                    new ObjectSettings {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            return _converter.Convert(pdfDoc);
        }

        private string GenerateHtmlContent(InvoiceRequest invoiceRequest)
        {
            var order = invoiceRequest.Request[0];
            var itemsHtml = string.Empty;

            foreach (var item in order.items)
            {
                itemsHtml += $"<tr><td>{item.order_item_id}</td><td>{item.product_name}</td><td>{item.purchase_order}</td><td>{item.item}</td><td>{item.month_name}</td><td>{item.year}</td><td>{item.gross_price}</td><td>{item.net_price}</td></tr>";
            }

            return $@"
    <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 20px;
                    background-color: #f7f7f7;
                }}
                .invoice-box {{
                    max-width: 800px;
                    margin: auto;
                    padding: 30px;
                    border: 1px solid #eee;
                    background: #fff;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
                }}
                .invoice-header {{
                    text-align: center;
                    margin-bottom: 20px;
                }}
                .invoice-header h1 {{
                    margin: 0;
                    font-size: 24px;
                    color: #333;
                }}
                .invoice-details, .billing-details {{
                    width: 100%;
                    margin-bottom: 20px;
                }}
                .invoice-details p, .billing-details p {{
                    margin: 0;
                }}
                .invoice-details .title, .billing-details .title {{
                    font-weight: bold;
                    margin-bottom: 5px;
                }}
                table {{
                    width: 100%;
                    border-collapse: collapse;
                    margin-bottom: 20px;
                }}
                table, th, td {{
                    border: 1px solid #ddd;
                }}
                th, td {{
                    padding: 10px;
                    text-align: left;
                }}
                th {{
                    background-color: #f4f4f4;
                }}
                .total {{
                    text-align: right;
                    font-weight: bold;
                }}
                .notes {{
                    margin-top: 20px;
                    font-size: 12px;
                    color: #555;
                }}
            </style>
        </head>
        <body>
            <div class='invoice-box'>
                <div class='invoice-header'>
                    <h1>Invoice</h1>
                </div>
                <div class='invoice-details'>
                    <p class='title'>Invoice Details</p>
                    <p>Invoice Number: {order.order_id}</p>
                    <p>Order Confirmed Date: {order.order_confirmed_date:yyyy-MM-dd}</p>
                    <p>Sales Person: {order.sales_person}</p>
                </div>
                <div class='billing-details'>
                    <p class='title'>Billing Information</p>
                    <p>Invoice Advertiser: {order.invoice_advertiser}</p>
                    <p>Invoice Company Name: {order.invoice_company_name}</p>
                    <p>Invoice Address: {order.invoice_address1}, {order.invoice_city}, {order.invoice_state_county}, {order.invoice_post_code}, {order.invoice_country_name}</p>
                    <p>Invoice Contact: {order.invoice_contact_name} ({order.invoice_contact_email_address})</p>
                </div>
                <table>
                    <tr>
                        <th>Order Item ID</th>
                        <th>Product Name</th>
                        <th>Purchase Order</th>
                        <th>Item</th>
                        <th>Month</th>
                        <th>Year</th>
                        <th>Gross Price</th>
                        <th>Net Price</th>
                    </tr>
                    {itemsHtml}
                </table>
                <p class='total'>Total Amount: £{order.FormattedTotalAmount}</p>
                <div class='notes'>
                    <p>Additional notes or terms can be placed here.</p>
                </div>
            </div>
        </body>
    </html>";

        }
    }
}