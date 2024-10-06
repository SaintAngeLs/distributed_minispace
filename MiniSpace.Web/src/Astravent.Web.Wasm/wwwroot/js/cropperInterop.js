var GLOBAL = GLOBAL || {};

var cropper = null;

// Function to set the DotNetReference from Blazor
GLOBAL.SetDotnetReference = function(dotNetReference) {
    GLOBAL.DotNetReference = dotNetReference;
};

// Function to display the selected image and initialize the cropper and buttons
function displayImageAndInitializeCropper(base64String, imageType) {
    console.log("Initializing cropper with image data");
    var imageContainer = document.getElementById('cropper-container');
    if (!imageContainer) {
        console.error('Image container not found');
        return;
    }

    imageContainer.innerHTML = `
        <div>
            <img id="image-to-crop" src="data:image/jpeg;base64,${base64String}" style="max-width: 100%;" />
            <button id="crop-image" class="btn btn-primary">Crop Image</button>
        </div>
    `;

    $('#cropperModal').modal('show');

    $('#cropperModal').on('shown.bs.modal', function () {
        var imageElement = document.getElementById('image-to-crop');
        if (imageElement) {
            var aspectRatio = imageType === "profile" ? 1 : 16 / 9; // 1:1 for profile, 16:9 for banner
            initializeCropper('image-to-crop', aspectRatio);
        } else {
            console.error("Image element not found in modal");
        }
    });

    var cropImageButton = document.getElementById('crop-image');
    if (cropImageButton) {
        cropImageButton.addEventListener('click', function() {
            console.log("Crop image button clicked");
            getCroppedImage('ReceiveCroppedImage');
        });
    }
}

// Function to display the cropped image in the modal
function displayCroppedImage(base64String) {
    var croppedImageContainer = document.getElementById('cropped-image-container');
    if (!croppedImageContainer) {
        console.error('Cropped image container not found');
        return;
    }

    croppedImageContainer.innerHTML = `
        <div>
            <img src="${base64String}" style="max-width: 100%;" />
        </div>
    `;
}

// Function to initialize the cropper on a specified image element with an aspect ratio
function initializeCropper(imageId, aspectRatio) {
    var imageElement = document.getElementById(imageId);
    if (!imageElement) {
        console.error('Image element not found');
        return;
    }

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
    console.log("Getting cropped image");
    if (!cropper) {
        console.error('Cropper instance not found');
        return;
    }

    var croppedCanvas = cropper.getCroppedCanvas();
    if (!croppedCanvas) {
        console.error('Failed to get cropped canvas.');
        return;
    }

    croppedCanvas.toBlob(function(blob) {
        if (!blob) {
            console.error('Blob is null or undefined.');
            return;
        }

        var reader = new FileReader();
        reader.onloadend = function() {
            var base64data = reader.result.split(',')[1]; // Get the base64 string
            if (GLOBAL.DotNetReference) {
                console.log("Invoking C# method with cropped image data");
                GLOBAL.DotNetReference.invokeMethodAsync(callbackMethodName, base64data);
            } else {
                console.error('DotNet reference not set.');
            }
        }
        reader.readAsDataURL(blob);
    }, 'image/png'); // Specify the format here if needed
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
    if (fileInput) {
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
    } else {
        console.error('File input element not found');
    }
});

window.showCropperModal = () => {
    $('#cropperModal').modal('show');
    $('#cropperModal').on('shown.bs.modal', function () {
        var imageContainer = document.getElementById('cropper-container');
        var img = imageContainer ? imageContainer.querySelector('img') : null;
        if (img) {
            initializeCropper('image-to-crop', 16 / 9);
        }
    });
};

window.hideCropperModal = () => {
    $('#cropperModal').modal('hide');
    destroyCropper();
};

// Function to display loaded image in gallery
window.displayImagePreview = function(base64Image) {
    const imgContainer = document.getElementById('imagePreviewContainer');
    if (imgContainer) {
        imgContainer.innerHTML = `<img src="${base64Image}" style="max-height: 200px;" />`;
    }
}
