using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace File.Service.WebApi
{
    public static class ImageExtensions
    {

        static void Main()
        {
            const string input = "C:\\background1.png";
            const string output = "C:\\thumbnail.png";

            // Load image.
            Image image = Image.FromFile(input);

            // Compute thumbnail size.
            Size thumbnailSize = GetThumbnailSize(image);

            // Get thumbnail.
            Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width,
                thumbnailSize.Height, null, IntPtr.Zero);

            // Save thumbnail.
            thumbnail.Save(output);
        }


        static Size GetThumbnailSize(Image original)
        {
            // Maximum size of any dimension.
            const int maxPixels = 40;

            // Width and height.
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalWidth > originalHeight)
            {
                factor = (double)maxPixels / originalWidth;
            }
            else
            {
                factor = (double)maxPixels / originalHeight;
            }

            // Return thumbnail size.
            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }
    }
}