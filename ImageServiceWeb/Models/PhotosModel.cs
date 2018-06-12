using System;
using System.IO;
using System.Web;
using ImageServiceWeb.Struct;
using System.Collections.Generic;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {
		private string path = HttpContext.Current.Server.MapPath(@"~\OutputDir\Thumbnails");
		private Object thisLock = new Object();
		private List<PhotoStruct> photos = null;
		public List<PhotoStruct> Photos
		{
			get
			{
				if (photos == null)
					GetPhotoList();
				return photos;
			}
		}

		public List<PhotoStruct> GetPhotoList()
		{
			lock (thisLock)
			{
				photos = new List<PhotoStruct>();
				// get all files into an array
				string[] files = Directory.GetFiles(path, "*",
					SearchOption.AllDirectories);
				foreach (string file in files)
				{
					//check file type
					if (IsImage(file))
					{
						String RelativePath = file.Replace(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty);
						photos.Add(new PhotoStruct(@"..\" + RelativePath));
					}
				}
			}
			return photos;
		}

		/// <summary>
		/// The function delete a file.
		/// </summary>
		/// <param name="photoNum">The number of the photo in model_photos</param>
		public void DeleteFile(int photoNum)
        {
			lock (thisLock)
			{
				if (photos == null)
					return;
				PhotoStruct photo = photos[photoNum];
				// delete the file
				File.Delete(HttpContext.Current.Server.MapPath(photo.Thumbnail));
				File.Delete(HttpContext.Current.Server.MapPath(photo.Photo));
			}
		}

		/// <summary>
		/// The function checks if the given file is an image.
		/// </summary>
		/// <param name="path">The string for file's path</param>
		/// <returns>True if file is image type, and false otherwise</returns>
		public bool IsImage(string path)
		{
			return (path.EndsWith(".jpg") || path.EndsWith(".png") ||
					 path.EndsWith(".gif") || path.EndsWith(".bmp") ||
					 path.EndsWith(".JPG") || path.EndsWith(".PNG") ||
					 path.EndsWith(".GIF") || path.EndsWith(".BMP"));
		}

		/// <summary>
		/// The function counts the number of image files in a given directory.
		/// </summary>
		/// <returns>The number of image files in the directory</returns>
		public int ImagesNumber()
		{
			// get all files into an array
			string[] files = Directory.GetFiles(path, "*",
				SearchOption.AllDirectories);
			// count image files
			int count = 0;
			foreach (string file in files)
			{
				//check file type
				if (IsImage(file))
					count++; // increase count value
			}
			return count;
		}
	}
}