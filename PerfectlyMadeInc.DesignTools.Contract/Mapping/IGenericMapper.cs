namespace PerfectlyMadeInc.DesignTools.Contract.Mapping
{
    public interface IGenericMapper
    {
        TTarget Map<TTarget>(object source);
    }
}
