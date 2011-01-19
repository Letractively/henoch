using System;
using System.ComponentModel;
using System.Windows.Forms;
using TextBox = System.Web.UI.WebControls.TextBox;

namespace ApplicationTypes.DesignPatterns
{
    /// <summary>
    /// Use Control.BeginInvoke (for System.Windows.Forms) to implement async functions.
    /// 
    /// </summary>
    public abstract class Subject : LoggingBase, ILogControl
    {
        public abstract ISite Site { get; set; }
        public abstract event EventHandler Disposed;
        public abstract void Dispose();

        #region Implementation of ILogControl

        public abstract TextBox Message { get; set; }
        public abstract ProgressBar ProgressBarLogger { get; set; }

        #endregion
    }

}