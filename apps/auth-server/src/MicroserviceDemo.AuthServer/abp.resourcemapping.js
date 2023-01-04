module.exports = {
    aliases: {
        "@node_modules": "./node_modules",
        "@wwwroot": "./wwwroot",
        "@libs": "./wwwroot/libs",
        "@resources": "./Resources"
    },
    clean: [
        "@libs",
        "@wwwroot/css",
        "@wwwroot/js",
        "@wwwroot/images",
        "@wwwroot/fonts"
    ],
    mappings: {
        "@resources/css/**/*.*": "@wwwroot/css/",
        "@resources/scss/**/*.*": "@wwwroot/css/",
        "@resources/js/**/*.*": "@wwwroot/js/",
        "@resources/images/**/*.*": "@wwwroot/images/",
        "@resources/fonts/**/*.*": "@wwwroot/fonts/"
    }
};