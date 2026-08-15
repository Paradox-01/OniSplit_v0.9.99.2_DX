using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Oni.Imaging
{
	internal static class SysReader
	{
		public static Surface Read(string filePath)
		{
			using (Bitmap bitmap = new Bitmap(filePath, false))
			{
				SurfaceFormat format;
				PixelFormat format2;
				if (bitmap.RawFormat == ImageFormat.Jpeg || bitmap.RawFormat == ImageFormat.Bmp)
				{
					format = SurfaceFormat.BGRX;
					format2 = PixelFormat.Format32bppRgb;
				}
				else
				{
					format = SurfaceFormat.BGRA;
					format2 = PixelFormat.Format32bppArgb;
				}
				Surface surface = new Surface(bitmap.Width, bitmap.Height, format);
				Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
				BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, format2);
				Marshal.Copy(bitmapData.Scan0, surface.Data, 0, surface.Data.Length);
				bitmap.UnlockBits(bitmapData);
				return surface;
			}
		}
	}
}
