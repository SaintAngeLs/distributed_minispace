// Ensure GLOBAL is defined and accessible
var GLOBAL = GLOBAL || {};

var cropper = null; 

// Function to set the DotNetReference from Blazor
GLOBAL.SetDotnetReference = function(dotNetReference) {
    console.log("Setting DotNet reference...");
    GLOBAL.DotNetReference = dotNetReference;
    console.log("DotNet reference set:", GLOBAL.DotNetReference);
};
// Function to initialize the cropper on a specified image element with an aspect ratio
function initializeCropper(imageId, aspectRatio) {
    var imageElement = document.getElementById(imageId);
    if (cropper) {
        cropper.destroy();
    }
    cropper = new Cropper(imageElement, {
        aspectRatio: aspectRatio,
        viewMode: 2,
        autoCropArea: 1,
        restore: false,
        guides: true,
        center: true,
        highlight: true,
        cropBoxMovable: true,
        cropBoxResizable: true
    });
}

// Function to get the cropped image and invoke a C# method asynchronously via JSInterop
function getCroppedImage(callbackMethodName) {
    var croppedCanvas = cropper.getCroppedCanvas();
    croppedCanvas.toBlob(function(blob) {
        var reader = new FileReader();
        reader.onload = function() {
            if (GLOBAL.DotNetReference) {
                GLOBAL.DotNetReference.invokeMethodAsync(callbackMethodName, reader.result);
            } else {
                console.error('DotNet reference not set.');
            }
        };
        reader.readAsDataURL(blob);
    });
}

// Function to cleanly destroy the cropper instance
function destroyCropper() {
    if (cropper) {
        cropper.destroy();
        cropper = null;
    }
}
