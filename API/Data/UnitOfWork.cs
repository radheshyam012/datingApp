using API.Interface;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        
        public UnitOfWork(DataContext context,IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;
            
        }
        public IUserRepository userRepository => new UserRepository(_context,_mapper);

        public async Task<bool> Complated()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public  bool hasChange()
        {
            return  _context.ChangeTracker.HasChanges();
        }
    }
}