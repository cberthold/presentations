var Entity = require('sourced').Entity;
var log = require('debug')('todo-es');
var sourcedRepoMongo = require('sourced-repo-mongo');
var Repository = sourcedRepoMongo.Repository;
var util = require('util');
var EventEmitter = require('events').EventEmitter;
var _ = require('lodash');



/* Todo aggregate */

function TodoList() {
  this.todos = [];
  
  Entity.Apply(this, arguments);
};

util.inherits(TodoList, Entity);

TodoList.prototype.create(param) {
  this.id = param.id;
  this.digest('create', param);
  this.emit('todoListCreated', param, this);
};

TodoList.prototype.addItem(param) {
  var item = new TodoItem(param.itemId, name);
  this.todos.push(item);
  this.digest('addItem', param);
  this.emit('todoItemAdded', param, this);
};

TodoList.prototype.removeItem(param) {
  var itemId = param.itemId;
  _.remove(this.todos, function(item) {
    return item.id === itemId;
  });
  this.digest('removeItem', param);
  this.emit('todoItemRemoved', param, this);
};

TodoList.prototype.renameItem(param) {
  var itemId = param.itemId;
  var item = _.find(this.todos, function(item) {
    return item.id === itemId;
  });
  
  if(item) item.rename(param.name);
  this.digest('renameItem', param);
  this.emit('todoItemRenamed', param, this);
};

/* Todo Item Entity */
function TodoItem(id, name) {
  this.id = id;
  this.name = name;
  this.rename = function(newName) {
    this.name = newName;
  };  
}

