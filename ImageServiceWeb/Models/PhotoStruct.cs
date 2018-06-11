namespace ImageServiceWeb.Struct
{
    public class PhotoStruct
	{
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
		public PhotoStruct(string path)
        {
			thumbnail = path;
			string[] items = path.Split('\\');
			int len = items.Length;
			name = items[len - 1];
			if (len >= 3)
				date = items[len - 2] + "/" + items[len - 3];
			else
				date = "error";
			photo = items[0];
			for (int i = 1; i < len; i++)
				if (items[i].CompareTo("Thumbnails") != 0)
					photo = photo + @"\" + items[i];
		}
    }
}