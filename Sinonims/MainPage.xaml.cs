/*
 * Copyright (C) 2014 Bernat Mut <berni.emerald@gmail.com>
 * 
 * This file is part of Traductor Softcatala.
 * Traductor Softcatala is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this program; if not, write to the
 * Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Sinonims.Resources;
using Microsoft.Phone.Tasks;
using GoogleAds;
using System.Diagnostics;
using System.Windows.Media;
using Microsoft.Phone.Net.NetworkInformation;
using Windows.Phone.Speech.Recognition;
using Sinonims.ViewModels;

namespace Sinonims
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }


        private void GetResponse(object sender, RoutedEventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show(AppResources.InternetNecesario);

                return;
            }


            JSonHelper serverRequest = new JSonHelper();
            serverRequest.ResponseEvent += ResponseEvent;
            serverRequest.sendJson(textToTranslateEdit.Text);




        }

        private void ResponseEvent(object sender, JSonHelper.ResponseEventArgs e)
        {

            //textTranslated.Text = e.Message;
            App.ViewModel.UpdateModel(e.Response);
        }

       


        #region Funcions comuns email, donate, publicitat,...

        private void EmailLink_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = "About " + AppResources.ApplicationTitle;
            emailComposeTask.To = emailLink.Content as string;
            emailComposeTask.Show();

        }

        private void donateClick(object sender, RoutedEventArgs e)
        {
            string url = "";

            string business = "apps@bitsdelocos.es";  // your paypal email
            string description = Uri.EscapeDataString(AppResources.Donate + " " + AppResources.ApplicationTitle);
            string country = "ES";                  // AU, US, etc.
            string currency = "EUR";                 // AUD, USD, etc.

            url += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_donations" +
                "&business=" + business +
                "&lc=" + country +
                "&item_name=" + description +
                "&currency_code=" + currency +
                "&bn=" + "PP%2dDonationsBF";

            WebBrowserTask wb = new WebBrowserTask();
            wb.Uri = new Uri(url);
            wb.Show();
        }


        private void AdControl_AdRefreshed(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    //ad.Visibility = Visibility.Collapsed;
                    foreach (AdView tb in FindVisualChildren<AdView>(this.LayoutRoot))
                    {
                        // do something with tb here
                        tb.Visibility = Visibility.Collapsed;

                    }
                }
                catch { }
            });
        }



        private void AdControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            Debug.WriteLine(e.Error.ToString());

            Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    //ad.Visibility = Visibility.Collapsed;
                    ((MainViewModel)(this.DataContext)).MsVisibilityResult = false;
                    foreach (AdView tb in FindVisualChildren<AdView>(this.LayoutRoot))
                    {
                        // do something with tb here
                        tb.Visibility = Visibility.Visible;

                    }
                }
                catch { }
            });

        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void AdView_ReceivedAd(object sender, GoogleAds.AdEventArgs e)
        {

        }

        private void AdView_FailedToReceiveAd(object sender, GoogleAds.AdErrorEventArgs e)
        {
            Debug.WriteLine(e.ErrorCode.ToString());

        }
        #endregion



    }
}