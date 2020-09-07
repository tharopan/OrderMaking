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

        public void GenerateCigarettes()
        {
            var rootPath = $@"C:\order\cigarettes\{DateTime.UtcNow.ToString("MMddyyyy")}";
            var fileList = new List<string>();
            var shoppingCarts = repository.Get(null, null, "Product, Category");

            if (shoppingCarts != null && shoppingCarts.Any())
            {
                var cigaretteList = shoppingCarts.Where(x => x.Category != null && x.Category.Name.ToLower() == "Cigarettes".ToLower());

                if (cigaretteList != null && cigaretteList.Any())
                {
                    System.IO.Directory.CreateDirectory(rootPath);

                    var groupedList = cigaretteList.ToList().GroupBy(x => x.Category.Name);

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
                                ProductSize = item.Product.SizeGroup,
                                Barcode = item.Product.BarCode,
                                NumberOfItems = item.NumberOfItems
                            });
                        }

                        var file = $"{rootPath}\\{fileName}.csv";
                        fileList.Add(file);
                        GenerateExcel(file, orderList);

                        orderList = new List<ShoppingCartFlat>();
                    }

                    SendMail(fileList);
                }
            }
        }

        public void GenerateOrder()
        {
            try
            {
                var rootPath = $@"C:\order\{DateTime.UtcNow.ToString("MMddyyyy")}";
                var fileList = new List<string>();
                var shoppingCarts = repository.Get(null, null, "Product, Category");
                var mergedOrderList = new List<ShoppingCartFlat>();

                if (shoppingCarts != null && shoppingCarts.Any())
                {
                    System.IO.Directory.CreateDirectory(rootPath);

                    var groupedList = shoppingCarts.Distinct().OrderBy(x => x.Category.SortOrder).ToList().GroupBy(x => x.Category.Name);

                    foreach (var groupedItem in groupedList)
                    {
                        var orderList = new List<ShoppingCartFlat>();
                        string fileName = groupedItem.Key;
                        mergedOrderList.Add(new ShoppingCartFlat() { ProductName = groupedItem.Key });

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
                                ProductSize = item.Product.SizeGroup,
                                Barcode = item.Product.BarCode,
                                NumberOfItems = item.NumberOfItems
                            });
                        }

                        var file = $"{rootPath}\\{groupedItem.Key}.csv";
                        //fileList.Add(file);
                        GenerateExcel(file, orderList);
                        mergedOrderList.AddRange(orderList);
                        mergedOrderList.Add(new ShoppingCartFlat() { });
                        //mergedOrderList.Add(new ShoppingCartFlat() { ProductName = groupedItem.Key });
                        orderList = new List<ShoppingCartFlat>();
                    }

                    var mergeFile = $"{rootPath}\\ShoppingList.csv";
                    GenerateExcel(mergeFile, mergedOrderList);
                    var mergeFileExcel = $"{rootPath}\\ShoppingList.xls";

                    //string worksheetsName = "TEST";

                    //bool firstRowIsHeader = false;

                    //var format = new ExcelTextFormat();
                    //format.Delimiter = ',';
                    //format.EOL = "\r";
                    //// DEFAULT IS "\r\n";
                    //// format.TextQualifier = '"';

                    //using (ExcelPackage package = new ExcelPackage(new FileInfo(mergeFileExcel)))
                    //{
                    //    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                    //    worksheet.Cells["A1"].LoadFromText(new FileInfo(mergeFile), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                    //    package.Save();
                    //}

                    fileList.Add(mergeFile);
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

            string clientHeader = $"\"Product Name\",\"Size\",\"Number of Items\",\"Selling Price\",\"Barcode\"";
            sb.AppendLine(clientHeader);
            foreach (var item in orderList)
            {
                sb.AppendLine($"{item.ProductName }{delimiter }{ item.ProductSize}{delimiter}{item.ProductPrice} {delimiter} {item.NumberOfItems} {delimiter}{item.Barcode}");
            }

            File.WriteAllText(file, sb.ToString());
        }

        public void SendMail(IList<string> files)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress("nishalocalconvenientstore@gmail.com", "Nisha Local Convenience Store"),
                Subject = $"Shopping list for {DateTime.UtcNow.ToShortDateString()}",
                Body = "<h1>Hi, <br> Find the attached shopping list</h1>",
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
            mail.To.Add(new MailAddress("ravi.chandran69@outlook.com"));

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
