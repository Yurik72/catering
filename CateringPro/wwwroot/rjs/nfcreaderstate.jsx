class NfcReaderState extends React.Component {
    constructor(props) {
        super(props);
        //this.state = { url: "wss://localhost:44360/ws", connected: false };

    };
    render() {
        const { isconnected, isconnecting, url, onurlchange, commands } = this.props;
        return (
            <div id="nfc_state" className={"row container-state text-white " + (isconnected ? "bg-success" : "bg-danger")} >
                <div className="col-4 container-state">
                    {!isconnected &&
                        <span id="nfc_isoffline">
                            NFC зчитувач вимкнено
                        </span>
                    }
                    {isconnected &&
                        <span class="service-status online">
                            <span id="nfc_readername"></span>
                            NFC Reader Online
                        </span>
                    }
                    <span id="error-offline" className="service-status error">
                    </span>
                </div>
                <div className="col-4 container-state">
                    <div className="input-group mb-3">
                        <div className="input-group-prepend">
                            <span className="input-group-text">URL</span>
                        </div>
                        <input id="nfcurl" className="form-control" disabled={isconnecting} value={url} onChange={(e) => onurlchange(e.target.value)} />
                    </div>
                </div>
                <div className="col-4 container-state">
                    <button id="connectnfc" disabled={isconnecting} className="btn btn-info" onClick={(e) => commands.connect()}>Підключити NFC</button>
                </div>
            </div>
        );

    }
}