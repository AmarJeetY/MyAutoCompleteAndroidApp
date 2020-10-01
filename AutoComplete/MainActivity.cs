using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;

namespace AutoComplete
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
       
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);
            
            ArrayAdapter adapter =  new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, GetAllWords());
            var autoComplete1 = FindViewById<AutoCompleteTextView>(Resource.Id.autoCompletionWord);
            
            autoComplete1.Adapter = adapter;
        }

        private List<string> GetAllWords()
        {
            var rgx = new Regex("^[a-zA-Z]+$");
            var appDirectory = Application.Context.GetExternalFilesDir(null).AbsolutePath;
            var textFilePath = Path.Combine(appDirectory, "DataFile.csv");
            var file = File.CreateText(textFilePath);
            file.Write("hello, helium, height, high, horse, flower, florence");
            file.Close();
            var text123 = File.ReadAllLines(textFilePath);
            var results = File.ReadAllLines(textFilePath)
                .SelectMany(line => line.Split(','))
                .SelectMany(line => line.Split(' '))

                //.GroupBy(word => word)
                .ToList();
                //.RemoveAll(m => criteria.Any(crit => m.rgx.Contains(crit)););


            var list = new List<string>();
            foreach (var x in results)
            {
                if (rgx.IsMatch(x.ToString())) list.Add(x.ToString());
            }

            return list;

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}