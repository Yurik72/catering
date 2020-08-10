﻿

function nfc_socket(url, callbackdata,callbackstatechange) {
    this.options = {};
    this.options.url = url;
    this.options.callbackdata = callbackdata;
    this.options.callbackstatechange = callbackstatechange;
    this.events = new function () {
        var _triggers = {};

        this.on = function (event, callback) {
            if (!_triggers[event])
                _triggers[event] = [];
            _triggers[event].push(callback);
        }

        this.triggerHandler = function (event, params) {
            if (_triggers[event]) {
                for (i in _triggers[event])
                    _triggers[event][i](params);
            }
        }
    };
}
nfc_socket.prototype.on = function (event, callback) {
    this.events.on(event, callback);
}
nfc_socket.prototype.connect=function (){
    this.socket = new WebSocket(this.options.url);
    var self = this;
    function processmessage(data) {
        var msg;
        try {
            msg = JSON.parse(data);
            self.events.triggerHandler(msg.Responsetype, msg);
        }
        catch{
            self.events.triggerHandler("ontext",data);
        }
        
    }
    var stateupdate = function (cmd, event) {
        if (self.options.callbackstatechange)
            self.options.callbackstatechange(cmd, event);
    }
    this.socket.onopen = function (event) {
        stateupdate('open',event);
    };
    this.socket.onclose = function (event) {
        stateupdate('close',event);
    };
    this.socket.onerror = function (event) {
        stateupdate('error', event);
    };
    this.socket.onmessage = function (event) {
        if (self.options.callbackdata)
            self.options.callbackdata(event.data);
        processmessage(event.data);
    };


};

nfc_socket.prototype.getstatus = function (cb) {

    if (!cb) {
        cb = function (status) { };
    }
    var status = { isconected: false, readername: '', ismonitoring: false, socketstate: WebSocket.CLOSED };
    if (!this.socket) {
        cb(status);
        return;
    }
    if (this.socket.readyState != WebSocket.OPEN) {
        status.socketstate = this.socket.readyState;
        cb(status);
        return;
    } 
    var request = { command: "status" };
    this.socket.send(JSON.stringify(request));

};
nfc_socket.prototype.writecard = function (tag) {

    if (!this.socket || this.socket.readyState != WebSocket.OPEN) {
        return false;
    }
    var request = { Command: "writecard", CardTag: tag };
    var cmd = { isconected: false, readername: '', ismonitoring: false, socketstate: WebSocket.CLOSED };


    this.socket.send(JSON.stringify(request));

};
nfc_socket.prototype.close=function (){
    if (!this.socket || this.socket.readyState !== WebSocket.OPEN) {
        console.log("closing not connected socket");
    }
    this.socket.close(1000, "Closing from client");
}