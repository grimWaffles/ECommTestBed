using Azure;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.IdentityModel.Tokens;
using UserServiceGrpc.Helpers;
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

        //Private functions
        private UserModel ConvertRequestToModel(CreateUserRequest r)
        {
            return new UserModel
            {
                Id = r.Id,
                Username = r.Username,
                Email = r.Email,
                Password = r.Password,
                MobileNo = r.MobileNo,
                RoleId = r.RoleId,
                IsDeleted = Convert.ToBoolean(r.IsDeleted)
            };
        }

        private CreateUserRequest ConvertModelToRequest(UserModel r)
        {
            return new CreateUserRequest
            {
                Id = r.Id,
                Username = r.Username,
                Email = r.Email,
                Password = r.Password,
                MobileNo = r.MobileNo,
                RoleId = r.RoleId,
                IsDeleted = Convert.ToInt32(r.IsDeleted)
            };
        }
        
        //CRUD Operations
        public override async Task<UserCrudResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            UserModel userModel = await _repo.GetUserByUsername(request.Username);
            UserCrudResponse response = new UserCrudResponse();

            if (userModel != null)
            {
                response.Status = 0;

                if (userModel.Username != request.Username)
                {
                    response.ErrorMesage = response.ErrorMesage.IsNullOrEmpty() ? "Username already exists" : response.ErrorMesage + " | " + "Username already exists";
                }
                if (userModel.Email == request.Email)
                {
                    response.ErrorMesage = response.ErrorMesage.IsNullOrEmpty() ? "Email ID already exists" : response.ErrorMesage + " | " + "Email ID already exists";
                }
                if (userModel.MobileNo == request.MobileNo)
                {
                    response.ErrorMesage = response.ErrorMesage.IsNullOrEmpty() ? "Mobile number already exists" : response.ErrorMesage + " | " + "Mobile number already exists";
                }

                return response;
            }

            UserModel requestModel = ConvertRequestToModel(request);
            requestModel.CreatedBy = request.UserId; requestModel.CreatedDate = DateTime.Now;

            int status = _repo.CreateUser(requestModel);

            response.Status = status;
            response.ErrorMesage = status == 1 ? "User added successfully" : "Failed to add user";

            return response;
        }

        public override async Task<UserCrudResponse> UpdateUser(CreateUserRequest request, ServerCallContext context)
        {
            UserModel userModel = await _repo.GetUserByUsername(request.Username);
            UserCrudResponse response = new UserCrudResponse();

            if (userModel == null)
            {
                response.Status = 0;
                response.ErrorMesage = "User does not exist";
                return response;
            }

            UserModel requestModel = ConvertRequestToModel(request);
            requestModel.ModifiedBy = request.UserId; requestModel.ModifiedDate = DateTime.Now;

            int status = _repo.UpdateUser(requestModel);

            response.Status = status;
            response.ErrorMesage = status == 1 ? "User updated successfully" : "Failed to update user";

            return response;
        }

        public override async Task<UserCrudResponse> DeleteUser(UserRequestSingle request, ServerCallContext context)
        {
            UserModel userModel = await _repo.GetUserById(request.Id);
            UserCrudResponse response = new UserCrudResponse();

            if (userModel == null)
            {
                response.Status = 0;
                response.ErrorMesage = "User does not exist";
                return response;
            }
            userModel.IsDeleted = true; userModel.ModifiedDate = DateTime.Now; ; userModel.ModifiedBy = request.UserId;

            int status = _repo.UpdateUser(userModel);

            response.Status = status;
            response.ErrorMesage = status == 1 ? "User updated successfully" : "Failed to update user";

            return response;
        }

        public override async Task<CreateUserRequest> GetUserByIdAsync(UserRequestSingle request, ServerCallContext context)
        {
            UserModel user = await _repo.GetUserById(request.Id);

            if (user == null)
            {
                return new CreateUserRequest()
                {
                    Id = 0
                };
            }

            return ConvertModelToRequest(user);
        }

        public override async Task<UserResponseMultiple> GetAllUsers(Empty request, ServerCallContext context)
        {
            List<UserModel> users = await _repo.GetUsers();

            if (users == null || users.Count == 0) { return new UserResponseMultiple(); }

            UserResponseMultiple usersResponse = new UserResponseMultiple();

            usersResponse.Users.AddRange(users.Select(r=> ConvertModelToRequest(r)).ToList());

            return usersResponse;
        }

        public override async Task GetAllUsersStream(Empty request, IServerStreamWriter<CreateUserRequest> responseStream, ServerCallContext context)
        {
            CreateUserRequest response = new CreateUserRequest();

            try
            {
                List<UserModel> userModels = await _repo.GetUsers();

                foreach (UserModel user in userModels)
                {
                    await responseStream.WriteAsync(ConvertModelToRequest(user));
                }
            }
            catch (Exception e)
            {
                response.Id = 0;
                await responseStream.WriteAsync(response);
            }
        }

        //User Authentication



    }
}
