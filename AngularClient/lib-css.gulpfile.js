const gulp = require("gulp");
const filter = require("gulp-filter");
const purify = require("gulp-purify-css");
const clean = require("gulp-clean");
const { series } = require("gulp");
const sass = require("gulp-sass")(require("sass"));

gulp.task("css", () => {
    return gulp
            .src("./projects/gnappo-lib/assets/scss/gnappo_theme.scss")
            .pipe(sass({outputStyle: 'compressed'}))
            .pipe(gulp.dest("./dist/gnappo-lib/assets"));
});

exports.default = series(
    "css"
);