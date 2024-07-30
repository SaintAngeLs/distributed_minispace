var GLOBAL = GLOBAL || {};

var cropper = null;

// Function to set the DotNetReference from Blazor
GLOBAL.SetDotnetReference = function(dotNetReference) {
    GLOBAL.DotNetReference = dotNetReference;
};

// Function to display the selected image and initialize the cropper and buttons
function displayImageAndInitializeCropper(base64String) {
    var imageContainer = document.getElementById('cropper-container');
    imageContainer.innerHTML = `
        <div style="">
        <img id="image-to-crop" src="data:image/jpeg;base64,${base64String}" style="max-width: 100%;" />
        <button id="crop-image" class="btn btn-primary">Crop Image</button>
        <button id="save-image" class="btn btn-primary" style="display: none;">Save Banner Image</button>
        </div>
    `;

    $('#cropperModal').modal('show');

    $('#cropperModal').on('shown.bs.modal', function () {
        var imageElement = document.getElementById('image-to-crop');
        if (imageElement) {
            initializeCropper('image-to-crop', 16 / 9);
        }
    });

    document.getElementById('crop-image').addEventListener('click', function() {
        getCroppedImage('ReceiveCroppedImage');
    });

    document.getElementById('save-image').addEventListener('click', function() {
        if (GLOBAL.DotNetReference) {
            GLOBAL.DotNetReference.invokeMethodAsync('SaveCroppedImage');
        } else {
            console.error('DotNet reference not set.');
        }
    });
}

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
                document.getElementById('save-image').style.display = 'inline-block';
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

// Attach event listener to file input for image upload
document.addEventListener('DOMContentLoaded', function () {
    var fileInput = document.getElementById('file-input');
    fileInput.addEventListener('change', function (event) {
        var file = event.target.files[0];
        if (file && file.type.match('image.*')) {
            var reader = new FileReader();
            reader.onload = function (e) {
                displayImageAndInitializeCropper(e.target.result.split(',')[1]);
            };
            reader.readAsDataURL(file);
        }
    });
});

window.showCropperModal = () => {
    $('#cropperModal').modal('show');
    $('#cropperModal').on('shown.bs.modal', function () {
        var imageContainer = document.getElementById('cropper-container');
        var img = imageContainer.querySelector('img');
        if (img) {
            initializeCropper('image-to-crop', 16 / 9);
        }
    });
};

window.hideCropperModal = () => {
    $('#cropperModal').modal('hide');
    destroyCropper();
};
