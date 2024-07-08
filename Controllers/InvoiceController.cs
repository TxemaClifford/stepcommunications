using InvoiceGenerator.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace InvoiceGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly PdfGeneratorService _pdfGeneratorService;

        public InvoiceController(PdfGeneratorService pdfGeneratorService)
        {
            _pdfGeneratorService = pdfGeneratorService;
        }

        [HttpPost("Generate")]
        public IActionResult GenerateInvoice([FromForm] string jsonData)
        {
            var pdfBytes = _pdfGeneratorService.GeneratePdfFromJson(jsonData);
            return File(pdfBytes, "application/pdf", "invoice.pdf");
        }
    }
}
