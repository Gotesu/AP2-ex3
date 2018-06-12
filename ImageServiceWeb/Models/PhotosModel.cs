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
			photos = new List<PhotoStruct>();
			// get all files into an array
			string[] files = Directory.GetFiles(path, "*",
				SearchOption.AllDirectories);
			foreach (string file in files)
			{
				//check file type
				if (file.EndsWith(".jpg") || file.EndsWith(".png") ||
					file.EndsWith(".gif") || file.EndsWith(".bmp") ||
					file.EndsWith(".JPG") || file.EndsWith(".PNG") ||
					file.EndsWith(".GIF") || file.EndsWith(".BMP"))
				{
					String RelativePath = file.Replace(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty);
					photos.Add(new PhotoStruct(@"..\" + RelativePath));
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
			if (photos == null)
				return;
			PhotoStruct photo = photos[photoNum];
			// delete the file
			File.Delete(HttpContext.Current.Server.MapPath(photo.Thumbnail));
			File.Delete(HttpContext.Current.Server.MapPath(photo.Photo));
		}
	}
}