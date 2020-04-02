function validateEmail(email) {
    var atpos = email.indexOf('@');
    var dotpos = email.lastIndexOf(".");
    if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= email.length) {
        return false;
    }
    else {
        return true;
    }
}