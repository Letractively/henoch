namespace MetaData.Beheer.Interface.BusinessEntities
{
    /// <summary>
    /// Obsolete: gebruik IBeheerContextEntity.
    /// </summary>
    public interface ICategorie
    {
        long Id { get; set; }

        string Categorienaam { get; set; }
    }
}