using OrderMaking.Data;
using OrderMaking.Models;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            var labelItems = repository.Get(null, null, "Product").ToList();
            int page = 1;
            int itemsTobeProcessed = labelItems.Count();
            PdfDocument pdfDoc = new PdfDocument();

            if (labelItems != null)
            {
                while (itemsTobeProcessed > 0)
                {
                    var tempLableItems = labelItems.Take(21).Skip((page - 1) * 21);
                    itemsTobeProcessed = itemsTobeProcessed - 21;
                    var pdfPage = GeneratePdf("TestHtmls");

                    pdfDoc.AddPage(new PdfPage());


                    page++;
                }
            }

            pdfDoc.Save($@"C:/order/Label/{DateTime.Now.ToString("mmDDYYYY")}_label.pdf");
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
