var Entity = require('sourced').Entity;
var log = require('debug')('todo-es');
var sourcedRepoMongo = require('sourced-repo-mongo');
var Repository = sourcedRepoMongo.Repository;
var util = require('util');
var EventEmitter = require('events').EventEmitter;
var _ = require('lodash');
var uuid = require('node-uuid');


/* Todo aggregate */

function TodoList() {
  this.todos = [];

  Entity.apply(this, arguments);
};

util.inherits(TodoList, Entity);

TodoList.prototype.findItemById = function(itemId) {
  var item = _.find(this.todos, function(item) {
    return item.id === itemId;
  });
  
  return item;
};

TodoList.prototype.create = function(param) {
  this.id = param.id;
  this.digest('create', param);
  this.emit('todoListCreated', param, this);
};

TodoList.prototype.addItem = function(param) {
  var title = param.title;
  var order = param.order;
  var completed = param.completed || false;
  var itemId = param.itemId || uuid.v4();
  
  var item = new TodoItem(itemId, title, order, completed);
  this.todos.push(item);
  this.digest('addItem', param);
  this.emit('todoItemAdded', param, this);
};

TodoList.prototype.removeItem = function(param) {
  var itemId = param.itemId;
  _.remove(this.todos, function(item) {
    return item.id === itemId;
  });
  this.digest('removeItem', param);
  this.emit('todoItemRemoved', param, this);
};

TodoList.prototype.renameItem = function(param) {
  var itemId = param.itemId;
  var item = this.findItemById(itemId);
  
  if(item) {
    item.rename(param.title);
    item.reorder(param.order);
    item.setCompleted(param.completed);
  }
  else {
    return;
  }
  this.digest('renameItem', param);
  this.emit('todoItemRenamed', param, this);
};

TodoList.prototype.removeAll = function(param) {
  param = null;
  this.todos = [];
  this.digest('removeAll', param);
  this.emit('allTodoItemsRemoved', param, this);
};

/* Todo Item Entity */
function TodoItem(id, title, order, completed) {
  this.id = id;
  this.title = title;
  this.order = order;
  this.completed = completed;
  this.rename = function(newName) {
    this.title = newName;
  };
  this.reorder = function(newOrder) {
    this.order = newOrder;
  };  
  this.setCompleted = function(isCompleted) {
    this.completed = isCompleted;
  };
  this.toTodo = function() {
    var todo = {};
    todo._id = this.id;
    todo.title = this.title;
    todo.order = this.order;
    todo.completed = this.completed;
    
    return todo;
  }
}

module.exports.TodoList = TodoList;