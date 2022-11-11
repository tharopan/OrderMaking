using OrderMaking.Data;
using OrderMaking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Forms.Barcode;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using System.Drawing.Imaging;
using OrderMaking.Templates;
using PdfSharp.Pdf;
using PdfSharp;
using OpenHtmlToPdf;
using PdfSharp.Pdf.IO;
using NetBarcode;

namespace OrderMaking.Business
{
    public class LabelAppService : AppService
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

        public void Delete(string barcode)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                var itemTobeDeleted = repository.Get(x => x.Product.BarCode == barcode).FirstOrDefault();

                if(itemTobeDeleted != null)
                {
                    repository.Delete(itemTobeDeleted);
                    repository.Save();
                }
            }
        }

        public void Delete()
        {
            var items = repository.Get();

            foreach (var item in items)
            {
                repository.Delete(item);
            }

            repository.Save();
        }

        public void PrintLable()
        {
            var labelItems = repository.Get(null, null, "Product").ToList();
            int page = 1;
            int itemsTobeProcessed = labelItems.Count();
            PdfDocument pdfDoc = new PdfDocument();
            BarcodeLib.Barcode barcode = new BarcodeLib.Barcode();

            //System.IO.Directory.CreateDirectory($"C:/order/TempLabel/{DateTime.Now.ToString("MMddyyyy")}");
            System.IO.Directory.CreateDirectory($"C:/order/Label");

            if (labelItems != null)
            {
                while (itemsTobeProcessed > 0)
                {
                    var tempLableItems = labelItems.Skip((page - 1) * 21).Take(21).ToList();

                    tempLableItems.ForEach(x =>
                    {
                        //var img = barcode.Encode(BarcodeLib.TYPE.CODE39, x.Product.BarCode);
                        var bbcode = new Barcode(x.Product.BarCode, NetBarcode.Type.Code128, true, 150, 100);

                        //Bitmap barcodeBmp = BarcodeWriter.CreateBarcode(
                        //    x.Product.BarCode,
                        //    BarcodeEncoding.AllOneDimensional
                        //    ).SetMargins(10).ToBitmap();

                        //System.IO.MemoryStream ms = new MemoryStream();
                        //img.Save(ms, ImageFormat.Jpeg);
                        //byte[] byteImage = ms.ToArray();

                        x.Product.BarCodeImageBase64 = bbcode.GetBase64Image();//Convert.ToBase64String(byteImage); // Get Base64
                    });

                    itemsTobeProcessed = itemsTobeProcessed - 21;

                    var template = new LabelItemTemplate() { LabelItems = tempLableItems };

                    var htmlPage = template.TransformText();

                    var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter()
                    {
                        Margins = new NReco.PdfGenerator.PageMargins { Top = 12, Bottom = 0, Left = 0, Right = 0 },
                        Size = NReco.PdfGenerator.PageSize.A4,
                        Zoom = 2
                    };

                    var pdf = htmlToPdf.GeneratePdf(htmlPage);
                    //File.WriteAllBytes($@"C:/order/TempLabel/{DateTime.Now.ToString("MMddyyyy")}/pdfnrec_{page}.pdf", pdf);


                    //var ppdf = Pdf
                    //            .From(htmlPage)
                    //            .OfSize(PaperSize.A4)
                    //            .WithMargins(x => x.Top(12.Millimeters()))
                    //            .WithResolution(96)
                    //            .Portrait()
                    //            .Content();
                    //File.WriteAllBytes($@"C:/order/TempLabel/{DateTime.Now.ToString("MMddyyyy")}/pdflib_{page}.pdf", ppdf);


                    var tempDoc = PdfReader.Open(new MemoryStream(pdf), PdfDocumentOpenMode.Import);
                    foreach (PdfPage pp in tempDoc.Pages)
                    {
                        pp.Size = PageSize.A4;
                        pdfDoc.Pages.Add(pp);
                    }

                    pdfDoc.AddPage(new PdfPage(tempDoc) { Size = PageSize.A4 });
                    page++;
                }

                var fileName = $@"C:/order/Label/{DateTime.Now.ToString("MMddyyyy")}_label.pdf";

                pdfDoc.Save(fileName);

                SendMail(new List<string>() { fileName }, $"Lables - {DateTime.UtcNow.ToShortDateString()}");

            }
        }

        public static bool IsImageBlank(Image image)
        {
            Bitmap bitmap = new Bitmap(image);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color pixel = bitmap.GetPixel(i, j);
                    if (pixel.R < 240 || pixel.G < 240 || pixel.B < 240)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
