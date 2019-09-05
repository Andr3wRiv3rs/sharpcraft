import './zip.js'
import './inflate.js'

window.zip.useWebWorkers = false

const getEntries = ( file ) => new Promise (
    ( resolve, reject ) => zip.createReader(
        new zip.BlobReader(file), 
        zipReader => zipReader.getEntries(resolve), 
        reject
    )
)

const getEntryFile = ( entry, onprogress = ( ) => { } ) => new Promise(
    ( resolve ) => entry.getData(
        new zip.BlobWriter(), 
        blob => resolve(URL.createObjectURL(blob)), 
        onprogress
    )
)

const fileInput = document.createElement('input')
fileInput.type = 'file'

fileInput.onchange = async ( event ) => {
    try {
        const entries = (await getEntries(fileInput.files[0])).filter(
            ({ filename }) => /\.png/.test( filename )
        )

        const result = await Promise.all(
            entries.map( async entry => [entry.filename, await getEntryFile( entry )] )
        )

        console.log(Object.fromEntries(result))
    } catch ( error ) {
        console.error(error)
    }
}

window.UnzipFilePrompt = _ => window.addEventListener(
    'click', 
    _ => fileInput.click()
)
