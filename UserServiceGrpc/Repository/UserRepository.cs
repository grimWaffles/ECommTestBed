using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserServiceGrpc.Database;
using UserServiceGrpc.Models.Entities;

namespace UserServiceGrpc.Repository
{
    public interface IUserRepository
    {
        Task<List<UserModel>> GetUsers();
        Task<UserModel> GetUserById(int id);
        int CreateUser(UserModel user);
        int UpdateUser(UserModel user);
        int DeleteUser(UserModel user);
        Task<UserModel> GetUserByUsername(string username);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext context)
        {
            _db = context;
        }

        public int CreateUser(UserModel user)
        {
            try
            {
                _db.Users.Add(user);
                _db.SaveChangesAsync();

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int DeleteUser(UserModel user)
        {
            try
            {
                _db.Users.Remove(user);
                _db.SaveChangesAsync();

                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<UserModel> GetUserById(int id)
        {
            try
            {
                UserModel u = await _db.Users.Include(u => u.Role).Where(u => u.Id == id).FirstAsync();

                return u;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<UserModel>> GetUsers()
        {
            try
            {
                List<UserModel> list = await _db.Users.Include(u => u.Role).ToListAsync();

                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int UpdateUser(UserModel user)
        {
            try
            {
                _db.Users.Add(user);

                _db.SaveChangesAsync();

                return 1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public async Task<UserModel> GetUserByUsername(string username)
        {
            try
            {
                UserModel user = await _db.Users.AsNoTracking().Where(u => u.Username == username).Select(u=> new UserModel
                {
                    Id = u.Id, Username = u.Username, Password = u.Password
                }).
                FirstAsync();

                return user;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
