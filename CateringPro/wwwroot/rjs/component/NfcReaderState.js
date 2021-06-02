
class NfcReaderState extends React.Component {
    render() {
        const compprops = {};
        return (
            <div id="nfc_isoffline" className="row container-state bg-danger text-white">
                <div className="col-4 container-state">
                    <span id="nfc_isoffline">
                        NFC зчитувач вимкнено
                    </span>
                    <span id="error-offline" className="service-status error">
                    </span>
                </div>
                <div className="col-4 container-state">
                    <div className="input-group mb-3">
                        <div className="input-group-prepend">
                            <span className="input-group-text">URL</span>
                        </div>
                        <input id="nfcurl" className="form-control" value="wss://localhost:44360/ws" />
                    </div>
                </div>
                <div className="col-4 container-state">
                    <button id="connectnfc" className="btn btn-info">Підключити NFC</button>
                </div>
            </div>
        );

    }
}

export default NfcReaderState;