using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MxSystemsLib.System
{
    public enum ErrorType
    {
        /// <summary>
        /// If there is an exception-handle under construction.
        /// </summary>
        Unknown= 0, //
        /// <summary>
        /// If validation has failed.
        /// </summary>
        ValidationFailed = 2, //
        /// <summary>
        /// TODO: handle the runtime Unknown exceptions in General.
        /// </summary>
        General = 3, 
        /// <summary>
        /// Process MUST stop if there is wrong (user) input.
        /// </summary>
        ProcessFailure = 4,
        IO = 6,
        ConfigFailure = 7,
        /// <summary>
        /// Key must be unique or must exist in collection.
        /// </summary>
        UniquenessFailure = 9,
        /// <summary>
        /// Missing credentials .
        /// </summary>
        SecurityFailure = 10,
        /// <summary>
        /// Environment variable not set.
        /// </summary>
        EnvironmentFailure = 11,
        /// <summary>
        /// Parsing failed.
        /// </summary>
        ParseFailure =12
    }

    ///
    /// For guidelines regarding the creation of new exception types, see
    ///    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
    /// and
    ///    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
    ///
    /// 
    /// 1)Exceptions due to programming errors: 
    /// In this category, exceptions are generated due to programming errors (e.g., NullPointerException and IllegalArgumentException). 
    /// The client code usually cannot do anything about programming errors.
    /// 
    /// 2)Exceptions due to client code errors: 
    /// Client code attempts something not allowed by the API, and thereby violates its contract. 
    /// The client can take some alternative course of action, if there is useful information provided in the exception. 
    /// For example: an exception is thrown while parsing an XML document that is not well-formed. 
    /// The exception contains useful information about the location in the XML document that causes the problem. 
    /// The client can use this information to take recovery steps.
    /// 
    /// 3)Exceptions due to resource failures: 
    /// Exceptions that get generated when resources fail. 
    /// For example: the system runs out of memory or a network connection fails. The client's response to resource failures is context-driven. 
    /// The client can retry the operation after some time or just log the resource failure and bring the application to a halt.
    /// <summary>
    /// All application related logical/functional EXCEPTIONAL flow of control must be handled correctly.
    /// </summary>
    [Serializable]
    public sealed class CheckedException : Exception, ICheckedException, ISerializable
    {
        public CheckedException()
        {
            
        }

        /// <summary>
        ///  Contructs with only message.
        /// </summary>
        public CheckedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructs with base.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public CheckedException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Contructs with base.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        private CheckedException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// OBSOLETE: Replace type enumeration to describe class behaviour with polymorhism.
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="message"></param>
        public CheckedException(ErrorType errorType, string message)
            : base(message)
        {
            ErrorType = errorType;            
        }

 
        /// <summary>
        /// OBSOLETE: Replace type enumeration to describe class behaviour with polymorhism.
        /// </summary>
        public ErrorType ErrorType { get; private set; }

        #region ISerializable Members


        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            GetObjectData(info, context);
        }

        #endregion

    }
}