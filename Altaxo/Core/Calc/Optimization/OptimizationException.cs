/*
 * OptimizationException.cs
 * 
 * Copyright (c) 2003-2004, dnAnalytics Project. All rights reserved.
*/

using System;
using System.Runtime.Serialization;

namespace Altaxo.Calc.Optimization {
    ///<summary>Base class for exceptions thrown from the Optimization namespace</summary>
  /// <remarks>
  /// <para>Copyright (c) 2003-2004, dnAnalytics Project. All rights reserved. See <a>http://www.dnAnalytics.net</a> for details.</para>
  /// <para>Adopted to Altaxo (c) 2005 Dr. Dirk Lellinger.</para>
  /// </remarks>
  [Serializable()]
    public sealed class OptimizationException : System.SystemException {

        ///<summary>Initializes a new instance of the <c>OptimizationException</c> class.</summary>
        public OptimizationException () : base () {}

        ///<summary>Initializes a new instance of the <c>OptimizationException</c> class with a specified error message.</summary>
        ///<param name="message">The error message that explains the reason for the exception.</param>
        public OptimizationException (String message) : base (message) {}

        ///<summary>Initializes a new instance of the <c>OptimizationException</c> class with a specified error message 
        ///and a reference to the inner exception that is the cause of this exception.</summary>
        ///<param name="message">The error message that explains the reason for the exception.</param>
        ///<param name="inner">The exception that is the cause of the current exception. 
        ///If the innerException parameter is not a null reference, the current exception is raised in a <c>catch</c> block 
        ///that handles the inner exception.</param>
        public OptimizationException (String message, System.Exception inner) : base(message,inner) {}  

        ///<summary>Initializes a new instance of the <c>OptimizationException</c> class with serialized data.</summary>
        ///<param name="info">The error message that explains the reason for the exception.</param>
        ///<param name="context">The error message that explains the reason for the exception.</param>
        private OptimizationException (SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}
