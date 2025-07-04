using System.Text.Json;
using InvoiceAPI.Models;
using MassTransit;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Shared.Events;

namespace InvoiceAPI.Consumer
{
    public class PaymentSucceededEventConsumer : IConsumer<PaymentSucceededEvent>
    {
        public Task Consume(ConsumeContext<PaymentSucceededEvent> context)
        {
            var invoice = Invoice.Write(new
            {
                Firstname = "Jack",
                Surname = "Fatma",
                PhoneNumber = "555 666 99 77"
            },
            new
            {
                Country = "Türkiye",
                City = "İstanbul",
                District = "Bakırköy",
                BlockNo = 78,
                DoorNo = 5
            });

            Console.WriteLine(invoice);

            try
            {
             CreateInvoicePDF(new
            {
                Firstname = "Jack",
                Surname = "Fatma",
                PhoneNumber = "555 666 99 77"
            },
            new
            {
                Country = "Türkiye",
                City = "İstanbul",
                District = "Bakırköy",
                BlockNo = 78,
                DoorNo = 5
            });   
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }

        public void CreateInvoicePDF(dynamic customer, dynamic address)
        {
            string msg = Invoice.Write(customer, address);
            QuestPDF.Settings.License = LicenseType.Community;
            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.Postcard);
                    page.Margin(0.3f, Unit.Inch);

                    page.Header()
                        .Text("Your Shopping Invoice From Belgrad")
                        .FontSize(28)
                        .Bold()
                        .FontColor(Colors.Blue.Darken3);

                    page.Content()
                        .PaddingVertical(8)
                        .Column(column =>
                        {
                            column.Item()
                                .Text(msg)
                                .Justify();
                        });
                });
            }).GeneratePdf("C:\\Users\\CANAVAR\\Desktop\\Invoice.pdf");
        }
    }
}