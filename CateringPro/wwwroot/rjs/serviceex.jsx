
async function doServiceFetch(apiurl,options) {

    try {
        var response = await fetch(apiurl,options);
        if (!response.status == 200)
            throw `Server responds with status ${response.status}`
        var json = await response.json();
        return json;
    }
    catch (ex) {
        console.log(ex);
    }

}
class DishPlateMenuItem extends React.Component {
    constructor(props) {
        super(props);
        this.state = { isselected: false }
        this.isselected = this.isselected.bind(this);
       
    }
    isselected() {
        return this.state.isselectecd;
    }
    render() {
        const { idx,onselected } = this.props;
        const { isselected } = this.state
        return (
           
            <div data-num={idx}
                className={"order-dish-select desk-non-bord col-6  d-flex justify-content-center align-items-center plate-button" + (isselected ? " active active-plate-button" : "")}
                onClick={(e) => {
                    e.preventDefault()
                    this.setState({ isselected: !isselected })
                    if (onselected)
                        onselected(idx, !isselected)
                }}
                style={{ minheight: '52px' }}>
                <SvgPlate/>

                 <span className="plate-text ml-2"
                        style={{ fontsize: '14px', fontweight: '400' }}><span style={{ fontsize: '18px' }}> { idx}</span> Dish</span>
                </div>

            
        )
    }
}
class DishPlateMenuSelectCategories extends React.Component {
    constructor(props) {
        super(props);
        this.state = {  isselected: false, data: [] }
      
        this.fillCategories = this.fillCategories.bind(this);
        this.onItemSelected = this.onItemSelected.bind(this);
        this.isselected = this.isselected.bind(this);
        this.selectedids = [];

    }


    onItemSelected(id) {
        if (this.isselected(id)) {
            this.selectedids.splice(this.selectedids.indexOf(id), 1)
        } else {
            this.selectedids.push(id);
        }
        if (this.props.onchanged) {
            this.props.onchanged(this.selectedids)
        }
    }
    isselected(id) {
        return this.selectedids.indexOf(id) >= 0;
    }
    async fillCategories() {
        var url = '/Service/GetAvailableCategories';
        const data = await doServiceFetch(url, {method:'POST'});
        if (data)
            this.setState({ data: data });
    }
    async componentDidMount() {

        await this.fillCategories();
    }

