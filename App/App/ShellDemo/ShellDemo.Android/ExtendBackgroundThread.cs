using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShellDemo.Services;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(ShellDemo.Droid.ExtendBackgroundThread))]
namespace ShellDemo.Droid
{
    class ExtendBackgroundThread: IExtendBackgroundThread
    {


        public async Task ExtendBackGroundThreadTime(Func<Task> action, string name = "MyBackgroundTaskName")
        {
            var powerManager = (PowerManager)Application.Context.GetSystemService(Context.PowerService);
            var wakeLock = powerManager.NewWakeLock(WakeLockFlags.Partial, name);
            wakeLock.Acquire(); // Prevent device from going to sleep.
            try
            {

                await action(); // perform background work
            }
            finally
            {
                wakeLock.Release();  // ensure wakeLock is released even if exception occurs
            }
        }


    }
}