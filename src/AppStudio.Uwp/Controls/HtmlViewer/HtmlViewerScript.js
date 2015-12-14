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

    doc.onscroll = function () {
        var rec = htmlDoc.getBoundingClientRect();
        win.external.notify(rec.left + '|' + rec.top + '|' + rec.width + '|' + rec.height);
    };

    // TODO: Review
    //win.onresize = function () {
    //    var rec = htmlDoc.getBoundingClientRect();
    //    win.external.notify(rec.left + '|' + rec.top + '|' + rec.width + '|' + rec.height);
    //};

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
}

function getHtmlDocumentRect() {
    var rec = htmlDocument.getBoundingClientRect();
    return rec.left + '|' + rec.top + '|' + rec.width + '|' + rec.height;
}

function getContentHeight() {
    return htmlDocument.getBoundingClientRect().height + "";
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

function setHtmlStyle(mxW, mL, mR) {
    var maxWidth = "max-width: " + mxW + ";";
    var marginL = "margin-left: " + mL + ";";
    var marginR = "margin-right: " + mR + ";";

    document.head.removeChild(htmlViewer.style);
    htmlViewer.style = document.createElement("style");
    document.head.appendChild(htmlViewer.style);
    var sheet = style.sheet;

    sheet.addRule("html, body", "margin: 0px; padding: 0px; background-color: transparent !important; color: inherit");
    sheet.addRule("body", "-ms-content-zooming: none; content-zooming: none");
    sheet.addRule("body", "display: block; margin: 12px 24px 36px 12px; width: 90%;" + maxWidth + marginL + marginR);
    sheet.addRule("body", "font-family: 'Segoe WP'; font-size: 1em; text-align: left");
    sheet.addRule("p, h1, h2, h3, h4, h5, h6, a", "background-color: transparent !important; color: inherit !important; font-family: inherit !important; font-weight: normal !important");
    sheet.addRule("p", "font-size: inherit !important; line-height: inherit !important");
    sheet.addRule("a", "font-size: inherit !important; font-weight: bold !important; text-decoration: underline !important");
    sheet.addRule("img", "display: block; max-width: 100%; height: auto;" + marginL + marginR);
    sheet.addRule("iframe", "display: block; max-width: 100%;" + marginL + marginR);
    sheet.addRule("video", "display: block; max-width: 100%;" + marginL + marginR);
    sheet.addRule("div", "width: 100% !important; min-height: unset");
}
