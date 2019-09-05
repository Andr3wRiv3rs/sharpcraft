import './zip.js'
import './inflate.js'

window.zip.useWebWorkers = false

var requestFileSystem = window.webkitRequestFileSystem || window.mozRequestFileSystem || window.requestFileSystem;

function onerror(message) {
    console.log(message);
}

function createTempFile(callback) {
    var tmpFilename = "tmp.dat";

    requestFileSystem(TEMPORARY, 4 * 1024 * 1024 * 1024, function(filesystem) {
        function create() {
            filesystem.root.getFile(tmpFilename, {
                create : true
            }, function(zipFile) {
                callback(zipFile);
            });
        }

        filesystem.root.getFile(tmpFilename, null, function(entry) {
            entry.remove(create, create);
        }, create);
    });
}

var model = (function() {
    var URL = window.webkitURL || window.mozURL || window.URL;

    return {
        getEntries : function(file, onend) {
            zip.createReader(new zip.BlobReader(file), function(zipReader) {
                zipReader.getEntries(onend);
            }, onerror);
        },

        getEntryFile : function(entry, creationMethod, onend, onprogress) {
            var writer, zipFileEntry;

            function getData() {
                entry.getData(writer, function(blob) {
                    var blobURL = creationMethod == "Blob" ? URL.createObjectURL(blob) : zipFileEntry.toURL();
                    onend(blobURL);
                }, onprogress);
            }

            if (creationMethod == "Blob") {
                writer = new zip.BlobWriter();
                getData();
            } else {
                createTempFile(function(fileEntry) {
                    zipFileEntry = fileEntry;
                    writer = new zip.FileWriter(zipFileEntry);
                    getData();
                });
            }
        }
    };
})();

var fileInput = document.createElement('input');
fileInput.type = 'file';

function download(entry, callback) {
    model.getEntryFile(entry, "Blob", function(blobURL) {
        // callback(entry.filename, blobURL)
        console.log(entry.filename, blobURL)
    }, function() {});
}

fileInput.addEventListener('change', function() {
    fileInput.disabled = true;
    model.getEntries(fileInput.files[0], function(entries) {
        entries.forEach(function(entry) {
            if (/\.png/.test(entry.filename)) download(entry, console.log)
        });
    });
}, false);

window.UnzipFilePrompt = function ( ) {
    window.addEventListener('click', () => fileInput.click())
};
