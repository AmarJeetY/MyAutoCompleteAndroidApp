using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using Xamarin.Essentials;
using AlertDialog = Android.App.AlertDialog;
using Environment = Android.OS.Environment;

namespace AutoComplete
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            var adapter =  new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, GetAllWordFromCsvFile());
            var autoComplete = FindViewById<AutoCompleteTextView>(Resource.Id.autoCompletionWord);
            autoComplete.Adapter = adapter;
            var button = FindViewById<Button>(Resource.Id.browseFile);
            button.Click += ButtonOnClick;
        }
        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            Intent = new Intent();
            Intent.SetType("file/*");
            Intent.SetAction(Intent.ActionGetContent);
            Intent.AddCategory(Intent.CategoryOpenable);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select file"),1000);
        }

        public void SaveSelectedFile(string filePath)
        {
            var csvData  = File.ReadAllBytes(filePath);
            var localFilename = "AutoComplete" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv";
            var documentsPath = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).AbsolutePath;
            var isDirectoryWritable = Environment.ExternalStorageState == Environment.MediaMounted;
            if (isDirectoryWritable)
            {
                var newFilepath = Path.Combine(documentsPath, localFilename);
                File.WriteAllBytes(newFilepath, csvData);
            }
            else
            {
                var alert = new AlertDialog.Builder(this);
                alert.SetTitle("File Write Error");
                alert.SetPositiveButton("OK", (senderAlert, args) => {});
                RunOnUiThread(() => {
                    alert.Show();
                });
            }
        }
        private List<string> GetAllWordFromCsvFile()
        {
            var rgx = new Regex("^[a-zA-Z]+$");
            var assets = Assets;
            var csvDestinationFilePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(csvDestinationFilePath, "AutoComplete.csv");
            using (var stream1 = new FileStream(filePath, FileMode.Create))
            {
                stream1.Dispose();
            }

            using (var sr = new StreamReader(assets.Open("AutoComplete.csv")))
            {
                var content = sr.ReadToEnd();
                File.WriteAllText(filePath, content);
            }

            var allWords = File.ReadAllLines(filePath)
                .SelectMany(line => line.Split(','))
                .SelectMany(line => line.Split(' '))
                .ToList();
            return allWords.Where(word => rgx.IsMatch(word)).ToList();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}