using OrderMaking.Data;
using OrderMaking.Models;
using OrderMaking.Models.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace OrderMaking.Business
{
    public class GenerateOrderSheet
    {
        Repository<ShoppingCart> repository;

        public GenerateOrderSheet()
        {
            repository = new Repository<ShoppingCart>();
        }

        public void GenerateOrder()
        {
            try
            {
                var rootPath = $@"C:\order\{DateTime.UtcNow.ToString("MMddyyyy")}";
                var fileList = new List<string>();
                var shoppingCarts = repository.Get(null, null, "Product, Category");

                if (shoppingCarts != null && shoppingCarts.Any())
                {
                    System.IO.Directory.CreateDirectory(rootPath);

                    var groupedList = shoppingCarts.ToList().GroupBy(x => x.Category.Name);

                    foreach (var groupedItem in groupedList)
                    {
                        var orderList = new List<ShoppingCartFlat>();
                        string fileName = groupedItem.Key;

                        foreach (var item in groupedItem)
                        {
                            orderList.Add(new ShoppingCartFlat()
                            {
                                //CustomOrder = item.Product.CustomOrder,
                                OrderDate = item.OrderDate,
                                ProductPrice = item.Product.Price,
                                //ProductCategoryId = groupedItem.Key,
                                ProductCategoryName = groupedItem.Key,
                                ProductId = item.Product.Id,
                                ProductName = item.Product.Description,
                                ProductSize = item.Product.SizeGroup
                            });
                        }
                        var file = $"{rootPath}\\{fileName}.csv";
                        fileList.Add(file);
                        GenerateExcel(file, orderList);

                        orderList = new List<ShoppingCartFlat>();
                    }

                    SendMail(fileList);
                    MoveCompleted();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //Move all shopping cart order to another table                
            }
        }

        public void GenerateExcel(string file, IList<ShoppingCartFlat> orderList)
        {

            string delimiter = ",";
            StringBuilder sb = new StringBuilder();

            string clientHeader = $"\"Product Name\",\"Size\",\"Selling Price\",\"Least Price\",\"Least Price At\"";
            sb.AppendLine(clientHeader);
            foreach (var item in orderList)
            {
                sb.AppendLine($"{item.ProductName }{delimiter }{ item.ProductSize}{delimiter}{item.ProductPrice} {delimiter} {string.Empty} {delimiter}{string.Empty}");
            }

            File.WriteAllText(file, sb.ToString());
        }

        public void SendMail(IList<string> files)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress("nishalocalconvenientstore@gmail.com", "Nisha Local Convenient Store"),
                Subject = $"Order list for {DateTime.UtcNow.ToShortDateString()}",
                Body = "<h1>Hi, <br> Find the attached order list</h1>",
                IsBodyHtml = true,
            };

            var toAddressesStr = ConfigurationManager.AppSettings["ToAddresses"];
            if (string.IsNullOrEmpty(toAddressesStr))
            {
                var toAddresses = toAddressesStr.Split(';');
                foreach (var toAddress in toAddresses)
                {
                    mail.To.Add(toAddress);
                }
            }

            mail.To.Add(new MailAddress("tharopan@hotmail.com"));

            foreach (var file in files)
            {
                mail.Attachments.Add(new Attachment(file));
            }

            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential("nishalocalconvenientstore@gmail.com", "thisismystore");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }

        public void MoveCompleted()
        {
            repository.Execute("MoveToCompletedOrder");
        }
    }
}
