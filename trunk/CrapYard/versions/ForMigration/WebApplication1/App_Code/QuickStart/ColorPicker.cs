namespace Telerik.QuickStart
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for ColorPickerControl.
	/// </summary>
	public class ColorPicker : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DropDownList DropDownList1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( this.DropDownList1.Items.Count == 0 )
			{
				this.DropDownList1.DataSource = Enum.GetNames( typeof(KnownColor) );
				this.DropDownList1.DataBind();
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public Color SelectedColor
		{
			get
			{
				string colorValue = "Black";
				if ( DropDownList1.SelectedIndex >=0 )
				{
					colorValue = DropDownList1.Items[ DropDownList1.SelectedIndex ].Value;
				}
				KnownColor color = (KnownColor)Enum.Parse( typeof(KnownColor), colorValue );
				return Color.FromKnownColor( color );
			}
		}		
	}
}
