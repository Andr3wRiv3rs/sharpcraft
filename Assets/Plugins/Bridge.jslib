let ws

const Connect = ( 
    url,
    gameObject = 'Setup', 
    onOpen = 'onOpen', 
    onClose = 'onClose', 
    onMessage = 'onMessage', 
    onError = 'onError',
) => {
    const emit = ( data, method ) => console.log("ok")//unityInstance.SendMessage(gameObject, method, data)

    ws = new WebSocket(url)

    ws.onopen = () => emit(data, onOpen)
    ws.onclose = () => emit(data, onClose)
    ws.onmessage = ({ data }) => emit(data, onMessage)
    ws.onerror = ({ data }) => emit(data, onError)
}

const Send = ( data ) => {
    ws.send( data )
}

const Close = ( ) => {
    ws.close()
}

mergeInto(LibraryManager.library, {
    wsConnect: Connect,
    wsSend: Send,
    wsClose: Close,
});