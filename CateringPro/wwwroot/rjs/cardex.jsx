

async function doСardFetch(apiurl) {

    try {
        var response = await fetch(apiurl);
        if (!response.status == 200)
            throw `Server responds with status ${response.status}`
        var json = await response.json();
        return json;
    }
    catch (ex) {
        console.log(ex);
    }
    
}


class NfcReaderCardScan extends React.Component {
    constructor(props) {
        super(props);
        this.emptystate = {cardtag: '', iscardscanned: false, userdata: undefined,isfetchinguser:false }
        this.state = this.emptystate 
        this.cardreaded = this.cardreaded.bind(this);
        this.fillcardetails = this.fillcardetails.bind(this);
    };
    async cardreaded(card) {
        this.setState({ iscardscanned: true, cardtag: card.cardtag, isfetchinguser: true })
        this.fillcardetails(card.cardtag);
    }
    async fillcardetails(token) {

        const data = await doСardFetch(`/Service/UserCardDetailsJson?token=${token}`)
        this.setState({ userdata: data, isfetchinguser: true})
    }
    componentDidMount() {
        this.props.childCallbacks({
            cardscanned: this.cardreaded
        });
    }
    render() {
        const { isconnected, isconnecting, url, onurlchange, commands } = this.props;
        const { userdata, iscardscanned, isfetchinguser} = this.state;

        const rendernouserrow = () => {
            return (
                <div className="row user-card  border border-danger rounded justify-content-center my-2">
                    <div className="col-12 justify-content-center bg-danger text-white">
                        <span>"NoRecords"</span>
                    </div>
                </div>
                )
        }
        const rendercardscannedinfo = () => {
            return (
                <>
                    <div className="row user-welcome bg-info text-white" >
                        <div className="col-4 card-info-scan">
                            <span className="card-info-scan" >Card Details</span>
                        </div>
                        <div className="col-8 card-info-scan ${addclass}">
                            <span className="card-info-scan" >cardtag</span>
                        </div>
                    </div>
                    <div id="user-card-details-temp" className="row user-card-details" >
                        <div className="col-4 card-info-scan">
                            <span className="card-info-scan" >..Fetchind user data</span>
                        </div>
                    </div>
                 </>
                )
        }
        return (
            <>
                <div className="row">
                    <div className="col-12">
                        <div id="card-info" className="container">
                        {iscardscanned && rendercardscannedinfo()}
                        </div>
                    </div>
                </div>
                {isfetchinguser && <FetchingUser/>}
            </>
        );

    }
}

