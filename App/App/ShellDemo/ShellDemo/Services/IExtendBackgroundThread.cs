using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShellDemo.Services
{
    public interface IExtendBackgroundThread
    {
        Task ExtendBackGroundThreadTime(Func<Task> action, string name = "MyBackgroundTaskName");
    
    }
}
