using System;
using System.Drawing;

namespace Steganography.ConsoleApplication
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Input();

            Output();
        }

        private static void Output()
        {
            // The password used to hide the information on the previous step
            var _PASSWORD = "password";

            // The path to the image that contains the hidden information
            ///Users/surajfehintola/RiderProjects/Steganography/Steganography.ConsoleApplication/img/screen.png
            var pathImageWithHiddenInformation =
                @"/Users/surajfehintola/RiderProjects/Steganography/Steganography.ConsoleApplication/img/image_example_with_hidden_information.png";

            // Create an instance of the SteganographyHelper
            var helper = new SteganographyHelper();

            // Retrieve the encrypted data from the image
            var encryptedData = SteganographyHelper.ExtractText(
                new Bitmap(
                    Image.FromFile(pathImageWithHiddenInformation)
                )
            );

            // Decrypt the retrieven data on the image
            var decryptedData = StringCipher.Decrypt(encryptedData, _PASSWORD);

            // Display the secret text in the console or in a messagebox
            // In our case is "Hello, no one should know that my password is 12345"
            Console.WriteLine(decryptedData);
            //MessageBox.Show(decryptedData);
        }

        private static void Input()
        {
            // Declare the password that will allow you to retrieve the encrypted data later
            var _PASSWORD = "password";

            // The String data to conceal on the image
            var _DATA_TO_HIDE = "Hello, no one should know that my password is 12345";

            // Declare the path where the original image is located
            var pathOriginalImage =
                @"/Users/surajfehintola/RiderProjects/Steganography/Steganography.ConsoleApplication/img/screen.png";
            // Declare the new name of the file that will be generated with the hidden information
            var pathResultImage =
                @"/Users/surajfehintola/RiderProjects/Steganography/Steganography.ConsoleApplication/img/image_example_with_hidden_information.png";

            // Create an instance of the SteganographyHelper
            var helper = new SteganographyHelper();

            // Encrypt your data to increase security
            // Remember: only the encrypted data should be stored on the image
            var encryptedData = StringCipher.Encrypt(_DATA_TO_HIDE, _PASSWORD);

            // Create an instance of the original image without indexed pixels
            var originalImage = SteganographyHelper.CreateNonIndexedImage(Image.FromFile(pathOriginalImage));
            // Conceal the encrypted data on the image !
            var imageWithHiddenData = SteganographyHelper.MergeText(encryptedData, originalImage);

            // Save the image with the hidden information somewhere :)
            // In this case the generated file will be image_example_with_hidden_information.png
            imageWithHiddenData.Save(pathResultImage);
        }
    }
}