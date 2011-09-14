using System;

namespace Beheer.BusinessObjects.Dictionary
{
    /// <summary>
    /// Obsolete: gebruik IBeheerContextEntity.
    /// </summary>
    public interface IThema
    {
        int Id { get; set; }
        string ThemaNaam { get; set; }
        ValueType Created { get; set; }
    }
}