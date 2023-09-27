// ***********************************************************************
// Assembly         : OpenAC.Net.Core
// Author           : RFTD
// Created          : 04-19-2014
//
// Last Modified By : RFTD
// Last Modified On : 08-30-2015
// ***********************************************************************
// <copyright file="AssemblyExtenssions.cs" company="OpenAC .Net">
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
using System.IO;
using System.Drawing;

namespace OpenAC.Net.Core.Extensions
{
    /// <summary>
    /// Class ByteExtensions.
    /// </summary>
    public static partial class ByteExtensions
    {
        /// <summary>
        /// To the base64.
        /// </summary>
        /// <param name="byteArrayIn">The byte array in.</param>
        /// <returns>System.String.</returns>
        public static string ToBase64(this byte[] byteArrayIn)
        {
            if (byteArrayIn == null || byteArrayIn.Length < 1) return string.Empty;
            return Convert.ToBase64String(byteArrayIn);
        }

        /// <summary>
        /// To the image.
        /// </summary>
        /// <param name="byteArrayIn">The byte array in.</param>
        /// <returns>Image.</returns>
        public static Image ToImage(this byte[] byteArrayIn)
        {
            if (byteArrayIn == null) return null;

            using var ms = new MemoryStream(byteArrayIn);
            var returnImage = Image.FromStream(ms);
            return returnImage;
        }

        /// <summary>
        /// Checa se o byte na posição informada é 1.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static bool IsBitOn(this byte value, int idx) => ((value >> idx) & 1) == 1;

        /// <summary>
        /// Checa se o byte na posição informada é 0.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static bool IsBitOff(this byte value, int idx) => !IsBitOn(value, idx);
    }
}