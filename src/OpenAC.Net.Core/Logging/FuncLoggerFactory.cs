// ***********************************************************************
// Assembly         : OpenAC.Net.Core
// Author           : RFTD
// Created          : 06-06-2016
//
// Last Modified By : RFTD
// Last Modified On : 06-06-2016
// ***********************************************************************
// <copyright file="FuncLogManager.cs" company="OpenAC .Net">
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
    ///
    /// </summary>
    public class FuncLoggerFactory : ILoggerFactory
    {
        #region Fields

        private readonly Func<Type, IOpenLogger> loggerByType;
        private readonly Func<string, IOpenLogger> loggerByKey;

        #endregion Fields

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="getLoggerType"></param>
        /// <param name="getLoggerKey"></param>
        public FuncLoggerFactory(Func<Type, IOpenLogger> getLoggerType, Func<string, IOpenLogger> getLoggerKey = null)
        {
            loggerByType = getLoggerType;
            loggerByKey = getLoggerKey;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="getLogger"></param>
        public FuncLoggerFactory(Func<string, IOpenLogger> getLogger) : this(null, getLogger)
        {
        }

        #endregion Constructors

        #region Methods

        public IOpenLogger LoggerFor(string keyName)
        {
            return loggerByKey?.Invoke(keyName);
        }

        public IOpenLogger LoggerFor(Type type)
        {
            return loggerByType?.Invoke(type);
        }

        #endregion Methods
    }
}