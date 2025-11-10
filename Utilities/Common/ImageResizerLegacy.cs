using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Common
{
    public static class ImageResizerLegacy
    {


        /// <summary>
        /// Resize ảnh từ dạng base64 string và trả về ảnh đã resize dưới dạng base64 string.
        /// Sử dụng System.Drawing.Common (cần lưu ý về đa nền tảng).
        /// </summary>
        /// <param name="base64Image">Dữ liệu ảnh dưới dạng base64 string.</param>
        /// <param name="width">Chiều rộng mong muốn.</param>
        /// <param name="height">Chiều cao mong muốn.</param>
        /// <returns>Ảnh đã resize dưới dạng base64 string.</returns>
        public static string ResizeImageBase64Legacy(string base64Image, int width, int height)
        {
            if (string.IsNullOrEmpty(base64Image))
            {
                throw new ArgumentException("Input base64 image string cannot be null or empty.", nameof(base64Image));
            }

            // Loại bỏ phần "data:image/png;base64," nếu có
            var base64Data = base64Image.Contains(",") ? base64Image.Split(',')[1] : base64Image;

            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64Data);

                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (System.Drawing.Image originalImage = System.Drawing.Image.FromStream(ms))
                    {
                        // Tạo một Bitmap mới với kích thước mong muốn
                        using (Bitmap resizedImage = new Bitmap(width, height))
                        {
                            using (Graphics graphics = Graphics.FromImage(resizedImage))
                            {
                                // Thiết lập chất lượng vẽ (tùy chọn)
                                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                                // Vẽ ảnh gốc vào Bitmap mới với kích thước đã thay đổi
                                graphics.DrawImage(originalImage, 0, 0, width, height);
                            }

                            using (MemoryStream outputMs = new MemoryStream())
                            {
                                // Lưu ảnh đã resize vào MemoryStream
                                // Lưu ý: System.Drawing.Common có thể không hỗ trợ lưu một số định dạng mà không có bộ giải mã/mã hóa phù hợp.
                                // ImageFormat.Png hoặc ImageFormat.Jpeg là các lựa chọn phổ biến.
                                resizedImage.Save(outputMs, System.Drawing.Imaging.ImageFormat.Png);

                                byte[] resizedImageBytes = outputMs.ToArray();
                                return Convert.ToBase64String(resizedImageBytes);
                            }
                        }
                    }
                }
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("Invalid base64 string format.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during image resizing: {ex.Message}", ex);
            }
        }
        public static string AutoReduceImageQualityBase64(string base64Input, int maxKb = 600)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64Input);

                using (var msInput = new MemoryStream(imageBytes))
                using (Bitmap bmp = new Bitmap(msInput))
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                    long quality = 100;
                    const long minQuality = 50;

                    while (quality >= minQuality)
                    {
                        using (var ms = new MemoryStream())
                        {
                            EncoderParameters encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                            bmp.Save(ms, jpgEncoder, encoderParams);

                            long sizeKb = ms.Length / 1024;
                            if (sizeKb <= maxKb)
                            {
                                return $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";
                            }
                        }
                        quality -= 1; // giảm dần chất lượng
                    }

                    //// Nếu vẫn không đạt, lưu ở minQuality
                    //using (var ms = new MemoryStream())
                    //{
                    //    EncoderParameters finalParams = new EncoderParameters(1);
                    //    finalParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, minQuality);
                    //    bmp.Save(ms, jpgEncoder, finalParams);
                    //    return $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";

                    //}
                }
            }
            catch
            {

            }
            return base64Input;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }
            return null;
        }
        public static async Task<string> DownloadAndOptimizeImageAsync(string url, string _UrlStaticImage, int maxKb = 500)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url_fixed = url.Contains("http") ? url : _UrlStaticImage + url;
                    HttpResponseMessage response = await client.GetAsync(url_fixed);
                    response.EnsureSuccessStatusCode();
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                    string contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg"; // fallback jpeg

                    long sizeKb = imageBytes.Length / 1024;
                    string base64 = Convert.ToBase64String(imageBytes);

                    if (sizeKb > maxKb)
                    {
                        // Gọi hàm giảm chất lượng
                        base64 = AutoReduceImageQualityBase64(base64, maxKb);
                        return await UpLoadHelper.UploadBase64Src(base64, _UrlStaticImage);
                    }
                }
                catch (Exception ex) { }
                return url;

            }
        }
    }
}
