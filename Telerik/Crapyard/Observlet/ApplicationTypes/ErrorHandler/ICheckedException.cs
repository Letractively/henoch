using System.Collections;

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