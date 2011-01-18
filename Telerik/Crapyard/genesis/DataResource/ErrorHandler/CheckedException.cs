using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

///
///  Copyright (C) 2008  D.S. Modiwirijo
///  This program is free software: you can redistribute it and/or modify
///  it under the terms of the GNU General Public License as published by
///  the Free Software Foundation, either version 3 of the License, or
///  (at your option) any later version.
///  This program is distributed in the hope that it will be useful,
///  but WITHOUT ANY WARRANTY; without even the implied warranty of
///  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
///  GNU General Public License for more details.
///  You should have received a copy of the GNU General Public License
///  along with this program.  If not, see <http://www.gnu.org/licenses/>.
namespace ExceptionHandler
{
	//TODO: replace type enumeration with polymorhism to describe class behaviour.

	#region ErrorType

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

	#endregion


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
            Message = message;
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
            Message= message;
		}

        /// <summary>
        /// Contructs to initialize the error display.
        /// </summary>
        /// <param name="errNumber"></param>
        /// <param name="errMessage"></param>
        /// <param name="isExpanded"></param>
		public CheckedException(int errNumber, string errMessage, bool isExpanded)
		{
            Message = errMessage;
		    ErrNumber = errNumber;
			ErrorType = (ErrorType) errNumber;
			GetExpandedStatus = isExpanded;

            //if (StackTrace != null) string.Format(CultureInfo.InvariantCulture,"{0}:\n{1}", errMessage, StackTrace.ToString());

		}

        /// <summary>
        /// OBSOLETE: Replace type enumeration to describe class behaviour with polymorhism.
        /// </summary>
        public ErrorType ErrorType { get; private set; }

        /// <summary>
        /// Represents the exception message.
        /// </summary>
        public new string Message { get; set; }


		public bool SetExpandedStatus
		{
		    set { GetExpandedStatus = value; }
		    get { return GetExpandedStatus; }
		}

        public int ErrNumber { get; private set; }

        public bool GetExpandedStatus { get; private set; }




        //void ICheckedException2.ShowErrorDialog()
		//{
		//    ShowErrorDialog();
		//}

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