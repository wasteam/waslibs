(function (win, doc) {
    doc.doctype = doc.implementation.createDocumentType("html", "", "");

    var viewport = doc.querySelector("meta[name=viewport]");
    if (viewport == null) {
        viewport = doc.createElement("meta");
        viewport.name = "viewport";
        doc.head.appendChild(viewport);
    }
    viewport.content = "width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no";

    var style = doc.createElement("style");
    doc.head.appendChild(style);

    var sheet = style.sheet;
    sheet.addRule("html, body", "margin: 0px; padding: 0px; background-color: transparent");
    sheet.addRule("body", "-ms-content-zooming: none; content-zooming: none; margin: 12px; font-family: 'Segoe WP'; font-size: 1em");
    sheet.addRule("div", "margin: 0 !important; padding: 0 !important; width: 100% !important");
    sheet.addRule("img", "display: block; margin: auto; max-width: 100%; height: auto");

    var htmlDoc = document.documentElement;

    doc.onscroll = function () {
        var rec = htmlDoc.getBoundingClientRect();
        win.external.notify(rec.left + '|' + rec.top + '|' + rec.width + '|' + rec.height);
    };

    win.onresize = function () {
        var rec = htmlDoc.getBoundingClientRect();
        win.external.notify(rec.left + '|' + rec.top + '|' + rec.width + '|' + rec.height);
    };
})(window, document);

var htmlDocument = document.documentElement;

function setHtmlDocumentMargin(margin) {
    htmlDocument.style.margin = margin;
}

function getHtmlDocumentRect() {
    var rec = htmlDocument.getBoundingClientRect();
    return rec.left + '|' + rec.top + '|' + rec.width + '|' + rec.height;
}

function setFontSize(size) {
    document.body.style.fontSize = size;
    window.external.notify(getHtmlDocumentRect());
}

function setHtmlColor(color) {
    htmlDocument.style.color = color;
}

function verticalScroll(y) {
    window.scroll(0, y);
}

function verticalScrollBy(offset) {
    window.scrollBy(0, offset);
}
