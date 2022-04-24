// ***********************************************************************
// Assembly         : ACBr.Net.ECF
// Author           : RFTD
// Created          : 04-05-2017
//
// Last Modified By : RFTD
// Last Modified On : 04-05-2017
// ***********************************************************************
// <copyright file="OpenIniSection.cs" company="OpenAC .Net">
//		        		   The MIT License (MIT)
//	     		    Copyright (c) 2014 - 2022 Projeto OpenAC .Net
//
//	 Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//	 The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//	 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using OpenAC.Net.Core.Extensions;

namespace OpenAC.Net.Core
{
    public sealed class OpenIniSection : Dictionary<string, string>
    {
        #region Constructors

        public OpenIniSection(string name) : this(null, name)
        {
        }

        public OpenIniSection(OpenIniFile parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        #endregion Constructors

        #region Properties

        public OpenIniFile Parent { get; internal set; }

        public string Name { get; set; }

        #endregion Properties

        #region Methods

        public TType GetValue<TType>(string key, TType defaultValue = default, IFormatProvider format = null)
        {
            if (key.IsEmpty()) return defaultValue;

            TType ret;
            try
            {
                if (format == null) format = CultureInfo.InvariantCulture;
                if (!ContainsKey(key)) return defaultValue;

                ret = (TType)Convert.ChangeType(this[key], typeof(TType), format);
            }
            catch (Exception)
            {
                ret = defaultValue;
            }

            return ret;
        }

        #endregion Methods
    }
}