mergeInto(LibraryManager.library, {

  wsConnect: function (ip) {
    Connect(ip);
  },
  wsSend: function (str) {
    Send(str);
  },
  wsClose: function () {
    Close();
  },
});