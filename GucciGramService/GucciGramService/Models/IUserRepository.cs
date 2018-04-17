using System.Linq;

namespace GucciGramService.Models
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
    }
}
