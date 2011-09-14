namespace MetaData.Beheer.Interface.BusinessEntities
{
    /// <summary>
    /// Obsolete: gebruik IBeheerContextEntity.
    /// </summary>
    public interface ITrefwoord
    {
        long Id { get; set; }

        string Trefwoordnaam { get; set; }
    }
}