    render() {
        const { idx, onselected } = this.props;
        const { data } = this.state

        return (

            <ul className="form-control col-6  pl-3 list-group" id="complexcategories" name="complexcategories">
                {data.map((item, idx) => {
                    const issel = this.isselected(item.id);
                    return (
                        <li className="role-item list-group-item"
                            onClick={(e) => {
                                e.preventDefault()
                                this.onItemSelected(item.id)
                            }}
                            style={{ cursor: 'pointer' }} value={item.id} > { item.value}</li>
                    )

                })}
            </ul>


        )
    }
}
class DishPlateMenuNonComplex extends React.Component {
    constructor(props) {
        super(props);
        this.state = { isselected: false }
        this.isselected = this.isselected.bind(this);

    }
    isselected() {
        return this.state.isselectecd;
    }
    render() {
        const { idx, onselected } = this.props;
        const { isselected } = this.state
        return (
            <div className="col-12 col-lg-4 col-md-4 p-0 d-flex justify-content-between ">
                <div className={"order-dish-select notComplex desk-non-bord col-6  d-flex justify-content-center align-items-center plate-button " + (isselected ? " active active-plate-button" : "")}
                    onClick={(e) => {
                        e.preventDefault()
                        this.setState({ isselected: !isselected })
                        if (onselected)
                            onselected(!isselected)
                    }}
                    style={{minheight: '52px'}}>
 

                    <span className="plate-text ml-2"
                        style={{ fontsize: '14px', fontweight: '400' }}><span style={{ fontsize: '18px' }}></span> Not Complex</span>
                </div>

            </div>




        )
    }
}
class DishPlateMenuWelcome extends React.Component {
    constructor(props) {
        super(props);
        this.state = { isselected: false }
        this.isselected = this.isselected.bind(this);

    }
    isselected() {
        return this.state.isselectecd;
    }
    render() {
        const { idx, onselected } = this.props;
        const { isselected } = this.state
        return (

            <div className={"desk-non-bord order-dish-select welcome col-12 col-md-2 col-lg-2 " + (isselected ? " active active-plate-button" : "")}
                onClick={(e) => {
                    e.preventDefault()
                    this.setState({ isselected: !isselected })
                    if (onselected)
                        onselected(!isselected)
                }}
                id="say-hello-button" style={{ minheight: '52px' }}>
                <SvgWelcome />
                <span className="stroke-svg ml-2"
                    style={{ texttransform: 'uppercase', fontsize: '14px', fontweight: 'bold' }}> Welcome</span>
            </div>


        )
    }
}
class ServiceMenu extends React.Component {
    constructor(props) {
        super(props);
        this.numdishes = 10;
        

        this.dishesselect = [];
        this.categories = [];
        for (var i = 0; i < this.numdishes; i++) {
            this.dishesselect.push(false);
        }
        this.selection = { iswelcome: false, iscomplex: false, dishnum: this.dishesselect, categories: this.categories }
        this.ondishselect = this.ondishselect.bind(this);
    }
    ondishselect(idx, value) {
        this.dishesselect[idx] = value;
    }
    render() {
        const { commands } = this.props;
        const rendersingledishesnum = (idx) => {
            return (
                <DishPlateMenuItem key={idx} idx={idx} onselected={this.ondishselect } />
            )
        }
        const rendergroupdishes = (idx) => {
            return (
                <div key={'g' + idx} className="col-12 col-lg-4 col-md-4 p-0 d-flex justify-content-between " >
                    {rendersingledishesnum(idx)}
                    {rendersingledishesnum(idx+1)}
                </div >
            )
        }
        const renderdishesnum = () => {
            var dishes = [];
          
            for (var i = 0; i < this.numdishes; i+=2 ){
                dishes.push(rendergroupdishes(i))
            }
            return dishes;
        }
        return (
            <div className="col-12 p-0 d-flex flex-row flex-wrap my-1 dishes-num">
                <div className="form-group  d-flex  p-0 p-lg-2 p-md-2 flex-column  col-12 col-md-2 col-lg-2 input-row-black  align-content-center desk-non-bord my-2 my-lg-0 my-md-0"
                    style={{ padding: '7px', maxheight: '52px'}}>
                    <label htmlFor="select-type-dish"
                        className="col-12  p-0 align-items-center mb-0 non-display "
                        style={{ fontsize: '14px', fontweight: 'bold', color: '#232323'}}>Обрати меню:</label>
                               

                </div>
                <DishPlateMenuWelcome onselected={(val) => this.selection.iswelcome=val} />
                {renderdishesnum()}
                <DishPlateMenuNonComplex onselected={(val) => this.selection.iscomplex = val}/>
                <div className="col-12 p-0 my-2 delivery-command" id="block-buttons-start" style={{ transition: '0.3s ease-in -out' }}>
                    {/*
                    <button type="button" className="btn btn-secondary my-2  col-md-4 col-lg-3 col-xl-3 col-12 start-button"
                        id="btnstart"
                        onClick={commands.starthandler}
                        style={{border: '1px solid #36C233', minheight: '48px',  texttransform: 'uppercase' , boxsizing: 'border-box'}}>
                        Почати
                    </button>
                    <button id="btnstop" type="button" className="btn btn-secondary my-2 col-md-4 col-lg-3 col-xl-3 col-12"
                        onClick={commands.stophandler}
                        style={{background: '#ffffff', border: '1px solid #F15E25', minheight: '48px', color: '#f15e25', texttransform: 'uppercase', boxsizing: 'border-box'}}>
                        припинити
                    </button>
                    <ul className="form-control col-6  pl-3 list-group" id="complexcategories" name="complexcategories">
                    </ul>
                    */}
                    <DishPlateMenuSelectCategories onchanged={(ids) => this.selection.categories=ids} />
                </div>
                
            </div>
            )
    }
}

class ServiceComp extends React.Component {
    constructor(props) {
        super(props);
        this.noop = () => { };
        this.noop = this.noop.bind(this);
        this.dialogstate = { isopen: false, title: '', message: '', type: 'none', yes_cb: this.noop, no_cb: this.noop };

        this.state = { dialog: this.dialogstate };
        this.change_url_handler = this.change_url_handler.bind(this);
        this.connect_handler = this.connect_handler.bind(this);

        this.nfcsock_status = this.nfcsock_status.bind(this);
        this.nfcsock_cardreaded = this.nfcsock_cardreaded.bind(this);
        this.nfcsock_error = this.nfcsock_error.bind(this);
        this.nfcsock_state = this.nfcsock_state.bind(this);
        this.setdialog = this.setdialog.bind(this);
        this.hidedialog = this.hidedialog.bind(this);
        this.onclosedialog = this.onclosedialog.bind(this);
        this.onyesdialog = this.onyesdialog.bind(this);
        this.onnodialog = this.onnodialog.bind(this);
        this.resetdialog = this.resetdialog.bind(this);
        
        this.showdialog = this.showdialog.bind(this);
        this.showdialog_yes_no = this.showdialog_yes_no.bind(this);
        this.promise_dialog_yes_no = this.promise_dialog_yes_no.bind(this);
        this.setchildCallbacks = this.setchildCallbacks.bind(this);
        this.starthandler = this.starthandler.bind(this);
        this.stophandler = this.stophandler.bind(this);
        this.stopstarthandler = this.stopstarthandler.bind(this);
        this.isstarted = this.isstarted.bind(this);
        this.iswelcome = this.iswelcome.bind(this);

        this.state = { url: "wss://localhost:44360/ws", nfcconnected: false, nfcconnecting: false ,isstarted:false,iswelcome:false}
        this.nfcsock = new nfc_socket(this.state.url);

        this.nfcsock.on('status', this.nfcsock_status);
        this.nfcsock.on('cardreaded', this.nfcsock_cardreaded);
        this.nfcsock.on('error', this.nfcsock_error);
        this.nfcsock.on('state', this.nfcsock_state);
        this.commands = {  connect: this.connect_handler, showdialog: this.showdialog,start:this.starthandler,stop:this.stophandler }
        


        //dialog.registerparent(this);
    }
    stopstarthandler() {
        if (this.isstarted()) {
            this.stophandler()
        } else {
            this.starthandler()
        }
    }
    isstarted() {
        return this.state.isstarted;
    }
    iswelcome() {
        return this.state.iswelcome;
    }
    starthandler() {
        for (var child in this.props.children) {
            
            console.log(child);
            
        }
        this.setState({ isstarted: true })
    }
    stophandler() {
        this.setState({ isstarted: false })
    }
    setchildCallbacks(callbacks) {
        this.childCallbacks = { ...this.childCallbacks, ...callbacks };  //to do change to arrays
    }

