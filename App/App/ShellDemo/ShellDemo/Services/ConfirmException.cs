using System;
using System.Collections.Generic;
using System.Text;

namespace ShellDemo.Services 
{
    public class ConfirmException : Exception
    {
       public ConfirmException(string message):base(message)
        {

        }
    }
}
