﻿// ***********************************************************************
// Assembly         : OpenAC.Net.Core
// Author           : RFTD
// Created          : 02-18-2018
//
// Last Modified By : RFTD
// Last Modified On : 02-18-2018
// ***********************************************************************
// <copyright file="OpenSafeHandle.cs" company="OpenAC .Net">
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
using System.Runtime.ConstrainedExecution;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using OpenAC.Net.Core.Logging;

namespace OpenAC.Net.Core.InteropServices
{
    /// <inheritdoc cref="SafeHandle" />
    public abstract class OpenSafeHandle : SafeHandle, IOpenLog
    {
        #region InnerTypes

        private class LibLoader
        {
            #region InnerTypes

            private static class Windows
            {
                [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
                public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

                [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
                public static extern IntPtr LoadLibraryW(string lpszLib);

                [DllImport("kernel32", SetLastError = true)]
                public static extern bool FreeLibrary(IntPtr hModule);
            }

            private static class Linux
            {
                [DllImport("libdl.so.2")]
                public static extern IntPtr dlopen(string path, int flags);

                [DllImport("libdl.so.2")]
                public static extern IntPtr dlsym(IntPtr handle, string symbol);

                [DllImport("libdl.so.2")]
                public static extern int dlclose(IntPtr handle);
            }

            private static class OSX
            {
                [DllImport("/usr/lib/libSystem.dylib")]
                public static extern IntPtr dlopen(string path, int flags);

                [DllImport("/usr/lib/libSystem.dylib")]
                public static extern IntPtr dlsym(IntPtr handle, string symbol);

                [DllImport("/usr/lib/libSystem.dylib")]
                public static extern int dlclose(IntPtr handle);
            }

            #endregion InnerTypes

            #region Fields

            private static readonly IOpenLogger Logger;

            #endregion Fields

            #region Exports

            [DllImport("libc")]
            private static extern int uname(IntPtr buf);

            #endregion Exports

            #region Constructors

            static LibLoader()
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32S:
                    case PlatformID.Win32Windows:
                    case PlatformID.Win32NT:
                    case PlatformID.WinCE:
                        IsWindows = true;
                        break;

                    case PlatformID.Unix:
                        try
                        {
                            var num = Marshal.AllocHGlobal(8192);
                            if (uname(num) == 0 && Marshal.PtrToStringAnsi(num) == "Darwin")
                                IsOSX = true;

                            Marshal.FreeHGlobal(num);
                            break;
                        }
                        catch
                        {
                            break;
                        }

                    case PlatformID.MacOSX:
                        IsOSX = true;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Logger = LoggerProvider.LoggerFor(typeof(OpenSafeHandle));
            }

            #endregion Constructors

            #region Properties

            public static readonly bool IsWindows;

            public static readonly bool IsOSX;

            #endregion Properties

            #region Methods

            public static IntPtr LoadLibrary(string libname)
            {
                if (IsWindows) return Windows.LoadLibraryW(libname);
                return IsOSX ? OSX.dlopen(libname, 1) : Linux.dlopen(libname, 1);
            }

            public static bool FreeLibrary(IntPtr library)
            {
                if (IsWindows) return Windows.FreeLibrary(library);
                return (IsOSX ? OSX.dlclose(library) : Linux.dlclose(library)) == 0;
            }

            public static IntPtr GetProcAddress(IntPtr library, string function)
            {
                var num = !IsWindows ? (!IsOSX ? Linux.dlsym(library, function) : OSX.dlsym(library, function)) :
                                       Windows.GetProcAddress(library, function);

                if (num == IntPtr.Zero || num == MinusOne) Logger.Warn("Função não encontrada: " + function);
                return num;
            }

            public static T LoadFunction<T>(IntPtr procaddress) where T : Delegate
            {
                if (procaddress == IntPtr.Zero || procaddress == MinusOne) return null;
                var functionPointer = Marshal.GetDelegateForFunctionPointer(procaddress, typeof(T));

                return functionPointer as T;
            }

            #endregion Methods
        }

        #endregion InnerTypes

        #region Fields

        protected readonly Dictionary<Type, string> methodList;
        protected readonly Dictionary<string, Delegate> methodCache;
        protected readonly string className;

        #endregion Fields

        #region Constructors

        static OpenSafeHandle() => MinusOne = new IntPtr(-1);

