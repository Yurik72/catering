class ModalDialog extends React.Component {
    constructor(props) {
        super(props);
    };
    render() {
        const { isopen, title, message, onclose, onyes, onno, type } = this.props;
        const renderbuttons = () => {
            if (type == 'yesno') {
                return (
                    <>
                        <button id="btnyes" type="button" className="btn btn-primary" onClick={onyes}>Tak</button>
                        <button id="btno" type="button" className="btn btn-primary" onClick={onno}>Ні</button>
                    </>
                )
            } else {
                return (
                    <button id="btnyes" type="button" className="btn btn-primary" onClick={onclose}>Tak</button>
                )
            }
        }
        return (
            <div className={"modal " + (isopen ? "show" : "fade")} id="ModalPopUp" role="dialog" aria-modal="true" style={isopen ? { display: "block" } : {}}>
                <div className="modal-dialog err-pop" >
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">{title}</h5>
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close" onClick={onclose}>
                                <span aria-hidden="true">X</span>
                            </button>
                        </div>
                        <div className="modal-body">
                            <p>{message}</p>
                        </div>
                        <div className="modal-footer">
                            {
                                renderbuttons()
                            }

                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
const FetchingUser = (props) => {

    return (
        <div id="user-card-details-temp" className="row user-card-details" >
            <div className="col-4 card-info-scan">
                <span className="card-info-scan" >..Fetchind user data</span>
            </div>
        </div>
    );
}