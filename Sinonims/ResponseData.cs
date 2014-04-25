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
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sinonims
{
    public class Matches
    {

        public MetaData metaData;
        public Synset[] synsets;
    }
    public class MetaData
    {
        public string apiVersion;
        public string warning;
        public string copyright;
        public string license;
        public string source;
        public string date;
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (MemberInfo mi in this.GetType().GetMembers())
            {
                if (mi.MemberType == MemberTypes.Field)
                {
                    FieldInfo pi = mi as FieldInfo;
                    if (pi != null)
                    {
                        try
                        {
                            sb.AppendLine(pi.Name + ": " + pi.GetValue(this).ToString());
                        }
                        catch {
                            sb.AppendLine(pi.Name + ": null");

                        }
                    }
                }
                else if (mi.MemberType == MemberTypes.Property)
                {
                    PropertyInfo pi = mi as PropertyInfo;
                    if (pi != null)
                    {
                        try
                        {
                            sb.AppendLine(pi.Name + ": " + pi.GetValue(this).ToString());
                        }
                        catch
                        {
                            sb.AppendLine(pi.Name + ": null");

                        }
                    }
                }
            }

            return sb.ToString();
        }


    }

    public class Synset
    {
        public int id;
        public String[] Categories;
        public Term[] terms;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Term t in terms) {
                sb.AppendLine("- "+ WebUtility.HtmlDecode(t.term));
            }
            return sb.ToString(0, sb.Length - 2);


        }
    }
    public class Term
    {
        public string term;
    }
}
