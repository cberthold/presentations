#!/bin/sh
docker-compose run jekyll jekyll new blog
mv blog blog2
mv blog2/blog .
rm -rf blog2