class CardList extends React.Component {
    constructor(props) {
        super(props);
        this.dosearch = this.dosearch.bind(this);
        this.searchhandler = this.searchhandler.bind(this);
        this.textSearch = React.createRef();
        this.state = { data: [] };
    }
    async componentDidMount() {

      await  this.dosearch('');
    }
    async dosearch(criteria) {
        var url = `/Service/CardsListJson/?searchcriteria=${criteria}`;
        const data = await doСardFetch(url);
        if (data)
            this.setState({ data: data });
    }
    async searchhandler() {
        await this.dosearch(this.textSearch.current.value);
     
    }
    render() {
        const { prop,commands } = this.props;
        const renderNoRec = (dt) => {
            if (!dt || dt.length == 0) {
                return <div className="row user-card border rounded">
                    <div className="col-12 ">
                        NoRecords
                            </div>
                </div>
            }
        }
        return (
            <div className="container users-cards-list-content">
                    <div className="row user-card-search justify-content-end my-2">
                        <div className="col-6">
                            <div id="custom-search-input">
                            <div className="input-group">
                                <input ref={this.textSearch} type="text" id="search-val" className="form-control input-lg" placeholder="Search"
                                    onKeyPress={(e) => {
                                        console.log(e)
                                        if (e.code== "Enter")
                                            this.searchhandler()
                                    }} />
                                    <span className="input-group-btn">
                                    <button className="btn btn-info btn-lg search-card-btn" id="search-card-btn" type="button" onClick={this.searchhandler}>
                                            <span className="fa fa-search" aria-hidden="true"></span>
                                        </button>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                <div id="cardlist" className="row user-card bg-info text-white my-2">
                    <div className="col-2 user-card-picture">
                        <span>Фотографія</span>
                    </div>

                    <div className="col-2 user-card-name overflow-hidden">
                        <span>Ім’я та прізвище</span>
                    </div>
                    <div className="col-2 user-card-email overflow-hidden">
                        <span>E-mail</span>
                    </div>
                    <div className="col-2 user-card-name overflow-hidden">
                        <span>Дитина</span>
                    </div>
                    <div className="col-2 user-card-name overflow-hidden">
                        <span>Токен</span>
                    </div>
                    <div className="col-2 user-card-action overflow-hidden">
                        <span>Дія</span>
                    </div>

                </div>
                {
                    renderNoRec(this.state.data)
                }
                {this.state.data.map((item, idx) => {
                    return (
                        <CardRow commands={ commands} key={ idx} userentry={item} />
                    )

                })}
                </div>
             
        )
    }
}
class CardRow extends React.Component{
    constructor(props) {
        super(props);
        this.state = { data: [] };
    }
    render() {
        const { userentry,commands } = this.props;
        return (
            <div key={`${userentry.userId}`} className="row user-card border-bottom text-wrap text-break word-break align-middle" data-id={`${userentry.userId}`}>
                <div className="col-2 user-card-picture p-2">
                    <img src={`/Pictures/GetPicture/${userentry.pictureId}?width=80&height=80`} style={{ width: '80px', height: '80px' }}/>
                </div>
                <div className="col-2 user-card-name">
                    {userentry.userName}
                </div>
                <div className="col-2 user-card-email">
                    {userentry.userEmail}
                </div>
                <div className="col-2 user-card-name">
                    {userentry.userChildName}
                </div>
                <div className="col-2 user-card-name">
                    {userentry.cardToken}
                </div>
                <div className="col-2 user-card-action  align-middle">
                    <button className="burn-card btn btn-danger" data-id={`${userentry.userId}`} onClick={(e) => commands.burn(userentry.userId)} >Write</button>
                </div>
            </div>
        )
    }
}
class CardComp extends React.Component {
    constructor(props) {
        super(props);
        this.noop = () => { };
        this.noop = this.noop.bind(this);
        this.dialogstate = { isopen: false, title: '', message: '', type: 'none', yes_cb: this.noop, no_cb: this.noop  };
       
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
        this.burn_handler = this.burn_handler.bind(this);
        this.showdialog = this.showdialog.bind(this);
        this.showdialog_yes_no = this.showdialog_yes_no.bind(this);
        this.promise_dialog_yes_no = this.promise_dialog_yes_no.bind(this);
        this.setchildCallbacks = this.setchildCallbacks.bind(this);

        this.state = { url: "wss://localhost:44360/ws", nfcconnected: false, nfcconnecting:false }
        this.nfcsock = new nfc_socket(this.state.url);

        this.nfcsock.on('status', this.nfcsock_status);
        this.nfcsock.on('cardreaded', this.nfcsock_cardreaded);
        this.nfcsock.on('error', this.nfcsock_error);
        this.nfcsock.on('state', this.nfcsock_state);
        this.commands = { burn: this.burn_handler, connect: this.connect_handler, showdialog: this.showdialog }
        this.childCallbacks = {};

      
        //dialog.registerparent(this);
    }
    setchildCallbacks(callbacks) {
        this.childCallbacks = { ...this.childCallbacks, ...callbacks };  //to do change to arrays
    }
    //dialog handling
    async burn_handler(userid) {
        console.log(`burn call ${userid}`)
        //if (!this.nfcsock.isconnected()) {
        //    this.showdialog('error','NFC ще не підключено.','error');
        //    return;
        //}
        let dialogres = await this.promise_dialog_yes_no('Підтвердіть дію', 'В результаті цієї операції, попередня картка буде недійсна. ');
        if (dialogres != 'yes')
            return;
        const data = await doСardFetch(`/Service/GenUserCardToken?userId=${userid}`);
        if (!data)
            return;
        const writeresult = await this.nfcsock.writecard(json.cardTag);
        console.log("written");
    }
    showdialog(title, message,type) {
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
        const renderDialog =(dlg) => {
            if (dlg && dlg.isopen) {
                return (
                    <ModalDialog isopen={dlg.isopen} title={dlg.title} message={dlg.message} type={ dlg.type}  onclose={this.onclosedialog} onyes={this.onyesdialog} onno={this.onnodialog} />
                    )
            }
        }
        return (
            <div>
                <header className="main-header"><h1>Керування картками користувачів</h1> </header>
                <NfcReaderState commands={this.commands} isconnected={this.state.nfcconnected}
                    isconnecting={this.state.nfcconnecting}
                    url={this.state.url}
                    onurlchange={this.change_url_handler}
                />
                <NfcReaderCardScan childCallbacks={this.setchildCallbacks} commands={this.commands} isconnected={this.state.nfcconnected}
                    isconnecting={this.state.nfcconnecting}
                    url={this.state.url}
                />
                <CardList commands={this.commands} />
                {renderDialog(dialog)}
            </div>
        );

        
    }
}
ReactDOM.render(
    <CardComp />,
    document.getElementById("content")
);
