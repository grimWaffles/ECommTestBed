using Grpc.Net.Client;
using UserServiceGrpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
namespace API_Gateway.Services
{
    public interface IUserService
    {
        Task<UserResponseSingle> GetUserById(int id);
        Task<UserResponseMultiple> GetAllUsersStream();
        Task<UserResponseMultiple> GetAllUsers();
        Task<UserLoginResponse> LoginUser(string username, string password);
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

        public async Task<UserResponseSingle> GetUserById(int id)
        {
            try
            {
                UserRequestSingle request = new UserRequestSingle() { Id = id };
                UserResponseSingle response = await userClient.GetUserByIdAsyncAsync(request);

                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                Console.WriteLine("-> Failed to fetch user");
                return null;
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
                using (var call = userClient.GetAllUsersStream(new Empty()))
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        response.List.Add(call.ResponseStream.Current);
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<UserLoginResponse> LoginUser(string username, string password)
        {
            UserLoginResponse response = new UserLoginResponse() { Status = -1, ErrorMessage = "Failed to authenticate user" };

            try
            {
                UserLoginRequest request = new UserLoginRequest() { Password = password, Username = username };

                response = await userClient.LoginUserAsync(request);

                return await Task.FromResult(response);
            }
            catch(Exception e)
            {
                return await Task.FromResult(response);
            }
        }
    }
}
