mergeInto(LibraryManager.library, {

  wsConnect: function (ip) {
    Connect(Pointer_stringify(ip));
  },
  wsSend: function (str) {
    Send(Pointer_stringify(str));
  },
  wsClose: function () {
    Close();
  },
});