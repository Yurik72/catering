

var glob_text_resources;

var text_resourcse =  {};

text_resourcse["ua"] = {
    confirm_delete:"",
    confirm_close:"Ви впевненi що хочете закрити вiкно ? Вашi змiни будуть втраченi"
}

glob_text_resources = text_resourcse["ua"];
function setLocale(locale) {
    glob_text_resources = text_resourcse[locale];
}
function get_text_res() { return glob_text_resources;}