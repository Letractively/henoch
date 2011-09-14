namespace Beheer.BusinessObjects.Dictionary
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