        /// <inheritdoc />
        protected OpenSafeHandle(string dllPath) : base(IsWindows ? IntPtr.Zero : MinusOne, true)
        {
            methodList = new Dictionary<Type, string>();
            methodCache = new Dictionary<string, Delegate>();
            className = GetType().Name;

            var pNewSession = LibLoader.LoadLibrary(dllPath);
            SetHandle(pNewSession);
            Guard.Against<OpenException>(IsInvalid, "Não foi possivel carregar a biblioteca.");
        }

        #endregion Constructors

        #region Properties

        private static IntPtr MinusOne { get; }

        /// <summary>
        /// Retornar o valor de um handler invalido.
        /// </summary>
        protected IntPtr InvalidHandler => IsWindows ? IntPtr.Zero : MinusOne;

        /// <inheritdoc />
        public override sealed bool IsInvalid => InvalidHandler == handle;

        public static bool IsWindows => LibLoader.IsWindows;

        public static bool IsOSX => LibLoader.IsOSX;

        public static bool IsLinux => !LibLoader.IsOSX && LibLoader.IsWindows;

        #endregion Properties

        #region Methods

        /// <inheritdoc />
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()

        {
            if (IsInvalid) return true;

            var ret = LibLoader.FreeLibrary(handle);

            if (ret)
                SetHandleAsInvalid();

            return ret;
        }

        /// <summary>
        /// Adiciona um delegate a lista para a função informada.
        /// </summary>
        /// <param name="functionName">Nome da função para exportar</param>
        /// <typeparam name="T">Delegate da função</typeparam>
        protected virtual void AddMethod<T>(string functionName) where T : Delegate
        {
            methodList.Add(typeof(T), functionName);
        }

        /// <summary>
        /// Retorna o delegate para uso.
        ///  </summary>
        /// <typeparam name="T">Delegate</typeparam>
        /// <returns></returns>
        /// <exception cref="OpenException"></exception>
        protected virtual T GetMethod<T>() where T : Delegate
        {
            if (!methodList.ContainsKey(typeof(T))) throw CreateException($"Função não adicionada para o [{nameof(T)}].");

            var method = methodList[typeof(T)];
            this.Log().Debug($"{className} : Acessando o método [{method}] da biblioteca.");
            if (methodCache.ContainsKey(method)) return methodCache[method] as T;

            var mHandler = LibLoader.GetProcAddress(handle, method);

            Guard.Against<ArgumentNullException>(mHandler == IntPtr.Zero || mHandler == MinusOne, "Função não encontrada: " + method);

            var methodHandler = LibLoader.LoadFunction<T>(mHandler);
            this.Log().Debug($"{className} : Método [{method}] carregado.");

            methodCache.Add(method, methodHandler);
            return methodHandler;
        }

        /// <summary>
        /// Executa a função e trata erros nativos.
        /// </summary>
        /// <param name="method"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="OpenException"></exception>
        [HandleProcessCorruptedStateExceptions]
        protected virtual T ExecuteMethod<T>(Func<T> method)
        {
            try
            {
                return method();
            }
            catch (Exception exception)
            {
                throw ProcessException(exception);
            }
        }

        /// <summary>
        /// Executa a função e trata erros nativos.
        /// </summary>
        /// <param name="method"></param>
        /// <exception cref="OpenException"></exception>
        [HandleProcessCorruptedStateExceptions]
        protected virtual void ExecuteMethod(Action method)
        {
            try
            {
                method();
            }
            catch (Exception exception)
            {
                throw ProcessException(exception);
            }
        }

        /// <summary>
        /// Cria e dispara uma <see cref="OpenException"/> com a mensagem informada.
        /// </summary>
        /// <param name="errorMessage">Mensagem de erro.</param>
        /// <returns><see cref="OpenException"/></returns>
        protected virtual OpenException CreateException(string errorMessage)
        {
            this.Log().Error($"{className} - Erro: {errorMessage}");
            return new OpenException(errorMessage);
        }

        /// <summary>
        /// Tatar uma <see cref="Exception"/> e dispara uma <see cref="OpenException"/> com a mensagem da mesma.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns><see cref="OpenException"/></returns>
        protected virtual OpenException ProcessException(Exception exception)
        {
            this.Log().Error($"{className} - Erro: {exception.Message}", exception);
            return new OpenException(exception, exception.Message);
        }

        #endregion Methods
    }
}