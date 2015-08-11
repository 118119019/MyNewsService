

function goTop() {
    function a() {
        h = $(window).height();
        t = $(document).scrollTop();

        if (t > h) {
            $("#gotop").show();
        } else {
            $("#gotop").hide();
        }

    }
    $("#gotop").click(function () {
        $(document).scrollTop(0);
    });
    $(window).scroll(function (b) {
        a();
    });
}

$(function () {
    goTop();



});



var StringBuilder = function (a) {
    this.s = new Array(a);
    this.onMultiAppendBeforeHandle = null;
    this.onMultiAppendBefore = function (b) {
        this.onMultiAppendBeforeHandle = b;
        return this;
    };
    this.append =
        function (b) {
            this.s.push(b);
            return this;
        };
    this.toString = function () {
        return this.s.join("");
    };
    this.clear =
        function () {
            this.s = new Array();
        };
    this.appendMultiFormat =
        function (c, d) {
            if (typeof (d) == "object") {
                for (var b in d) {
                    if (this.onMultiAppendBeforeHandle != null) {
                        this.onMultiAppendBeforeHandle(d[b]);
                    } this.appendFormat(c, d[b]);
                }
            } return this;
        };
    this.appendFormat = function () {
        var p = arguments.length;
        if (p == 0) { return this; }
        var l = arguments[0];
        if (p == 1) { return this.append(l); }
        var b = arguments[1];
        if (b == null) { b = ""; }
        var g, m, o, d, j; if (typeof (b) == "object") {
            j = function (c, e) { return c[1][e]; };
        } else { j = function (c, e) { return c[e - 0 + 1]; }; }
        for (g = 0; g < l.length;) {
            o = l.charAt(g);
            if (o == "{") { m = l.indexOf("}", g); d = l.substring(g + 1, m); this.s.push(j(arguments, d)); g = m + 1; continue; } this.s.push(o); g++;
        } return this;
    };
};

