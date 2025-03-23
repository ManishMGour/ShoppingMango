using Mango.MessageBus;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.RabbitMQSender;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {

        private readonly IAuthService _authService;
        //private readonly IMessageBus _messageBus;
        private readonly IRabbitMQAuthMessageSender _messageBus;
        private readonly IConfiguration _configuration;
        protected ResponseDto _responseDto;

        //public AuthAPIController(IAuthService authService, IMessageBus messageBus, IConfiguration configuration)
        public AuthAPIController(IAuthService authService, IRabbitMQAuthMessageSender messageBus, IConfiguration configuration)
        {
            _authService = authService;
            _messageBus = messageBus;
            _configuration = configuration;
            _responseDto = new();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = errorMessage;
                return BadRequest(_responseDto);
            }
            //await _messageBus.PublishMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueName:RegisterUserQueue"));
             _messageBus.SendMessage(model.Email, _configuration.GetValue<string>("TopicAndQueueName:RegisterUserQueue"));
            return Ok(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);

            if (loginResponse.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or Password is incorrect";
                return BadRequest(_responseDto);
            }
            _responseDto.Result = loginResponse;
            return Ok(_responseDto);
        }


        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());

            if (!assignRoleSuccessful)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error encountered";
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }

    }
}
