using Infrastructure.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Todo.BoundedContext.Commands;
using Todo.BoundedContext.Data;
using Todo.BoundedContext.ReadModel;

namespace TodoMvcApi.Controllers
{
    public class TodosController : ApiController
    {
        readonly ITodoListReadModelFacade readModel;
        readonly CommandDispatcher dispatcher;
        readonly Guid TenantId = Guid.Parse("2DF4109C-0B42-40C0-97B6-E9024EF47253");

        public TodosController(
            CommandDispatcher dispatcher,
            ITodoListReadModelFacade readModel)
        {
            this.dispatcher = dispatcher;
            this.readModel = readModel;
        }

        [HttpGet]
        [Route("api/Todos/Rebuild")]
        public void RebuildReadModel()
        {

        }

        [HttpGet]
        // GET api/Todos
        [Route("api/Todos")]
        public List<TodoItemDTO> Get()
        {
            return readModel.Get(TenantId).Todos;
        }

        [HttpGet]
        // GET api/Todos/5
        [Route("api/Todos/{id}")]
        public TodoItemDTO Get(Guid id)
        {
            var list = readModel.Get(TenantId);
            return list.Todos.FirstOrDefault(a=>a.ItemId == id);
        }

        [HttpPost]
        // POST api/Todos
        [Route("api/Todos")]
        public TodoItemDTO Post([FromBody]AddTodoItem cmd)
        {
            cmd.TenantId = TenantId;
            cmd.ItemId = Guid.NewGuid();
            dispatcher.Send(cmd);

            return Get(cmd.ItemId);
        }

        [HttpPut]
        // PUT api/Todos/5
        [Route("api/Todos/{id}")]
        public TodoItemDTO Put(Guid id, [FromBody]UpdateTodoItem cmd)
        {
            cmd.TenantId = TenantId;

            cmd.ItemId = id;
            dispatcher.Send(cmd);
            
            return Get(cmd.ItemId);
        }

        [HttpDelete]
        // DELETE api/Todos/5
        [Route("api/Todos/{id}")]
        public string Delete(Guid id)
        {
            var cmd = new RemoveTodoItem();
            cmd.TenantId = TenantId;
            cmd.ItemId = id;
            dispatcher.Send(cmd);

            return "All ToDos Deleted";
        }

        [HttpDelete]
        // DELETE api/Todos
        [Route("api/Todos")]
        public string DeleteAll()
        {
            var cmd = new RemoveCompletedTodoItems();
            cmd.TenantId = TenantId;
            dispatcher.Send(cmd);
            return "All ToDos Deleted";
        }
    }
}
