namespace Dictionary.BusinessObjects
{
    public class ParentKeyEntity:IAlternateKey
    {
        public ParentKeyEntity()
        {
            Id = -1;//-1 betekent: bestaat niet. 0 is de laagste waarde voor indices.
        }
        public string Tablename { get; set; }

        public int Id { get; set; }

        public string DataKeyValue { get; set; }
        public string DataKeyName { get; set; }

        public ParentKeyEntity Parent { get; set; }
    }
}