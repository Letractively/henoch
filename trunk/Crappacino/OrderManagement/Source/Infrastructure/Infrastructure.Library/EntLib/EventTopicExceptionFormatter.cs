//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by this guidance package as part of the solution template
//
// The EventTopicExceptionFormatter class is an Enterprise Library TextExceptionFormatter
// that will format an EventTopicException in a clear way
// 
//  
//
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

using System;
using System.IO;
using Microsoft.Practices.CompositeUI.EventBroker;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace OrderManagement.Infrastructure.Library.EntLib
{
    public class EventTopicExceptionFormatter : TextExceptionFormatter
    {
        public EventTopicExceptionFormatter(TextWriter writer, Exception exception)
            : base(writer, exception)
        {
        }

        protected override void WriteException(Exception exceptionToFormat, Exception outerException)
        {
            EventTopicException ete = exceptionToFormat as EventTopicException;
            if (ete != null)
            {
                foreach (Exception ex in ete.Exceptions)
                {
                    base.WriteException(ex, null);
                }
            }
        }
    }
}
