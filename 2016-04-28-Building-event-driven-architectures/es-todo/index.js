'use strict';
var async = require('asyncawait/async');
var await = require('asyncawait/await');

var bodyParser = require('body-parser');
var express = require('express');
var http = require('http');
var configJs = require('./config.js');
var domain = require('./domain.js');
var commandHandlers = require('./command-handlers.js');
var commandRegistry = require('./command-registry.js');

var todomvc = require('todomvc');
var todomvcApi = require('todomvc-api');
var io = require('socket.io');

var amqpClient = require('amqp10').Client;
var Promise = require('bluebird');

var config = configJs.config();
console.log('loading mongo');
require('sourced-repo-mongo/mongo').connect(config.MONGO_URL);
console.log('done loading');

var api = module.exports.api = express();
api.use(bodyParser.json());

api.use(function(req, res, next) {
  res.header("Access-Control-Allow-Origin", "*");
  res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
  next();
});

var app = module.exports.app = express();
app.use(express.static(__dirname + '/static'));
app.use('/api', [todomvcApi.server, api]);

app.get('/', function(req, res) {
  res.redirect('/examples/angularjs');
});
app.use(todomvc);

app.get('/_ah/health', function(req, res) {
  res.status(200)
    .set('Content-Type', 'text/plain')
    .send('ok');
});

var handleResponse = function(promise, res, successStatus) {

  promise.then(function(payload) {
  
    if(successStatus) {
      res.status(successStatus);
    }
  
    res.json(payload);
  
  }).catch(function(err) {
    console.error(err);
    res.status(err.code).send(err.message);
  });

};

var executeCommand = function(command, res, successStatus) {
  var returnValue = async (function() {
     var result = await (command.execute());
     
     return result;
  }); 
  
  handleResponse(returnValue(), res);  
};

// API Routes
api.get('/', function(req, res) {
  res.status(200)
    .set('Content-Type', 'text/plain')
    .send('ok');
});

api.get('/todos', function(req, res) {
  var command = commandRegistry.build('getAll', null);
  executeCommand(command, res);
});

api.get('/todos/:id', function(req, res) {
  var id = req.param('id');
  var command = commandRegistry.build('getItemById', id);
  executeCommand(command, res);  
});


api.put('/todos/:id', function(req, res) {
  var id = req.param('id');
  var todo = req.body;
  todo.itemId = id;
  var command = commandRegistry.build('renameItem', todo);
  executeCommand(command, res);
});

api.post('/todos', function(req, res) {
  var todo = req.body;
  var command = commandRegistry.build('addItem', todo);
  executeCommand(command, res, 201);
});


api.delete('/todos', function(req, res) {
  var command = commandRegistry.build('removeAll', null);
  executeCommand(command, res, 204);
});

api.delete('/todos/:id', function(req, res) {
  var id = req.param('id');
  var itemToRemove = { itemId: id };
  var command = commandRegistry.build('removeItem', itemToRemove);
  executeCommand(command, res, 204);
});

var serverPort = process.env.PORT || 8080;

var server = http.createServer(app).listen(serverPort, function() {
  console.log("Express server listening on port " + serverPort);
});

var sio = io.listen(server);

var address_list = new Array();

// wire up socket.io
sio.sockets.on('connection', function(socket) {
  var address = socket.handshake.address;
  
  if(address_list[address]) {
    var socketid = address_list[address].list;
    socketid.push(socket.id);
    address_list[address].list = socketid;
  } else {
    var socketid = new Array();
    socketid.push(socket.id);
    address_list[address] = new Array();
    address_list[address].list = socketid;
  }
  
  // disconnected state
  socket.on('disconnect', function() {
    var socketid = address_list[address].list;
    delete socketid[socketid.indexOf(socket.id)];
    if(Object.keys(socketid).length == 0) {
      delete address_list[address];
    }
  });
  
});

var client = new amqpClient();
client.connect('amqp://localhost')
  .then(function() {
    return Promise.all([
      client.createReceiver('amq.topic'),
      client.createSender('amq.topic')
    ]);
  })
  .spread(function(receiver, sender) {
    receiver.on('errorReceived', function(err) {
      console.log("error: ", err);
    });
    receiver.on('message', function(message) {
      console.log('Rx message: ', message.body);
    });
    
    return sender.send({ key: "value" });
  })
  .error(function(err) {
    console.log("error: ", err);
  });