namespace MetaData.Beheer
{
    public interface IDetailsController<TDetailObject>
    {
        TDetailObject GetMaster();
        void AddDetail(TDetailObject detail);
        TDetailObject GetSelectedMaster();
    }
}