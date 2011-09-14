namespace Beheer.BusinessObjects.Dictionary
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