using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Oni.Imaging
{
	internal class SysWriter
	{
		public static void Write(Surface surface, string filePath)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			surface = surface.Convert(SurfaceFormat.BGRA);
			using (Bitmap bitmap = new Bitmap(surface.Width, surface.Height, PixelFormat.Format32bppArgb))
			{
				Rectangle rect = new Rectangle(0, 0, surface.Width, surface.Height);
				BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
				Marshal.Copy(surface.Data, 0, bitmapData.Scan0, surface.Data.Length);
				bitmap.UnlockBits(bitmapData);
				switch (Path.GetExtension(filePath))
				{
				case ".png":
					bitmap.Save(filePath, ImageFormat.Png);
					break;
				case ".jpg":
					bitmap.Save(filePath, ImageFormat.Jpeg);
					break;
				case ".bmp":
					bitmap.Save(filePath, ImageFormat.Bmp);
					break;
				case ".tif":
					bitmap.Save(filePath, ImageFormat.Tiff);
					break;
				}
			}
		}
	}
}
