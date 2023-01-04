"use strict";

let gulp = require("gulp");
let path = require('path');
let fs = require('fs');
let gulpif = require('gulp-if');
let merge = require("merge-stream");
let glob = require('glob');
let micromatch = require('micromatch');
let extendObject = require('extend-object');

let concat = require('gulp-concat');
let less = require('gulp-less');
const sass = require('gulp-sass')(require('sass'));
let uglify = require('gulp-uglify-es').default;
let cleanCss = require('gulp-clean-css');

let postcss = require("gulp-postcss");
let url = require("postcss-url");

const {watch} = require('gulp');

let investigatedPackagePaths = {};

let rootPath = path.resolve('./');

let resourceMapping;

function replaceAliases(text) {
    if (!resourceMapping.aliases) {
        return text;
    }

    for (let alias in resourceMapping.aliases) {
        if (!resourceMapping.aliases.hasOwnProperty(alias)) {
            continue;
        }

        text = replaceAll(text, alias, resourceMapping.aliases[alias]);
    }

    return text;
}

function replaceAll(text, search, replacement) {
    return text.replace(new RegExp(search, 'g'), replacement);
}

function requireOptional(filePath) {
    //TODO: Implement this using a library instead of try-catch!
    try {
        return require(filePath);
    } catch (e) {
        return undefined;
    }
}

function cleanDirsAndFiles(patterns) {
    const {dirs, files} = findDirsAndFiles(patterns);

    files.forEach(file => {
        try {
            fs.unlinkSync(file);
        } catch (_) {
        }
    });

    dirs.sort((a, b) => a < b ? 1 : -1);

    dirs.forEach(dir => {
        if (fs.readdirSync(dir).length) return;

        try {
            fs.rmdirSync(dir, {});
        } catch (_) {
        }
    });
}

function findDirsAndFiles(patterns) {
    const dirs = [];
    const files = [];

    const list = glob.sync('**/*', {dot: true});

    const matches = micromatch(list, normalizeGlob(patterns), {
        dot: true,
    });

    matches.forEach(match => {
        if (!fs.existsSync(match)) return;

        (fs.statSync(match).isDirectory() ? dirs : files).push(match);
    });

    return {dirs, files};
}

function normalizeGlob(patterns) {
    return patterns.map(pattern => {
        const prefix = /\*$/.test(pattern) ? '' : '/**';
        return replaceAliases(pattern).replace(/(!?)\.\//, '$1') + prefix;
    });
}

function normalizeResourceMapping(resourcemapping) {
    let defaultSettings = {
        aliases: {
            "@node_modules": "./node_modules",
            "@libs": "./wwwroot/libs"
        },
        clean: [
            "@libs"
        ]
    };

    extendObject(defaultSettings.aliases, resourcemapping.aliases);
    resourcemapping.aliases = defaultSettings.aliases;

    resourcemapping.clean = resourcemapping.clean || defaultSettings.clean;

    return resourcemapping;
}

function buildResourceMapping(packagePath) {
    if (investigatedPackagePaths[packagePath]) {
        return {};
    }

    investigatedPackagePaths[packagePath] = 'OK';

    let packageJson = requireOptional(path.join(packagePath, 'package.json'));
    let resourcemapping = requireOptional(path.join(packagePath, 'abp.resourcemapping.js')) || {};

    if (packageJson && packageJson.dependencies) {
        let aliases = {};
        let mappings = {};

        for (let dependency in packageJson.dependencies) {
            if (packageJson.dependencies.hasOwnProperty(dependency)) {
                let dependedPackagePath = path.join(rootPath, 'node_modules', dependency);
                let importedResourceMapping = buildResourceMapping(dependedPackagePath);
                extendObject(aliases, importedResourceMapping.aliases);
                extendObject(mappings, importedResourceMapping.mappings);
            }
        }

        extendObject(aliases, resourcemapping.aliases);
        extendObject(mappings, resourcemapping.mappings);

        resourcemapping.aliases = aliases;
        resourcemapping.mappings = mappings;
    }

    return resourcemapping;
}

function postCssUrlOptions() {
    return {
        url: function (asset) {
            // Ignore absolute URLs
            if (asset.url.substring(0, 1) === '/' || asset.url.startsWith("http") || asset.url.startsWith("//")) {
                return asset.url;
            }

            var outputFolder = '';

            if (asset.url.match(/\.(png|svg|jpg|gif)$/)) {
                outputFolder = 'img';
            } else if (asset.url.match(/\.(woff|woff2|eot|ttf|otf)[?]{0,1}.*$/)) {
                outputFolder = 'fonts';
            } else {
                // Ignore not recognized assets like data:image etc...
                return asset.url;
            }

            var fileName = path.basename(asset.absolutePath);
            var outputPath = path.join(rootPath, '/wwwroot/' + outputFolder + '/');

            gulp.src(asset.absolutePath).pipe(gulp.dest(outputPath));

            return '/' + outputFolder + '/' + fileName;
        }
    };
}

function getTask(source, destination) {
    return gulp.src(source)
        .pipe(gulpif(function (file) {
            return file.extname.match(/\.(sass|scss)$/);
        }, sass().on('error', sass.logError)))
        .pipe(gulpif(function (file) {
            return file.extname.match(/\.(less)$/);
        }, less({math: 'parens-division'})))
        .pipe(gulpif(function (file) {
            return file.extname.match(/\.(css|sass|scss)$/);
        }, postcss([url(postCssUrlOptions())])))
        .pipe(gulp.dest(destination));
}

function build() {
    resourceMapping = normalizeResourceMapping(buildResourceMapping(rootPath));

    cleanDirsAndFiles(resourceMapping.clean);

    let tasks = [];

    if (resourceMapping.mappings) {
        for (let mapping in resourceMapping.mappings) {
            if (resourceMapping.mappings.hasOwnProperty(mapping)) {
                let destination = replaceAliases(resourceMapping.mappings[mapping]);
                if (fs.existsSync(destination)) continue;

                let source = replaceAliases(mapping);

                tasks.push(getTask(source, destination));
            }
        }
    }

    return merge(tasks);
}

function buildDev() {
    resourceMapping = normalizeResourceMapping(buildResourceMapping(rootPath));

    cleanDirsAndFiles(resourceMapping.clean);

    let tasks = [];

    if (resourceMapping.mappings) {
        for (let mapping in resourceMapping.mappings) {
            if (resourceMapping.mappings.hasOwnProperty(mapping)) {
                let destination = replaceAliases(resourceMapping.mappings[mapping]);
                if (fs.existsSync(destination)) continue;

                let source = replaceAliases(mapping);

                tasks.push(getTask(source, destination));

                let watcher = watch(source);

                watcher.on('change', function (path, stats) {
                    console.log(`${path} updated`);

                    getTask(source, destination);
                });
            }
        }
    }

    return merge(tasks);
}


exports.build = build;
exports.buildDev = buildDev;
