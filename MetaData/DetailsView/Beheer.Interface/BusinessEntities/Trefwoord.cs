namespace MetaData.Beheer.Interface.BusinessEntities
{
    /// <summary>
    /// Obsolete: gebruik BeheerContextEntity ipv Trefwoord.
    /// </summary>
    public class Trefwoord : BeheerContextEntity
    {
        public long Id { get; set; }

        public string Trefwoordnaam { get; set; }
    }
}