namespace MetaData.Beheer.Interface.BusinessEntities
{
    public interface IAlternateKey
    {
        string Tablename { get; set; }
        int Id { get; set; }
        string DataKeyName { get; set; }
    }
}