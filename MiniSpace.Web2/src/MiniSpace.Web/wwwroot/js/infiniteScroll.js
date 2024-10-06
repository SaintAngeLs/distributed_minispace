window.infiniteScroll = {
    initialize: function (dotnetHelper) {
        let options = {
            root: null,
            rootMargin: "0px",
            threshold: 0.1
        };

        let observer = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    dotnetHelper.invokeMethodAsync("LoadMoreData");
                }
            });
        }, options);

        observer.observe(document.querySelector('#end-of-list'));
    }
};
