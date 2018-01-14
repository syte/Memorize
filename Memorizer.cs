﻿using System;
using System.Collections.Generic;

namespace Sylveon.Memorize
{
    /// <summary>
    /// A class used to memoize a single-parameter function. Unrecommended,
    /// use the <see cref="Memorizer{T, TResult}"/> and <see cref="NullableMemorizer{T, TResult}"/>
    /// wrappers instead.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the parameter of the function
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the return value of the function
    /// </typeparam>
    /// <typeparam name="TDictionary">
    /// The type of the Dictionary to use.
    /// </typeparam>
    /// <seealso cref="Memorizer{T, TResult}" />
    /// <seealso cref="NullableMemorizer{T, TResult}" />
    public class Memorizer<T, TResult, TDictionary> where TDictionary : IDictionary<T, TResult>, new()
    {
        private readonly Func<T, TResult> _functionToMemorize;
        private readonly TDictionary _memorizedResults = new TDictionary();

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="func">
        /// The function to memoize.
        /// </param>
        public Memorizer(Func<T, TResult> func) => _functionToMemorize = func;

        /// <summary>
        /// Determines whether the result associated to a parameter has been memorized.
        /// </summary>
        /// <param name="param">
        /// The parameter to verify.
        /// </param>
        /// <returns>
        /// true if the result has been memorized; otherwise, false.
        /// </returns>
        public bool IsResultMemorized(T param) => _memorizedResults.ContainsKey(param);

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void ClearMemorizedResults() => _memorizedResults.Clear();

        /// <summary>
        /// Gets the memorized result associated with the specified input parameter.
        /// </summary>
        /// <param name="param">
        /// The input parameter of the result to get.
        /// </param>
        /// <returns>
        /// The result associated with the specified parameter. If the specified key is
        /// not found, throws a <see cref="KeyNotFoundException"/>.
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// The result has not been memorized yet.
        /// </exception>
        public TResult GetMemorizedResult(T param) => _memorizedResults[param];

        /// <summary>
        /// Gets the memorized result associated with the specified input parameter.
        /// </summary>
        /// <param name="param">
        /// The input parameter of the result to get.
        /// </param>
        /// <param name="result">
        /// When this method returns, contains the results associated with the
        /// specified parameter, if it was memorized; otherwise, the default value
        /// for the type of the result. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// true if the result has been memorized; otherwise, false.
        /// </returns>
        public bool TryGetMemorizedResult(T param, out TResult result) => _memorizedResults.TryGetValue(param, out result);

        /// <summary>
        /// Return the memorized result if memorized; otherwise call the function, memorize the
        /// result, and return it.
        /// </summary>
        /// <param name="param">
        /// The parameter that the function must be invoked with
        /// </param>
        /// <returns>
        /// The memorized result if memorized; otherwise the result of calling the function.
        /// </returns>
        public TResult Invoke(T param)
        {
            if(!IsResultMemorized(param))
            {
                _memorizedResults[param] = _functionToMemorize(param);
            }
            return _memorizedResults[param];
        }
    }

    /// <summary>
    /// A wrapper for the main <seealso cref="Memorizer{T, TResult, TDictionary}" /> class taking a non-nullable value only.
    /// If you need one handling nullable values, use <see cref="NullableMemorizer{T, TResult}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the parameter of the function
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the return value of the function
    /// </typeparam>
    /// <seealso cref="Memorizer{T, TResult, TDictionary}" />
    /// <seealso cref="NullableMemorizer{T, TResult}" />
    public class Memorizer<T, TResult> : Memorizer<T, TResult, Dictionary<T, TResult>> where T : struct
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="func">
        /// The function to memoize.
        /// </param>
        public Memorizer(Func<T, TResult> func) : base(func) { }
    }

    /// <summary>
    /// A wrapper for the main <seealso cref="Memorizer{T, TResult, TDictionary}" /> class taking a nullable value only.
    /// If you need one handling non-nullable values, use <see cref="Memorizer{T, TResult}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the parameter of the function
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the return value of the function
    /// </typeparam>
    /// <seealso cref="Memorizer{T, TResult, TDictionary}" />
    /// <seealso cref="Memorizer{T, TResult}" />
    public class NullableMemorizer<T, TResult> : Memorizer<T, TResult, NullableDictionary<T, TResult>> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="func">
        /// The function to memoize.
        /// </param>
        public NullableMemorizer(Func<T, TResult> func) : base(func) { }
    }
}
