using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using GeoLearn.Droid;
using System.IO;
using GeoLearn.Database;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(MyEntryRenderer))]
[assembly: ExportRenderer(typeof(GeoLearn.Pages.TintableButton), typeof(TintableButtonRenderer))]
[assembly: Dependency(typeof(SQLite_Android))]
namespace GeoLearn.Droid
{
        public class MyEntryRenderer : EntryRenderer
        {
            protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
            {
                base.OnElementChanged(e);

                if (Control == null || e.NewElement == null) return;

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colorset.Bright.ToAndroid());
                else
                    Control.Background.SetColorFilter(Colorset.Bright.ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcAtop);
            }
        }
    
    public class TintableButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            var control = e.NewElement as Pages.TintableButton;
            if (control != null)
            {
                if (control.TintColor != Xamarin.Forms.Color.Default)
                {
                    var androidColor = control.TintColor.ToAndroid();
                    Control.Background.SetColorFilter(androidColor, Android.Graphics.PorterDuff.Mode.Src);
                }
            }
        }
    }

    public class SQLite_Android : Database.ISQLite
    {
        public SQLite_Android() { }
        public string GetDatabasePath(string sqliteFilename)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            return path;
        }
    }


    [Activity(Label = "GeoLearn", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Xamarin.Forms.Forms.SetFlags("RadioButton_Experimental");
            Window.SetStatusBarColor(Colorset.Bright.ToAndroid());

            RequestedOrientation = ScreenOrientation.Portrait;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}