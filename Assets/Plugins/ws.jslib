mergeInto(LibraryManager.library, {
    wsConnect: function ( url ) {
        function emit ( method, data ) { 
            unityInstance.SendMessage( 'Setup', method, data ); 
        };
    
        var ws = new WebSocket( Pointer_stringify(url) );
        window.WebSocketClient = ws;

        ws.onopen = function ( ) { 
            emit('onOpen'); 
        };

        ws.onclose = function ( ) { 
            emit('onClose'); 
        };

        ws.onmessage = function ( event ) { 
            emit('onMessage', event.data); 
        };

        ws.onerror = function ( event ) { 
            emit('onError', event.data); 
        };
    },

    wsSend: function ( data ) {
        window.WebSocketClient.send( Pointer_stringify(data) );
    },

    wsClose: function ( ) {
        window.WebSocketClient.close();
    },
});