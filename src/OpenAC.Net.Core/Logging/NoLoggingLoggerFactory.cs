// ***********************************************************************
// Assembly         : OpenAC.Net.Core
// Author           : RFTD
// Created          : 03-21-2014
//
// Last Modified By : RFTD
// Last Modified On : 01-30-2015
// ***********************************************************************
// <copyright file="NoLoggingLoggerFactory.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.Core.Logging
{
    /// <summary>
    /// Classe NoLoggingLoggerFactory.
    /// </summary>
	public class NoLoggingLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// The nologging
        /// </summary>
		private static readonly IOpenLogger Nologging = new NoLoggingLogger();

        /// <summary>
        /// Loggers for.
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <returns>IOpenLogger.</returns>
		public IOpenLogger LoggerFor(string keyName)
        {
            return Nologging;
        }

        /// <summary>
        /// Loggers for.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>IOpenLogger.</returns>
		public IOpenLogger LoggerFor(Type type)
        {
            return Nologging;
        }
    }
}