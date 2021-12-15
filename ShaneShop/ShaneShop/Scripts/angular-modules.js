// By Luis Perez
// From blog post: http://www.simplygoodcode.com/2014/04/angularjs-getting-around-ngapp-limitations-with-ngmodule/
// Lina update: 2017-11-15

(function () {
    'use strict';

    function appendElement(vList, vElement) {
        vElement && vList.push(vElement);
    }

    function appendElements(vList, vDocument, vName) {
        var elements2 = vDocument.querySelectorAll(vName);
        for (var j = 0; j < elements2.length; j++) {
            appendElement(vList, elements2[j]);
        }
    }

    function initNgModules(vDocument) {
        var elements = [vDocument],
            moduleElements = [],
            modules = [],
            names = ['ng:module', 'ng-module', 'x-ng-module', 'data-ng-module', 'ng:modules', 'ng-modules', 'x-ng-modules', 'data-ng-modules'],
            NG_MODULE_CLASS_REGEXP = /\sng[:\-]module[s](:\s*([\w\d_]+);?)?\s/;

        for (var i = 0; i < names.length; i++) {
            var name = names[i];
            var name2 = name.replace(':', '\\:');

            appendElement(elements, document.getElementById(name));

            if (vDocument.querySelectorAll) {
                appendElements(elements, vDocument, '.' + name2);
                appendElements(elements, vDocument, '.' + name2 + '\\:');
                appendElements(elements, vDocument, '[' + name2 + ']');
            }
        }

        for (var m = 0; m < elements.length; m++) {
            var ele = elements[m];

            var className = ' ' + ele.className + ' ';
            var match = NG_MODULE_CLASS_REGEXP.exec(className);

            if (match) {
                moduleElements.push(ele);
                modules.push((match[2] || '').replace(/\s+/g, ','));
            } else {
                if (ele.attributes) {
                    for (var n = 0; n < ele.attributes.length; n++) {
                        var attr = ele.attributes[n];

                        if (names.indexOf(attr.name) !== -1) {
                            moduleElements.push(ele);
                            modules.push(attr.value);
                        }
                    }
                }
            }
        }

        for (var o = 0; o < moduleElements.length; o++) {
            var moduleElement = moduleElements[o];
            var moduleName = modules[o].replace(/ /g, '').split(",");
            angular.bootstrap(moduleElement, moduleName);
        }
    }

    angular.element(document).ready(function () {
        initNgModules(document);
    });
})();