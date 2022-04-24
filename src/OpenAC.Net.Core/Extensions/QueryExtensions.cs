// ***********************************************************************
// Assembly         : OpenAC.Net.Core
// Author           : RFTD
// Created          : 02-28-2015
//
// Last Modified By : RFTD
// Last Modified On : 23-04-2022
// ***********************************************************************
// <copyright file="QueryExtensions.cs" company="OpenAC .Net">
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
using System.Linq;
using System.Linq.Expressions;

namespace OpenAC.Net.Core.Extensions
{
    public static class QueryExtensions
    {
        /// <summary>
        /// Wheres if.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="ifFunc">If function.</param>
        /// <param name="ifExpression">If expression.</param>
        /// <param name="elseExpression">The else expression.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query,
            Func<bool> ifFunc,
            Expression<Func<T, bool>> ifExpression,
            Expression<Func<T, bool>> elseExpression = null)
        {
            return query.WhereIf(ifFunc(), ifExpression, elseExpression);
        }

        /// <summary>
        /// Wheres if.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="ifFunc">if set to <c>true</c> [if function].</param>
        /// <param name="ifExpression">If expression.</param>
        /// <param name="elseExpression">The else expression.</param>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query,
            bool ifFunc,
            Expression<Func<T, bool>> ifExpression,
            Expression<Func<T, bool>> elseExpression = null)
        {
            if (ifFunc)
                return query.Where(ifExpression);

            return elseExpression != null ? query.Where(elseExpression) : query;
        }

        /// <summary>
        /// Wheres if.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="ifFunc">If function.</param>
        /// <param name="ifExpression">If expression.</param>
        /// <param name="elseExpression">The else expression.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query,
            Func<bool> ifFunc,
            Func<T, bool> ifExpression,
            Func<T, bool> elseExpression = null)
        {
            return query.WhereIf(ifFunc(), ifExpression, elseExpression);
        }

        /// <summary>
        /// Wheres if.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">The query.</param>
        /// <param name="ifFunc">if set to <c>true</c> [if function].</param>
        /// <param name="ifExpression">If expression.</param>
        /// <param name="elseExpression">The else expression.</param>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query,
            bool ifFunc,
            Func<T, bool> ifExpression,
            Func<T, bool> elseExpression = null)
        {
            if (ifFunc)
                return query.Where(ifExpression);

            return elseExpression != null ? query.Where(elseExpression) : query;
        }
    }
}