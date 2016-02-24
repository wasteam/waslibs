(function (win, doc) {
    doc.doctype = doc.implementation.createDocumentType("html", "", "");

    var viewport = doc.querySelector("meta[name=viewport]");
    if (viewport == null) {
        viewport = doc.createElement("meta");
        viewport.name = "viewport";
        doc.head.appendChild(viewport);
    }
    viewport.content = "width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no";

    this.style = doc.createElement("style");
    doc.head.appendChild(style);

    var htmlDoc = document.documentElement;

    win.onload = function () {
        var rec = htmlDoc.getBoundingClientRect();
        win.external.notify("L" + rec.left + '|' + rec.top + '|' + rec.width + '|' + rec.height);
    };

    win.onresize = function () {
        var rec = htmlDoc.getBoundingClientRect();
        win.external.notify("R" + rec.left + '|' + rec.top + '|' + rec.width + '|' + rec.height);
    };

    doc.onscroll = function () {
        var rec = htmlDoc.getBoundingClientRect();
        win.external.notify("S" + rec.left + '|' + rec.top + '|' + rec.width + '|' + rec.height);
    };

    var bottom = doc.createElement("div");
    doc.body.appendChild(bottom);
    this.refreshLayout = function () {
        bottom.style.display = "none";
        setTimeout(function () { bottom.style.display = "block"; }, 100);
    };

    win.htmlViewer = this;

})(window, document);

var htmlDocument = document.documentElement;

function setHtmlDocumentMargin(margin) {
    htmlDocument.style.margin = margin;
    htmlViewer.refreshLayout();
    return getHtmlDocumentRect();
}

function getHtmlDocumentRect() {
    var rec = htmlDocument.getBoundingClientRect();
    return rec.left + '|' + rec.top + '|' + rec.width + '|' + rec.height;
}

function setFontSize(size) {
    document.body.style.fontSize = size;
    window.external.notify("R" + getHtmlDocumentRect());
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

function setHtmlStyle(mxW, mL, mR) {
    var maxWidth = "max-width: " + mxW + ";";
    var margin = "margin-left: " + mL + "; margin-right: " + mR + ";";

    document.head.removeChild(htmlViewer.style);
    htmlViewer.style = document.createElement("style");
    document.head.appendChild(htmlViewer.style);
    var sheet = style.sheet;

    sheet.addRule("html, body, div, p", "margin: 0px; padding: 0px");
    sheet.addRule("html, body", "background-color: transparent !important; color: inherit");
    sheet.addRule("body", "-ms-content-zooming: none; content-zooming: none");
    sheet.addRule("body", "display: block; margin: 16px; padding: 0px 0px; width: 90%;" + margin);
    sheet.addRule("body", "font-family: 'Segoe WP'; font-size: 1em; text-align: left");
    sheet.addRule("p, h1, h2, h3, h4, h5, h6, a", "background-color: transparent !important; color: inherit !important; font-family: inherit !important; font-weight: normal !important");
    sheet.addRule("div", "margin: 0px; padding: 4px 0px 4px 0px; width: 100% !important; min-height: unset !important");
    sheet.addRule("p", "padding: 6px 0px 8px 0px; font-size: inherit !important; line-height: inherit !important");
    sheet.addRule("a", "font-size: inherit !important; font-weight: bold !important; text-decoration: underline !important");
    sheet.addRule("img", "display: block; max-width: 100%; height: auto;" + margin);
    sheet.addRule("iframe", "display: block; max-width: 100%;" + margin);
    sheet.addRule("video", "display: block; max-width: 100%;" + margin);
}
