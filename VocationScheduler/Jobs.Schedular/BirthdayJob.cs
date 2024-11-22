using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;

namespace VocationScheduler.Jobs.Schedular
{
    public class BirthdayJob
    {
        private readonly ApplicationDbContext _context;

        public BirthdayJob(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CheckBirthdays()
        {
            var today = DateTime.UtcNow;
            var upcomingBirthdays = await _context.Users
                .Where(u => u.Birthdate.Month == today.Month && u.Birthdate.Day == today.Day)
                .ToListAsync();

            foreach (var user in upcomingBirthdays)
            {
                await NotifyGroupMembers(user);
            }
        }

        private async Task NotifyGroupMembers(User birthdayUser)
        {
            var groupIds = await _context.UserGroupMembers
                .Where(ugm => ugm.UserId == birthdayUser.Id)
                .Select(ugm => ugm.GroupId)
                .ToListAsync();

            var groupMembers = await _context.UserGroupMembers
                .Where(ugm => groupIds.Contains(ugm.GroupId) && ugm.UserId != birthdayUser.Id)
                .ToListAsync();

            foreach (var member in groupMembers)
            {
                var email = await _context.Users
                    .Where(u => u.Id == member.UserId)
                    .Select(u => u.Email)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(email))
                {
                    await SendEmailAsync(email, birthdayUser);
                }
            }
        }

        private async Task SendEmailAsync(string email, User birthdayUser)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Admin", "admin@example.com"));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = "Upcoming Birthday Alert";
            message.Body = new TextPart("plain")
            {
                Text = $"Merhaba, {birthdayUser.Username}'in doğum günü yaklaşıyor! Lütfen kutlamayı unutmayın."
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.example.com", 587, false);
            await client.AuthenticateAsync("your_smtp_username", "your_smtp_password");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
