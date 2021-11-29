using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using ShellDemo.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ShellDemo.iOS.ExtendBackgroundThread))]
namespace ShellDemo.iOS
{
    class ExtendBackgroundThread : IExtendBackgroundThread
    {

        public async Task ExtendBackGroundThreadTime(Func<Task> action,
                            string name = "MyBackgroundTaskName")
        {
            nint taskId = 0;
            taskId = UIApplication.SharedApplication.BeginBackgroundTask(name, () => {
                // this code runs if background work does not complete within permitted time
                // request background work to stop (set a flag checked by background task)
                UIApplication.SharedApplication.EndBackgroundTask(taskId); // report background work done
            });
            try
            {
                await action(); // perform background work
            }
            finally
            {
                UIApplication.SharedApplication.EndBackgroundTask(taskId);
            }
        }

    }
}