    showdialog(title, message, type) {
        this.setdialog({ isopen: true, title: title, message: message })
    }
    async promise_dialog_yes_no(title, message) {
        try {
            var promisedlg = new Promise((resolve, reject) => {
                this.showdialog_yes_no(title, message, resolve, reject)
            });
            var res = await promisedlg;
            return res;
        }
        catch (ex) {
            return ''
        }
    }
    showdialog_yes_no(title, message, yesCallback, noCallback) {
        this.setdialog({ isopen: true, title: title, message: message, type: 'yesno', yes_cb: yesCallback, no_cb: noCallback })
    }
    setdialog(dlg) {
        this.setState({ dialog: dlg })
    }
    resetdialog() {
        //dialog.resethandlers()
    }
    hidedialog() {
        this.setdialog(this.dialogstate)
        this.resetdialog();
    }
    onclosedialog(e) {
        e.preventDefault();
        const { yes_cb, no_cb } = this.state.dialog;
        //probably we are waiting promise
        if (no_cb)
            no_cb("no");
        if (yes_cb)
            yes_cb("no");
        //dialog.callclose();
        this.hidedialog();
    }
    onyesdialog(e) {
        e.preventDefault();
        //this is a promise
        const { yes_cb } = this.state.dialog;
        if (yes_cb)
            yes_cb("yes");
        this.hidedialog();
    }
    onnodialog(e) {
        e.preventDefault();
        const { yes_cb, no_cb } = this.state.dialog;
        //to call no will raise error
        // if (yes_cb)
        //     yes_cb("no");
        if (no_cb)
            no_cb("no");
        this.hidedialog();
    }
    nfcsock_status(msg) {
        console.log(msg)
        this.setState({ nfcconnected: this.nfcsock.isconnected(), nfcconnecting: this.nfcsock.isconnecting() })
    }
    nfcsock_cardreaded(msg) {
        if (this.cardreaded && this.childCallbacks.cardreaded)
            this.childCallbacks.cardreaded(msg);
    }
    nfcsock_error(msg) {
        console.log("nfcsock_error")
        console.log(msg)
        this.showdialog("Connection Error", msg)
        //dialog.showdialog("Connection Error",msg)
    }
    nfcsock_state(msg) {
        console.log(msg)
        this.setState({ nfcconnected: this.nfcsock.isconnected(), nfcconnecting: this.nfcsock.isconnecting() })
    }
    change_url_handler(val) {
        this.setState({ url: val })
    }
    connect_handler() {
        this.nfcsock.connect();
        console.log("Connect pressed")
    }
    render() {
        const compprops = {};
        const { dialog } = this.state;
        const renderDialog = (dlg) => {
            if (dlg && dlg.isopen) {
                return (
                    <ModalDialog isopen={dlg.isopen} title={dlg.title} message={dlg.message} type={dlg.type} onclose={this.onclosedialog} onyes={this.onyesdialog} onno={this.onnodialog} />
                )
            }
        }
        return (
            <>
                <div className="left-fixed-title-btn title-btn-mob-dsk toogles" onClick={ this.stopstarthandler}>
                    Start/Stop
                 </div>
                <div className="right-fixed-title-btn title-btn-mob-dsk toogles-right">
                    History
                </div>
                <div className="row mr-0 ml-0 flex-grow-1">
                    <div className="container-xl container container-md container-sm container-lg p-md-0 p-lg-0">
                        <div className="col-12 align-items-center pb-3  ml-xl-0 ml-md-0 ml-sm-0 p-0 ml-0 mt-2">
                            <NfcReaderState commands={this.commands} isconnected={this.state.nfcconnected}
                                isconnecting={this.state.nfcconnecting}
                                url={this.state.url}
                                onurlchange={this.change_url_handler}
                            />
                            <ServiceMenu commands={this.commands}/>

                        </div>
                    </div>
                 </div>
                {renderDialog(dialog)}
            </>
        );


    }
}
ReactDOM.render(
    <ServiceComp />,
    document.getElementById("content")
);