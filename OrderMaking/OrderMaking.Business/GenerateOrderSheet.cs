using ExcelLibrary.SpreadSheet;
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
                                ProductPrice = item.Product?.Price ?? 0,
                                //ProductCategoryId = groupedItem.Key,
                                ProductCategoryName = groupedItem.Key,
                                ProductId = item.Product?.Id ?? 0,
                                ProductName = item.Product == null ? item.ItemDescription : item.Product.Description,
                                ProductSize = item.Product?.SizeGroup,
                                Barcode = item.Product?.BarCode,
                                NumberOfItems = item.NumberOfItems
                            });
                        }

                        var file = $"{rootPath}\\{fileName}.csv";
                        fileList.Add(file);
                        GenerateCSV(file, orderList);

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
                                ProductPrice = item.Product?.Price ?? 0,
                                //ProductCategoryId = groupedItem.Key,
                                ProductCategoryName = groupedItem.Key,
                                ProductId = item.Product?.Id ?? 0,
                                ProductName = item.Product == null ? item.ItemDescription : item.Product.Description,
                                ProductSize = item.Product?.SizeGroup,
                                Barcode = item.Product?.BarCode,
                                NumberOfItems = item.NumberOfItems
                            });
                        }

                        var file = $"{rootPath}\\{groupedItem.Key}.csv";
                        //fileList.Add(file);
                        GenerateCSV(file, orderList);
                        mergedOrderList.AddRange(orderList);
                        mergedOrderList.Add(new ShoppingCartFlat() { });
                        //mergedOrderList.Add(new ShoppingCartFlat() { ProductName = groupedItem.Key });
                        orderList = new List<ShoppingCartFlat>();
                    }

                    var mergeFile = $"{rootPath}\\ShoppingList.csv";
                    GenerateCSV(mergeFile, mergedOrderList);

                    var mergeFileExcel = $"{rootPath}\\ShoppingList.xls";
                    GenerateExcel(mergeFileExcel, mergedOrderList);

                    //fileList.Add(mergeFile);
                    fileList.Add(mergeFileExcel);

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
            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet("Shopping List");

            for (int i = 0; i < 100; i++)
                worksheet.Cells[i, 0] = new Cell("");

            //Adding Heading
            worksheet.Cells[0, 0] = new Cell("Product Name");
            worksheet.Cells[0, 1] = new Cell("Size");
            worksheet.Cells[0, 2] = new Cell("Number of Items");
            worksheet.Cells[0, 3] = new Cell("Selling Price");
            worksheet.Cells[0, 4] = new Cell("Barcode");
            worksheet.Cells.ColumnWidth[0, 0] = 7000;
            worksheet.Cells.ColumnWidth[4, 4] = 3500;


            //After a heading
            int row = 1;

            foreach (var shopingCart in orderList)
            {
                worksheet.Cells[row, 0] = new Cell(shopingCart.ProductName);
                worksheet.Cells[row, 1] = new Cell(shopingCart.ProductSize);
                worksheet.Cells[row, 2] = new Cell(shopingCart.NumberOfItems);
                worksheet.Cells[row, 3] = new Cell(shopingCart.ProductPrice);
                worksheet.Cells[row, 4] = new Cell(shopingCart.Barcode);
                row++;
            }

            workbook.Worksheets.Add(worksheet);
            workbook.Save(file);
        }

        public void GenerateCSV(string file, IList<ShoppingCartFlat> orderList)
        {
            string delimiter = ",";
            StringBuilder sb = new StringBuilder();

            string clientHeader = $"\"Product Name\",\"Size\",\"Number of Items\",\"Selling Price\",\"Barcode\"";
            sb.AppendLine(clientHeader);
            foreach (var item in orderList)
            {
                sb.AppendLine($"{item.ProductName }{delimiter }{ item.ProductSize}{delimiter}{item.NumberOfItems} {delimiter} {item.ProductPrice} {delimiter}{item.Barcode}");
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
