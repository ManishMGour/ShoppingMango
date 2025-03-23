using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Message;
using Mango.Services.EmailAPI.Model;
using Mango.Services.EmailAPI.Model.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;
        public EmailService(DbContextOptions<AppDbContext> dboptions)
        {
            _dbOptions = dboptions;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder messgae = new StringBuilder();

            messgae.AppendLine("<br/>Cart Email Requested ");
            messgae.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            messgae.Append("<br/>");
            messgae.Append("<ul>");

            foreach (var item in cartDto.CartDetails)
            {
                messgae.Append("<li>");
                messgae.Append(item.Product.Name + " x " + item.Count);
                messgae.Append("</li>");
            }
            messgae.Append("</ul>");
            await LogAndEmail(messgae.ToString(), cartDto.CartHeader.Email);
        }

        public async Task LogOrderPlaced(RewardsMessage rewardsMessage)
        {
            string message = "New Order Placed. <br/> Order ID : " + rewardsMessage.OrderId;
            await LogAndEmail(message, "manishgour1995@gmail.com");
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            var message = "User Registeration Successful. <br/> Email : " + email;
            await LogAndEmail(message, "manishgour1995@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLogger = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };

                await using var _db = new AppDbContext(_dbOptions);
                await _db.emailLoggers.AddAsync(emailLogger);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
