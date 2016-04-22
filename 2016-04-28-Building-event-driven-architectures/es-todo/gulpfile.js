'use strict';

var gulp = require('gulp');
var jshint = require('gulp-jshint');

var server = require('./server');
var api = server.api;
var app = server.app;

gulp.task('lint', function() {
  return gulp.src(['*.js', '*/*.js'])
    .pipe(jshint())
    .pipe(jshint.reporter('default'));
});

gulp.task('serve', function(cb) {
  app.listen(8080, cb);
});

