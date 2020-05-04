/// <binding AfterBuild='default' Clean='clean' />

var gulp = require('gulp');
var del = require('del');
var less = require('gulp-less');
var sass = require('gulp-sass');
var concat = require('gulp-concat');
const minify = require('gulp-minify');
var cssmin = require('gulp-cssmin');

var paths = {
    scripts: ['wwwroot/uncompiled/ts/**/*.js'],
    webroot: 'wwwroot/'
};

gulp.task('clean', function () {
    return del(['wwwroot/compiled/js/*', 'wwwroot/compiled/css/*', 'wwwroot/bundles/css/*', 'wwwroot/bundles/js/*']);
});

gulp.task('scripts', function () {
    return gulp.src(paths.scripts)
        .pipe(gulp.dest('wwwroot/compiled/js'));
});

gulp.task('less', function () {
    return gulp.src('wwwroot/uncompiled/less/**/*.less')
        .pipe(less())
        .pipe(gulp.dest(paths.webroot + 'compiled/css'));
});

gulp.task("sass", function () {
    return gulp.src('wwwroot/uncompiled/sass/**/*.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.webroot + 'compiled/css'));
});

gulp.task('bundle-js', function () {
    return gulp.src(paths.webroot + 'compiled/js/*.js')
        .pipe(concat('bundle.js'))
        .pipe(minify())
        .pipe(gulp.dest(paths.webroot + '/bundles/js'));
});

gulp.task('bundle-css', function () {
    return gulp.src(paths.webroot + 'compiled/css/*.css')
        .pipe(concat('bundle.css'))
        .pipe(cssmin())
        .pipe(gulp.dest(paths.webroot + '/bundles/css'));
});

gulp.task('style', gulp.parallel('less', 'sass'));

gulp.task('bundle', gulp.parallel('bundle-css', 'bundle-js'));

gulp.task('default', gulp.series('clean', 'scripts', 'style', 'bundle'));