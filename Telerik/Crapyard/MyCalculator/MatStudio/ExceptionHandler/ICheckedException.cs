using System.Collections;

namespace MatStudio.ExceptionHandler
{
    /// <summary>
    /// Interfaces with an error dialog.
    /// </summary>
    public interface ICheckedException
    {
        /// <summary>
        /// Halts process.
        /// </summary>
        /// <remarks>SpecificationException explicitly implements ICheckedException2</remarks>
        //void ShowErrorDialog();

        string Message { get; set; }
        /// <summary>
        /// OBSOLETE: Replace type enumeration to describe class behaviour with polymorhism.
        /// </summary>
        ErrorType ErrorType { get; }


        bool SetExpandedStatus { set; get; }
        int ErrNumber { get; }
        bool GetExpandedStatus { get; }
        IDictionary Data { get; }
    }
}