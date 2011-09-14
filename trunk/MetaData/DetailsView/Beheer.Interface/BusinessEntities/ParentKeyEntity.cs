namespace MetaData.Beheer.Interface.BusinessEntities
{
    public class ParentKeyEntity:IAlternateKey
    {
        public string Tablename { get; set; }

        public int Id { get; set; }

        public string DataKeyName { get; set; }

        public ParentKeyEntity Parent { get; set; }
    }
}