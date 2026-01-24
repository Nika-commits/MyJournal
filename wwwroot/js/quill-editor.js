let quill;

window.initQuill = (editorId) => {
    quill = new Quill(`#${editorId}`, {
        theme: 'snow',
        modules: {
            toolbar: [
                ["bold", "italic", "underline", "strike"],
                [{ 'header': 1 }, { 'header': 2 }],
                [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                ["clean"]
            ]
        }
    });
};

window.getQuillHtml = () => {
    return quill.root.innerHTML;
};

window.setQuillHtml = (html) => {
    quill.root.innerHTML = html;
}