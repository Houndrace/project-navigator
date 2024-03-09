using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace project_navigator.helpers;

public static class XamlToPngConverter
{
    public static void ExportToPng(Uri path, FrameworkElement element)
    {
        if (path == null) return;

        // Save current canvas transform
        var transform = element.LayoutTransform;
        // reset current transform (in case it is scaled or rotated)
        element.LayoutTransform = null;

        // Get the size of canvas
        var size = new Size(element.Width, element.Height);
        // Measure and arrange the surface
        // VERY IMPORTANT
        element.Measure(size);
        element.Arrange(new Rect(size));

        // Create a render bitmap and push the surface to it
        var renderBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
        renderBitmap.Render(element);

        // Create a file stream for saving image
        using (var outStream = new FileStream(path.LocalPath, FileMode.Create))
        {
            // Use png encoder for our data
            var encoder = new PngBitmapEncoder();
            // push the rendered bitmap to it
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            // save the data to the stream
            encoder.Save(outStream);
        }

        // Restore previously saved layout
        element.LayoutTransform = transform;
    }
}