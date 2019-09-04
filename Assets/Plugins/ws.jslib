mergeInto(LibraryManager.library, {
    wsConnect: ( 
        url,
        gameObject = 'Setup', 
        onOpen = 'onOpen', 
        onClose = 'onClose', 
        onMessage = 'onMessage', 
        onError = 'onError',
    ) => {
        const emit = ( method, data ) => unityInstance.SendMessage(gameObject, method, data)
    
        window.WebSocketClient = new WebSocket(url)
        const ws = window.WebSocketClient
    
        ws.onopen = () => emit(onOpen)
        ws.onclose = () => emit(onClose)
        ws.onmessage = ({ data }) => emit(onMessage, data)
        ws.onerror = ({ data }) => emit(onError, data)
    },

    wsSend: ( data ) => {
        window.WebSocketClient.send( data )
    },

    wsClose: ( ) => {
        window.WebSocketClient.close()
    },
});