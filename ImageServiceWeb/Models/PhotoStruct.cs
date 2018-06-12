namespace ImageServiceWeb.Struct
{
    public class PhotoStruct
	{
		// Thumbnail file path
		private string thumbnail;
		public string Thumbnail
		{
			get
			{
				return thumbnail;
			}
			set
			{
				thumbnail = value;
			}
		}
		// Photo file path
		private string photo;
		public string Photo
		{
			get
			{
				return photo;
			}
			set
			{
				photo = value;
			}
		}
		// Photo's name string
		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}
		// Photo's date string
		private string date;
		public string Date
		{
			get
			{
				return date;
			}
			set
			{
				date = value;
			}
		}

		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="path">The string for thumbnail file's path</param>
		public PhotoStruct(string path)
        {
			// get thumbnail path
			thumbnail = path;
			string[] items = path.Split('\\');
			int len = items.Length;
			// get file name
			name = items[len - 1];
			// get photo date
			if (len >= 3)
				date = items[len - 2] + "/" + items[len - 3];
			else
				date = "error";
			// get photo file path
			photo = items[0];
			for (int i = 1; i < len; i++)
				// get every part of the thumbnails path except "Thumbnails" dir
				if (items[i].CompareTo("Thumbnails") != 0)
					photo = photo + @"\" + items[i];
		}
    }
}