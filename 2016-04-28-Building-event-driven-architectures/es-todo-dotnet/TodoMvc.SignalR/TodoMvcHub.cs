using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Todo.BoundedContext.Data;

namespace TodoMvc.SignalR
{
    public class TodoMvcHub : Hub
    {
        private static IHubContext context = 
            GlobalHost.ConnectionManager.GetHubContext<TodoMvcHub>();

        public static void SendBroadcast(TodoListDTO list)
        {
            context.Clients.All.receieveTodos(list);
        }
    }
}