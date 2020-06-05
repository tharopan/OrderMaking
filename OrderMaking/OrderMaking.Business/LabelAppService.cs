using OrderMaking.Data;
using OrderMaking.Models;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Barcode;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using IronBarCode;
using System.Drawing.Imaging;
using OrderMaking.Templates;

namespace OrderMaking.Business
{
    public class LabelAppService
    {
        Repository<LabelItem> repository;
        Repository<DeprecatedProduct> productRepository;

        public LabelAppService()
        {
            repository = new Repository<LabelItem>();
            productRepository = new Repository<DeprecatedProduct>();
        }

        public void Add(LabelItem labelItem)
        {
            DeprecatedProduct product;
            if (!string.IsNullOrEmpty(labelItem.Barcode))
            {
                product = productRepository.Get(x => x.BarCode == labelItem.Barcode).FirstOrDefault();
                if (product != null)
                {
                    var existingItem = repository.Get(x => x.ProductId == product.Id, null, "Product").FirstOrDefault();
                    if (existingItem == null)
                    {
                        labelItem.ProductId = product.Id;
                        repository.Insert(labelItem);
                        repository.Save();
                    }
                }
                else
                {
                    //var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    //{
                    //    Content = new StringContent(string.Format("No product with ID = {0}", shoppingCart.Barcode),
                    //    ReasonPhrase = "Product ID Not Found"
                    //};
                    //throw new HttpResponseException(resp);
                }
            }
            else if (labelItem.ProductId > 0)
            {
                product = productRepository.GetById(labelItem.ProductId);
                if (product != null)
                {
                    labelItem.ProductId = product.Id;
                    repository.Insert(labelItem);
                    repository.Save();
                }
            }
        }

        public void PrintLable()
        {
            StringWriter stringWriter = new StringWriter();

            var htmlstring = string.Format(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"">
                                            <html>
                                            <title>{0}</title>
                                            <link rel=""stylesheet"" type=""text/css"" href=""style.css"">
                                            </head>
                                            <body>cdsfsdfsdfsdfsdfsdfsdfsdffwerwerwerwefsdfsdfsad</body>
                                            </html>
                                            ", "ndhfdsfsdfsdfsdfsdfsdfsd");


            var labelItems = repository.Get(null, null, "Product").ToList();
            int page = 1;
            int itemsTobeProcessed = labelItems.Count();
            PdfDocument pdfDoc = new PdfDocument();

            if (labelItems != null)
            {
                while (itemsTobeProcessed > 0)
                {
                    var tempLableItems = labelItems.Take(21).Skip((page - 1) * 21).ToList();

                    tempLableItems.ForEach(x =>
                    {
                        Bitmap barcodeBmp = BarcodeWriter.CreateBarcode(
                            x.Product.BarCode,
                            BarcodeEncoding.PDF417
                            ).ResizeTo(300, 200)
                            .SetMargins(100)
                            .ToBitmap();

                        System.IO.MemoryStream ms = new MemoryStream();
                        barcodeBmp.Save(ms, ImageFormat.Jpeg);
                        byte[] byteImage = ms.ToArray();
                        x.Product.BarCodeImageBase64 = Convert.ToBase64String(byteImage); // Get Base64
                    });

                    itemsTobeProcessed = itemsTobeProcessed - 21;

                    var template = new LabelItemTemplate();

                    template.Session = new Dictionary<string, object>();
                    template.Session["LabelItems"] = itemsTobeProcessed;

                    // Add other parameter values to t.Session here.
                    template.Initialize(); // Must call this to t


                    var pdfPage = GeneratePdf(template.TransformText());

                    var tempDoc = PdfReader.Open(new MemoryStream(pdfPage), PdfDocumentOpenMode.Import);
                    foreach (PdfPage pp in tempDoc.Pages)
                    {
                        pdfDoc.Pages.Add(pp);
                    }

                    pdfDoc.AddPage(new PdfPage(tempDoc));
                    page++;
                }
            }

            System.IO.Directory.CreateDirectory("C:/order/Label");
            pdfDoc.Save($@"C:/order/Label/{DateTime.Now.ToString("MMddyyyy")}_label.pdf");
        }

        public static Byte[] GeneratePdf(String html)
        {
            Byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return res;
        }




    }
}
