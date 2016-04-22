var async = require('asyncawait/async');
var await = require('asyncawait/await');

var _ = require('lodash');

var handlers = {};

// 
var CommandHandler = function(handler)
{
  var handlerFunction = handler;
  
  var executeInternal = async(function(data)
  {
    var returnValue = await (handlerFunction(data));
    
    return returnValue;
  });
  
  return {
    execute: executeInternal
  };
};

var Command = function(name, data) {
  
  var nameCopy = _.clone(name);
  var dataCopy = _.clone(data);
  
  // checks if the command handler exists and 
  // executes the named handler with the data 
  var executeInternal = async (function() {
  
    if(!handlerExists(nameCopy)) return null;
  
    var handler = handlers[nameCopy];
    
    if(!dataCopy) dataCopy = null;
  
    var returnValue = await (handler.execute(dataCopy));
    
    return returnValue;
  });
  
  return {
    name: nameCopy,
    data: dataCopy,
    execute: executeInternal
  };
  
};
 
// checks to see if the named hadler exists
var handlerExists = function(handlerName) {
  return handlers.hasOwnProperty(handlerName);
};

// registers a command handler to execute logic
var registerCommandHandler = function(name, handlerFunction) {
  if(!handlerFunction) return;
  
  handlers[name] = new CommandHandler(handlerFunction);
};

// creates a new command to execute
var buildCommand = function(name, data) {
  return new Command(name, data); 
};

var Registry = function () {
return {
  register: registerCommandHandler,
  build: buildCommand
  };
};

// public side of command handler registry
module.exports = exports = new Registry();

