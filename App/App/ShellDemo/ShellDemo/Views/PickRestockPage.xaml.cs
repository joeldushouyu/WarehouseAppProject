using ShellDemo.Models;
using ShellDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShellDemo.Views
{
    public partial class PickRestockPage : ContentPage
    {
        public Item Item { get; set; }

        public PickRestockPage(Order ord, OrderAction act)
        {
            InitializeComponent();
            BindingContext = new PickRestockViewModel(ord, act);
        }
    }
}