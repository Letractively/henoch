using System;

namespace MetaData.Beheer.Interface.BusinessEntities
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