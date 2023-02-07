using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Hubs
{
    public class DekorHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DekorEvStartupAppDbContext _context;

        public DekorHub(UserManager<AppUser> userManager, DekorEvStartupAppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task BlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.isConfirmed = false;
            await  _context.SaveChangesAsync();
            await Clients.Client(user.ConnectionId).SendAsync("blockuser");
        }

        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser User = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;

                User.ConnectionId = Context.ConnectionId;

                var result = _userManager.UpdateAsync(User).Result;
                Clients.All.SendAsync("UserConnected", User.Id);
            }
            return base.OnConnectedAsync();
        }
      

   


    }
}
