using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Web2.Code
{
    public class NotificationHub : Hub
    {
        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }

        public void SendToEveryone(NotificationInput input)
        {
            var output = new NotificationOutput
            {
                From = input.From,
                Data = "EVERYONE"
            };

            Clients.All.sendNotification(output);
        }

        public void SendToMe(NotificationInput input)
        {
            var output = new NotificationOutput
            {
                From = input.From,
                Data = "ME"
            };

            Clients.Caller.sendNotification(output);
        }

        public void JoinExcitingGroup(NotificationInput input)
        {   
            Groups.Add(Context.ConnectionId, "Exciting");
            
        }

        public void SendToExcitingGroup(NotificationInput input)
        {
            var output = new NotificationOutput
            {
                From = input.From,
                Data = "Exciting"
            };

            Clients.Group("Exciting").sendNotification(output);
        }
    }
}