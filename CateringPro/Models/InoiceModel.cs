using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CateringPro.Models
{
    public class InvoiceModel
    {
        public string Number { get; set; }
        public CompanyModel Seller { get; set; }
        public CompanyModel Buyer { get; set; }
        public IEnumerable<InvoiceItemModel> Items { get; set; }

        public static InvoiceModel Example()
        {
            return new InvoiceModel()
            {
                Number = "123",
                Seller = new CompanyModel()
                {
                    Name = "Next Step Webs, Inc.",
                    Address1 = "12345 Sunny Road",
                    Country = "Sunnyville, TX 12345"
                },
                Buyer = new CompanyModel()
                {
                    Name = "Acme Corp.",
                    Address1 = "16 Johnson Road",
                    Country = "Paris, France 8060"
                },
                Items = new List<InvoiceItemModel>()
                {
                    new InvoiceItemModel()
                    {
                        Name = "Website design",
                        Price = 300
                    },
                    new InvoiceItemModel()
                    {
                        Name = "Implementing specific components",
                        Price = 600
                    },
                    new InvoiceItemModel()
                    {
                        Name = "Maintenance and support",
                        Price = 150
                    }
                }
            };
        }
    }

    public class CompanyModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }

        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public int? PictureId { get; set; }
    }

    public class InvoiceItemModel
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }
        [DisplayFormat(DataFormatString = "0.00")]
        public decimal Price { get; set; }
        [DisplayFormat(DataFormatString ="0.00") ]
        public decimal Amount { get; set; }
    }
}