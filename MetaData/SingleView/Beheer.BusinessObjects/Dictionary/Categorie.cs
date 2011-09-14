namespace Beheer.BusinessObjects.Dictionary
{
    /// <summary>
    /// Obsolete: gebruik BeheerContextEntity ipv Categorie.
    /// </summary>
    public class Categorie : BeheerContextEntity, ICategorie
    {
        public long Id { get; set; }

        public string Categorienaam { get; set; }
    }
}