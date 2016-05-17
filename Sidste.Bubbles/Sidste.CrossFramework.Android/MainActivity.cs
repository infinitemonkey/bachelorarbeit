using Android.App;
using Android.Content.PM;
using Android.OS;
using CocosSharp;
using Sidste.CrossFramework.Common;

namespace Sidste.CrossFramework.Android
{
    [Activity(Label = "Sidste.CrossFramework.Android", MainLauncher = true, Icon = "@drawable/icon", 
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our game view from the layout resource,
            // and attach the view created event to it
            CCGameView gameView = (CCGameView)FindViewById(Resource.Id.GameView);
            gameView.ViewCreated += GameInitialization.LoadGame;
        }
    }
}


