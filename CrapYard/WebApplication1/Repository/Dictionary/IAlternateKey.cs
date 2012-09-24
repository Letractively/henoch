namespace Dictionary.BusinessObjects
{
    public interface IAlternateKey
    {
        string Tablename { get; set; }
        int Id { get; set; }
        string DataKeyValue { get; set; }
        string DataKeyName { get; set; }
    }
}