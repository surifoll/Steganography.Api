using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using Steganography.WebApi.Models;

namespace Steganography.Engine
{
    public static class Utils
    {
        public static string GetExtension(DataImage image)
        {
            var extension = "";
            switch (image.MimeType)
            {
                case "image/jpeg":
                    extension = ".jpeg";
                    break;
                case "image/jpg":
                    extension = ".jpg";
                    break;
                case "image/gif":
                    extension = ".gif";
                    break;
                case "image/png":
                    extension = ".png";
                    break;
            }

            return extension;
        }

        public static Bitmap Hide(DataModel data, HttpPostedFileBase file, string filePath, string _PASSWORD,
            string filePathResult)
        {
            file.SaveAs(filePath);
            // Encrypt your data to increase security
            // Remember: only the encrypted data should be stored on the image
            var encryptedData = StringCipher.Encrypt(data.Info, _PASSWORD);

            // Create an instance of the original image without indexed pixels
            var originalImage = SteganographyHelper.CreateNonIndexedImage(Image.FromFile(filePath));
            // Conceal the encrypted data on the image !
            var imageWithHiddenData = SteganographyHelper.MergeText(encryptedData, originalImage);

            // Save the image with the hidden information somewhere :)
            // In this case the generated file will be image_example_with_hidden_information.png
            imageWithHiddenData.Save(filePathResult);

            return imageWithHiddenData;
        }

        public static string Hide2(DataModel data, Image file, string filePath, string _PASSWORD,
            string filePathResult)
        {
            file.Save(filePath, ImageFormat.Png);
            // Encrypt your data to increase security
            // Remember: only the encrypted data should be stored on the image
            var encryptedData = StringCipher.Encrypt(data.Info, _PASSWORD);

            // Create an instance of the original image without indexed pixels
            var originalImage = SteganographyHelper.CreateNonIndexedImage(file);
            // Conceal the encrypted data on the image !
            var imageWithHiddenData = SteganographyHelper.MergeText(encryptedData, originalImage);

            // Save the image with the hidden information somewhere :)
            // In this case the generated file will be image_example_with_hidden_information.png
            imageWithHiddenData.Save(filePathResult);

            return "";
        }

        public static Bitmap Extract(DataModel data, HttpPostedFileBase file, string filePath, string _PASSWORD,
            string filePathResult)
        {
            // Retrieve the encrypted data from the image
            var encryptedData = SteganographyHelper.ExtractText(
                new Bitmap(
                    Image.FromFile(filePathResult)
                )
            );

            // Decrypt the retrieven data on the image
            var decryptedData = StringCipher.Decrypt(encryptedData, _PASSWORD);

            data.Info = decryptedData;
            return (Bitmap) Image.FromFile(filePath);
        }

        public static string Extract2( Image file, string _PASSWORD,
            string filePathResult)
        {
            // Retrieve the encrypted data from the image
            var encryptedData = SteganographyHelper.ExtractText(
                new Bitmap(
                    file
                )
            );

            // Decrypt the retrieven data on the image
            var decryptedData = StringCipher.Decrypt(encryptedData, _PASSWORD);

            //data.Info = decryptedData;
            return decryptedData;//data.Info; //(Bitmap) Image.FromFile(filePath);
        }
    }
}