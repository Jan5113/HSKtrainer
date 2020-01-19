using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace HSKtrain2.Droid
{
    [Activity(Label = "HSKtrain2", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

			string vocPath = FileAccessHelper.GetLocalFilePath("HSK_Voc.csv");

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(vocPath));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

	public class FileAccessHelper {
		public static string GetLocalFilePath(string filename) {
			string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			string filePath = System.IO.Path.Combine(path, filename);

			CopyDatabaseIfNotExists(filePath, filename);

			return filePath;
		}

		private static void CopyDatabaseIfNotExists(string filePath, string filename) {
			if (!System.IO.File.Exists(filePath)) {
				using var br = new System.IO.BinaryReader(Application.Context.Assets.Open(filename));
				using var bw = new System.IO.BinaryWriter(new System.IO.FileStream(filePath, System.IO.FileMode.Create));
				byte[] buffer = new byte[2048];
				int length = 0;
				while ((length = br.Read(buffer, 0, buffer.Length)) > 0) {
					bw.Write(buffer, 0, length);
				}
			}
		}
	}

}