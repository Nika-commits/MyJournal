//let quill;

window.initQuill = (editorId) => {

    if (window.ImageResize) {
        var ImageResizeModule = window.ImageResize.default || window.ImageResize;
        Quill.register('modules/imageResize', ImageResizeModule);
    }
    else {
        console.error("Image Resize not loaded");
    }

    quill = new Quill(`#${editorId}`, {
        theme: 'snow',
        modules: {
            imageResize: {
                displaySize: true,
                modules: ['Resize', 'DisplaySize', 'Toolbar']
            },
            toolbar: [
                ["bold", "italic", "underline", "strike"],
                [{ 'header': 1 }, { 'header': 2 }],
                [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                ['image', 'link'],
                ["clean"]
            ]
        }
    });
};

window.getQuillHtml = () => {
    return window.quill.root.innerHTML;
};

window.setQuillHtml = (content) => {
    if (window.quill) {
        window.quill.setContents([]);
        if (content) {
            window.quill.clipboard.dangerouslyPasteHTML(content);

        }
    }

}