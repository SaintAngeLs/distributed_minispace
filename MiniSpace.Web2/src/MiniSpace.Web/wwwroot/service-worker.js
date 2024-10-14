// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development more difficult (changes would not
// be reflected on the first load after each change).
// self.addEventListener('fetch', () => { });
self.addEventListener('install', event => {
    console.log('Service worker installing...');
    // Add a call to skipWaiting here
    self.skipWaiting();
  });
  
  self.addEventListener('activate', event => {
    console.log('Service worker activating...');
  });
  
  self.addEventListener('fetch', event => {
    console.log('Fetching:', event.request.url);
    // Add fetch event handler here
  });
  