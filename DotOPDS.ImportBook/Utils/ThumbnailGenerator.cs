using iTextSharp.text.pdf;

namespace DotOPDS.ImportBook.Utils
{
    public class ThumbnailProvider
    {
        public static byte[] ExtractImagesFromPDF(string sourcePdf)
        {
            try
            {
                var pdf = new PdfReader(sourcePdf);
                var raf = new RandomAccessFileOrArray(sourcePdf);


                PdfDictionary pg = pdf.GetPageN(1);


                var obj = FindImageInPDFDictionary(pg);
                if (obj != null)
                {

                    var XrefIndex = Convert.ToInt32(((PRIndirectReference)obj).Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    PdfObject pdfObj = pdf.GetPdfObject(XrefIndex);
                    var pdfStrem = (PdfStream)pdfObj;
                    byte[] bytes = PdfReader.GetStreamBytesRaw((PRStream)pdfStrem);
                    if (bytes != null)
                    {
                        //using (var memStream = new MemoryStream(bytes))
                        //{
                        //    memStream.Position = 0;
                        //    var img = Image.FromStream(memStream);
                        //    var parms = new EncoderParameters(1);
                        //    parms.Param[0] = new EncoderParameter(Encoder.Compression, 0);
                        //    //img.Save($@"D:\bookPhotos\{imageName}.jpg", ImageFormat.Jpeg);
                        //    var b = ImageToByte(img);

                        //    return b;
                        //}

                        return bytes;
                    }
                }

                pdf.Close();
                raf.Close();
            }
            catch { }

            return null;
        }

        private static PdfObject FindImageInPDFDictionary(PdfDictionary pg)
        {
            var res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
            if (res is null) return null;
            var xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
            if (xobj != null)
            {
                foreach (PdfName name in xobj.Keys)
                {

                    PdfObject obj = xobj.Get(name);
                    if (obj.IsIndirect())
                    {
                        var tg = (PdfDictionary)PdfReader.GetPdfObject(obj);

                        var type = (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));

                        //image at the root of the pdf
                        if (PdfName.IMAGE.Equals(type))
                        {
                            return obj;
                        }// image inside a form
                        else if (PdfName.FORM.Equals(type))
                        {
                            return FindImageInPDFDictionary(tg);
                        } //image inside a group
                        else if (PdfName.GROUP.Equals(type))
                        {
                            return FindImageInPDFDictionary(tg);
                        }
                    }
                }
            }

            return null;

        }

        //public static byte[] ImageToByte(Image img)
        //{
        //    var converter = new ImageConverter();
        //    return (byte[])converter.ConvertTo(img, typeof(byte[]));
        //}
    }
}
