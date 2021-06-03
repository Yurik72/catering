class DialogBridge {
    constructor(comp) {
        this.component = {}
        this.noop = () => { };
        this.closehandle = this.noop
        this.yeshandle = this.noop
        this.nohandle = this.noop
    }
    registerparent(comp) {
        this.component = comp;
    }
    showdialog(title, message) {
        this.component.setdialog({ isopen: true, title: title, message: message })
    }
    resethandlers() {
        this.closehandle = this.noop
        this.yeshandle = this.noop
        this.nohandle = this.noop
    }
    callclose() {
        this.closehandle();
    }
    callyes() {
        this.yeshandle();
    }
    callno() {
        this.nohandle();
    }
}
var dialog = new DialogBridge();