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
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using Sinonims.Resources;

namespace Sinonims.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.SynsetList = new ObservableCollection<SynsetViewModel>();
            this.MetaData = new MetaData();
        }



        MetaData _metaData;
        public MetaData MetaData
        {
            get
            {
                return _metaData;
            }
            set
            {
                if (value != _metaData)
                {
                    _metaData = value;
                    NotifyPropertyChanged("MetaData");
                }
            }
        }




        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<SynsetViewModel> SynsetList { get; private set; }

        public string AppVersion
        {
            get
            {
                var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
                var version = nameHelper.Version;
                return version.ToString();

            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }
          /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        /// 
        public void UpdateModel(Matches match)
        {
            SynsetList.Clear();
            foreach(Synset s in match.synsets){

                SynsetList.Add(new SynsetViewModel() { Synset=s});

            }
            MetaData = match.metaData;
        
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        /// 
        public void LoadData()
        {

            // Setting Ads 
            MsVisibilityResult = true;
#if DEBUG
            MsAdUnitId = "Image480_80";
            MsApplicationId = "test_client";
#else
            MsAdUnitId = "10735038";
            MsApplicationId = "47bad1ab-66a7-4865-b4ae-bad8724de530";

#endif

            MetadataVisible = false;
            this.IsDataLoaded = true;


        }


        Boolean _metadataVisible;
        public Boolean MetadataVisible
        {
            get
            {
                return _metadataVisible;
            }
            set
            {
                if (value != _metadataVisible)
                {
                    _metadataVisible = value;
                    NotifyPropertyChanged("MetadataVisible");
                }
            }
        }
        
        string _msAdUnitId;
        public string MsAdUnitId
        {
            get
            {
                return _msAdUnitId;
            }
            set
            {
                if (value != _msAdUnitId)
                {
                    _msAdUnitId = value;
                    NotifyPropertyChanged("MsAdUnitId");
                }
            }
        }


        string _msApplicationId;
        public string MsApplicationId
        {
            get
            {
                return _msApplicationId;
            }
            set
            {
                if (value != _msApplicationId)
                {
                    _msApplicationId = value;
                    NotifyPropertyChanged("MsApplicationId");
                }
            }
        }



        Boolean _msVisibilityResult;
        public Boolean MsVisibilityResult
        {
            get
            {
                return _msVisibilityResult;
            }
            set
            {
                if (value != _msVisibilityResult)
                {
                    _msVisibilityResult = value;
                    NotifyPropertyChanged("MsVisibilityResult");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}