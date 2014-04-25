/*
 * Copyright (C) 2014 Bernat Mut <berni.emerald@gmail.com>
 * 
 * This file is part of Openthesaurus-ca.
 * Openthesaurus-ca is free software; you can redistribute it and/or
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sinonims
{
    public class JSonHelper
    {
        private static String SERVER_URL = "http://openthesaurus.softcatala.org/synonyme/search";
        private static String format = "application/json";
        public delegate void RecievedResponseHandler(object sender, ResponseEventArgs e);

        public event RecievedResponseHandler ResponseEvent;

        protected void OnRecievedResponse(ResponseEventArgs e)
        {
            ResponseEvent(this, e);
        }


        // Adds a query parameter to an URL 	
        String AddQueryParameter(String key, String value)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("&");
            sb.Append(key);
            sb.Append("=");
            try
            {
                sb.Append(Uri.EscapeDataString(value));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return sb.ToString();
        }

        private Uri BuildURL(String text)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SERVER_URL);
            sb.Append("?");
            sb.Append(AddQueryParameter("q", text));
            sb.Append(AddQueryParameter("format",format));
            return new Uri(sb.ToString());
        }



        public void sendJson(String text)
        {


            WebClient client = new WebClient();

            Uri url = BuildURL(text);
            client.DownloadStringCompleted += StringDownloadCompleted;
            client.DownloadStringAsync(url);






        }

        private void StringDownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(e.Result))
            {
                string json = e.Result;

                // JArray x = new JArray(e.Result);
                //Translated = x["responseData"]["translatedText"].ToString();
                try
                {
                    Matches res = JsonConvert.DeserializeObject<Matches>(json);
                    ResponseEventArgs args = new ResponseEventArgs(res);
                    OnRecievedResponse(args);
                }
                catch(Exception ex) {
                    Debug.WriteLine(ex);
                
                }

            }


            ((WebClient)sender).DownloadStringCompleted -= StringDownloadCompleted;



        }



        public class ResponseEventArgs : EventArgs
        {
            private Matches _response;
            public ResponseEventArgs(Matches Message)
            {
                this._response = Message;
            }
            public Matches Response
            {
                get { return this._response; }
                set { this._response = value; }
            }


        }

    }

   


}
