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
using System.ComponentModel;
using System.Text;

namespace Sinonims.ViewModels
{
    public class SynsetViewModel : INotifyPropertyChanged
    {

        private Synset _synset;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public Synset Synset
        {
            get
            {
                return _synset;
            }
            set
            {
                if (value != _synset)
                {
                    _synset = value;
                    NotifyPropertyChanged("Synset");
                    NotifyPropertyChanged("Category");
                }
            }
        }

        public string Category {
            get {
                StringBuilder sb = new StringBuilder();    
                foreach (String st in Synset.Categories){
                   sb.Append(st);
                   sb.Append(" ");
                    
                }
                return sb.ToString();

            }
        
        }

        public string DetallsUri {

            get {

                return "http://openthesaurus.softcatala.org/synonyme/edit/"+Synset.id;
            }
        
        }
        
        public override string ToString()
        {
            return _synset.ToString();
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