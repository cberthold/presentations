var async = require('asyncawait/async');
var await = require('asyncawait/await');
var Promise = require('bluebird');
var sourcedRepoMongo = require('sourced-repo-mongo');
var util = require('util');
var EventEmitter = require('events').EventEmitter;
var _ = require('lodash');
var uuid = require('node-uuid');

var commandRegistry = require('./command-registry.js');
var domain = require('./domain.js');

var Repository = sourcedRepoMongo.Repository;
var TodoList = domain.TodoList;

var tenantId = '4558ba3f-ad7b-4292-9018-c1a0111fdcb3';

var getRepository = function() {
  var repository = new Repository(TodoList);
  var asyncRepository = Promise.promisifyAll(repository);
  return asyncRepository;
};

var getOrCreateTodoList = async (function(repository) {
  
    var list = await (repository.getAsync(tenantId));
  
    if(list) return list;
    
    list = new TodoList();
    list.create({ id: tenantId });
    
    var committed = await (repository.commitAsync(list));
    
    list = await (repository.getAsync(tenantId));
    
    return list;
});

var addItem = async (function(data) {

  data.itemId = uuid.v4();

  var repository = getRepository();
  
  var list = await (getOrCreateTodoList(repository));

  list.addItem(data);
  
  var committed = await (repository.commitAsync(list));
  
  return list.findItemById(data.itemId);
    
});

var removeItem = async (function(data) {

  var itemToRemove = data;

  var repository = getRepository();
  
  var list = await (getOrCreateTodoList(repository));

  list.removeItem(itemToRemove);
  
  var committed = await (repository.commitAsync(list));
  
  return list.findItemById(data.itemId);
    
});  

var renameItem = async (function(data) {
  var repository = getRepository();
  
  var list = await (getOrCreateTodoList(repository));

  list.renameItem(data);
   
  var committed = await (repository.commitAsync(list));
  
  return list.findItemById(data.itemId);
    
});  

var getItemById = async (function(data) {
  var repository = getRepository();
  
  var list = await (getOrCreateTodoList(repository));

  return list.findItemById(data.itemId);  
});

var getAll = async (function(data) {
  var repository = getRepository();
  
  var list = await (getOrCreateTodoList(repository));

  if(!list) return null;

  return list.todos;
});

var deleteAll = async (function(data) {
  var repository = getRepository();
  
  var list = await (getOrCreateTodoList(repository));

  list.deleteAll();
  
  var committed = await (repository.commitAsync(list));
  
  return "All ToDos Deleted";
    
});

commandRegistry.register('getAll', getAll);
commandRegistry.register('getItemById', getItemById); 

commandRegistry.register('addItem', addItem);
commandRegistry.register('removeAll', deleteAll);
commandRegistry.register('removeItem', removeItem);
commandRegistry.register('renameItem', renameItem);
commandRegistry.register('getItemById', getItemById);