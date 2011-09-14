using Beheer.BusinessObjects.Dictionary;

namespace MetaData.Beheer.Interface.Services
{
    public interface ICategorieService : IBeheerService
    {
        BeheerContextEntity Selected { get; set; }
        
    }
}
