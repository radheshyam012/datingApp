namespace API.Interface
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository {get;}

        Task<bool> Complated();
       // bool hasChange();
    }
}