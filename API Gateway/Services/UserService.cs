using Grpc.Net.Client;
using UserServiceGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
namespace API_Gateway.Services
{
    public interface IUserService
    {
        Task GetUserById(int id);
        Task<UserResponseMultiple> GetAllUsersStream();
        Task<UserResponseMultiple> GetAllUsers();
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private string userServiceAddress;
        private GrpcChannel channel;
        private User.UserClient userClient;

        public UserService(IConfiguration configuration)
        {
            _config = configuration;

            userServiceAddress = _config["Microservices:userService"] ?? "";

            channel = GrpcChannel.ForAddress(userServiceAddress);

            userClient = new User.UserClient(channel);
        }

        public async Task GetUserById(int id)
        {
            try
            {
                UserRequestSingle request = new UserRequestSingle() { Id = id };
                UserResponseSingle response = await userClient.GetUserByIdAsyncAsync(request);
            }
            catch (Exception e)
            {
                Console.WriteLine("-> Failed to fetch user");
            }
        }

        public async Task<UserResponseMultiple> GetAllUsers()
        {
            try
            {
                UserResponseMultiple response = await userClient.GetAllUsersAsync(new Empty());

                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<UserResponseMultiple> GetAllUsersStream()
        {
            UserResponseMultiple response = new UserResponseMultiple();
            try
            {
                using(var call = userClient.GetAllUsersStream(new Empty()))
                {
                    while(await call.ResponseStream.MoveNext())
                    {
                        response.List.Add(call.ResponseStream.Current);
                    }
                }

                return response;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
