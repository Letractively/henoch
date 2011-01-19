using System.ComponentModel;
using System.Windows.Forms;
using TextBox = System.Web.UI.WebControls.TextBox;

namespace ApplicationTypes.DesignPatterns
{
    /// <summary>
    /// For web.
    /// </summary>
    public interface ILogControl: IComponent
    {
        TextBox Message { get; set; }
        ProgressBar ProgressBarLogger { get; set; }
    }
    /// <summary>
    /// For Desktop
    /// </summary>
    public interface ILogControl2 : IComponent
    {
        System.Windows.Forms.TextBox Message { get; set; }
        ProgressBar ProgressBarLogger { get; set; }
    }
}