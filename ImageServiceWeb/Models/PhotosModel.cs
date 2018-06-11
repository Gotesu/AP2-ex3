using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.IO;
using System.Web;
using ImageServiceWeb.Struct;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {
		private FileSystemWatcher m_dirWatcher;     // The Watcher of the Dir
        //data binding property change event
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// notify on property change
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
		
		//colection of handlers
		private ObservableCollection<PhotoStruct> model_photos;
		public ObservableCollection<PhotoStruct> photos
        {
            get
            {
                return model_photos;
            }

            set
            {
				model_photos = value;
				NotifyPropertyChanged("photos");
            }
        }

        //lock objext
        private static object _lock = new object();
        
		/// <summary>
        /// ctor
        /// </summary>
        public PhotosModel()
        {
			try
			{
				m_dirWatcher = new FileSystemWatcher();
				// set dirWatcher path
				m_dirWatcher.Path = HttpContext.Current.Server.MapPath(@"~\OutputDir\Thumbnails");
				// set the photos list (using the path)
				UpdatePhotosList();
				BindingOperations.EnableCollectionSynchronization(model_photos, _lock);
				/* Watch for changes in LastAccess and LastWrite times, and
				   the renaming of files or directories. */
				m_dirWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
				   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
				// update photos list on changed, created or deleted event
				m_dirWatcher.Changed += new FileSystemEventHandler(OnChanged);
				m_dirWatcher.Created += new FileSystemEventHandler(OnChanged);
				m_dirWatcher.Deleted += new FileSystemEventHandler(OnChanged);
				//we will be filtering nothing because we need to watch multiple types, filtering will be done on event.
				//this is supposed to be more efficient than having 4 watchers to each folder.
				m_dirWatcher.Filter = "*.*";
				// Start monitoring
				m_dirWatcher.EnableRaisingEvents = true;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		/// <summary>
		/// on changed, updates the photos list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void OnChanged(object source, FileSystemEventArgs e)
		{
			UpdatePhotosList();
		}

		/// <summary>
		/// Updates the photos list
		/// </summary>
		private void UpdatePhotosList()
		{
			ObservableCollection<PhotoStruct> pho = new ObservableCollection<PhotoStruct>();
			// get all files into an array
			string[] files = Directory.GetFiles(m_dirWatcher.Path, "*",
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
					pho.Add(new PhotoStruct(@"..\" + RelativePath));
				}
			}
			photos = pho;
		}

		/// <summary>
		/// The function delete a file.
		/// </summary>
		/// <param name="photoNum">The number of the photo in model_photos</param>
		public void DeleteFile(int photoNum)
        {
			PhotoStruct photo = model_photos[photoNum];
			// delete the file
			File.Delete(HttpContext.Current.Server.MapPath(photo.Thumbnail));
			File.Delete(HttpContext.Current.Server.MapPath(photo.Photo));
		}
	}
}