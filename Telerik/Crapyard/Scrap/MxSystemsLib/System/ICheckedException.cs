using System.Collections;

namespace MxSystemsLib.System
{
    /// <summary>
    /// Interfaces with an error dialog.
    /// </summary>
    public interface ICheckedException
    {
        ErrorType ErrorType { get; }

    }
}