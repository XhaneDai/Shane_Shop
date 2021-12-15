(function (global, factory) {
    "use strict";

    if (typeof module === "object" && typeof module.exports === "object") {
        module.exports = global.document ?
            factory(global, true) :
            function (w) {
                if (!w.document) {
                    throw new Error("commin requires a window with a document");
                }
                return factory(w);
            };
    } else {
        factory(global);
    }
})(typeof window !== "undefined" ? window : this, function () {
    "use strict";

    // ******************** JS Extension ******************** //

    Array.prototype.insert = function (index, items) {
        this.splice.apply(this, [index, 0].concat(items));
    };

    String.prototype.padLeft = function (l, c) { return Array(l - this.length + 1).join(c || " ") + this; };

    var
        version = "1.0.0",
        common = function (selector, context) {
            return new common.fn.init(selector, context);
        };

    common.fn = common.prototype = {
        version: version,
        constructor: common,
    };

    common.fn.init = function (selector, context, root) {
        var match, elem;

        if (!selector) {
            return this;
        }
        else {
            return this;
        }
    };

    // ******************** Log ******************** //

    common.log = function (val) {
        if (typeof val === "string") {
            console.log(" =========== " + val + " =========== ");
        }
        else {
            console.log(val);
        }
    };

    // ******************** bootstrap common alert ******************** //

    var getAlertMsgModel = function (data) {
        if (typeof data === "string") {
            return { error: false, message: data, noTitle: true, noAction: true };
        }
        else if (!data.message) {
            return { error: true };
        }
        else {
            var result = data;
            result.error = false;
            result.noTitle = !data.title;
            result.noAction = !data.action;
            return result;
        }
    }

    var _HasShow = false;
    common.Alert = function (data) {
        //console.log("common.Alert");
        if (!data) return;

        var model = getAlertMsgModel(data);
        var ele = $("#AlertModal");
        var noEle = !ele || !ele.length;

        if (model.error) {
            console.log("data is error");
            return;
        }

        if (noEle) {
            alert(model.message);
            if (!model.noAction) {
                model.action();
            }
        }
        else {
            _HasShow = true;

            ele.find('.modal-body').html(model.message);
            ele.modal({ backdrop: 'static', keyboard: true });

            if (!model.noTitle) {
                ele.find('.modal-title').html(model.title);
            }

            if (!model.noAction) {
                $('#AlertModal').on('hidden.bs.modal', function (e) {
                    if (_HasShow) {
                        _HasShow = false;
                        console.log("common.Alert _# before action");
                        model.action();
                        console.log("common.Alert _# after action");
                    }
                });
            }
        }
    };
    
    // ******************** End ******************** //
    window.common = common;

    return common;
});