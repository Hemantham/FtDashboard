
/**
 * System configuration for Angular 2 samples
 * Adjust as necessary for your application needs.
 */
(function(global) {

  // map tells the System loader where to look for things
  var map = {
    'app':                        'app', // 'dist',
    '@angular':                   'node_modules/@angular',
    'angular2-in-memory-web-api': 'node_modules/angular2-in-memory-web-api',
    'rxjs': 'node_modules/rxjs',
    'ng2-nvd3': 'node_modules/ng2-nvd3/build/lib/ng2-nvd3.js',
    'linq-es2015': 'node_modules/linq-es5/dist/linq.js',    
    'babel-polyfill': 'node_modules/babel-polyfill/dist/polyfill.js',
    'ng2-select': 'node_modules/ng2-select',
    'angular2-highcharts': 'node_modules/angular2-highcharts/dist',
    'highcharts/highstock.src': 'node_modules/highcharts/highstock.src.js',
    'jspdf': 'node_modules/jspdf/dist/jspdf.min.js',
  
    'rgbcolor': 'node_modules/rgbcolor',
    'stackblur': 'node_modules/stackblur',
    'xmldom': 'node_modules/xmldom',
    'canvg-browser': 'node_modules/canvg-browser'

  };

  // packages tells the System loader how to load when no filename and/or no extension
  var packages = {
        'app': { main: 'main.js', defaultExtension: 'js' },
        'ng2-select': { main: 'ng2-select.js', defaultExtension: 'js' },
        'rxjs': { defaultExtension: 'js' },
        'angular2-highcharts': { main: 'index', format: 'cjs', defaultExtension: 'js' },
        'canvg-browser': { main: 'index.js', format: 'cjs', defaultExtension: 'js' },
        'rgbcolor': { main: 'index', format: 'cjs', defaultExtension: 'js' },
        'stackblur': { main: 'index', format: 'cjs', defaultExtension: 'js' },
        'xmldom': { main: 'dom',  defaultExtension: 'js' },
        'jspdf': { format: 'global', defaultExtension: 'js' }//,
     //   'es6-promise': { main: 'es6-promise.min.js', defaultExtension: 'js' }
  };
    
  var ngPackageNames = [
    'common',
    'compiler',
    'core',
    'forms',
    'http',
    'platform-browser',
    'platform-browser-dynamic',
    'router',
    'router-deprecated',
    'upgrade',
    'ng2-nvd3'
  ];

  // Individual files (~300 requests):
  function packIndex(pkgName) {
    packages['@angular/'+pkgName] = { main: 'index.js', defaultExtension: 'js' };
  }

  // Bundled (~40 requests):
  function packUmd(pkgName) {
    packages['@angular/'+pkgName] = { main: '/bundles/' + pkgName + '.umd.js', defaultExtension: 'js' };
  }

  // Most environments should use UMD; some (Karma) need the individual index files
  var setPackageConfig = System.packageWithIndex ? packIndex : packUmd;

  // Add package entries for angular packages
  ngPackageNames.forEach(setPackageConfig);

  // No umd for router yet
  packages['@angular/router'] = { main: 'index.js', defaultExtension: 'js' };

  var config = {
    map: map,
    packages: packages
  };

  System.config(config);

})(this);
