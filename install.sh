#!/bin/sh

# need the command line tools here -- this is a long install on mac
# this is used by the hapi tools in hapi-todo
#xcode-select --install

# current bug with NPM progress - make it go faster
npm set progress = false

brew update
brew doctor

#brew prune

brew install node
brew install git

brew install postgresql
initdb /usr/local/var/postgres -E utf8
sudo gem install lunchy
mkdir -p ~/Library/LaunchAgents
cp /usr/local/Cellar/postgresql/9.5.2/homebrew.mxcl.postgresql.plist ~/Library/LaunchAgents/

lunchy start postgres
#lunchy stop postgres

brew install mongodb
cp /usr/local/opt/mongodb/*.plist ~/Library/LaunchAgents
lunchy start mongodb
#lunchy stop mongodb

git clone https://github.com/pamo/hapi-todo/

cd hapi-todo
npm install node-gyp -g 
npm cache clean 
rm -rf node_modules
npm install
cd node_modules/mongodb/
npm install
cd ../..

# create "todos" database in robomongo
# node index.js
# run url http://www.todobackend.com/client/index.html?http://localhost:3000
