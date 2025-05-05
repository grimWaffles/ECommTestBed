using Azure;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using UserServiceGrpc.Models.Entities;
using UserServiceGrpc.Repository;

namespace UserServiceGrpc.Services
{
    public class UserService : User.UserBase
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository userRepository)
        {
            _repo = userRepository;
        }

        public override async Task<UserResponseSingle> GetUserByIdAsync(UserRequestSingle request, ServerCallContext context)
        {
            UserResponseSingle response = new UserResponseSingle();
            UserModel user = await _repo.GetUserById(request.Id);

            if (user == null)
            {
                response.Status = -1;
                response.ErrorMessage = "User does not exist";

                return await Task.FromResult(response);
            }

            response.Status = 1;
            response.UserId = user.Id;
            response.Username = user.Username;
            response.Email = user.Email;
            response.RoleName = user.Role == null ? "" : user.Role.Name.ToString();

            return await Task.FromResult(response);
        }

        public override async Task<UserResponseMultiple> GetAllUsers(Empty request, ServerCallContext context)
        {
            UserResponseMultiple response = new UserResponseMultiple();

            try
            {
                List<UserModel> userModels = await _repo.GetUsers();

                if (userModels == null)
                {
                    response.Status = -1;
                    response.ErrorMessage = "Failed to get users";

                    return await Task.FromResult(response);
                }

                foreach (UserModel user in userModels)
                {
                    response.List.Add(new UserResponseSingle()
                    {
                        Status = 1,
                        UserId = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        RoleName = user.Role == null ? "" : user.Role.Name.ToString(),
                    });

                    response.Status = 1;
                    response.ErrorMessage = "";
                }

                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                response.Status = -1;
                response.ErrorMessage = "Failed to get users";

                return await Task.FromResult(response);
            }
        }

        public override async Task GetAllUsersStream(Empty request, IServerStreamWriter<UserResponseSingle> responseStream, ServerCallContext context)
        {
            UserResponseSingle response = new UserResponseSingle();

            try
            {
                List<UserModel> userModels = await _repo.GetUsers();

                if (userModels == null)
                {
                    response.Status = -1;
                    response.ErrorMessage = "Failed to get users";

                    await responseStream.WriteAsync(response);
                }
                else
                {
                    foreach (UserModel user in userModels)
                    {
                        response = new UserResponseSingle()
                        {
                            Status = 1,
                            UserId = user.Id,
                            Username = user.Username,
                            Email = user.Email,
                            RoleName = user.Role == null ? "" : user.Role.Name.ToString(),
                        };

                        await responseStream.WriteAsync(response);
                    }
                }
            }
            catch (Exception e)
            {
                response.Status = -1;
                response.ErrorMessage = "Failed to get users";

                await responseStream.WriteAsync(response);
            }
        }

        public override async Task<UserLoginResponse> LoginUser(UserLoginRequest request, ServerCallContext context)
        {
            UserLoginResponse response = new UserLoginResponse();
            response.Status = -1;
            response.ErrorMessage = "Failed to verify login";

            try
            {
                UserModel u = await _repo.GetUserByUsername(request.Username);

                if (u == null)
                {
                    return await Task.FromResult(response);
                }

                if (u.Password != request.Password)
                {
                    response.ErrorMessage = "Password is incorrect!";

                    return await Task.FromResult(response);
                }

                response.Status = 1; response.ErrorMessage = "Login successful!"; response.AccessToken = "token";

                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                return await Task.FromResult(response);
            }
        }
    }
}
