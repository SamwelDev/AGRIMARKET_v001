window.agriTheme = {
    set: function (theme) {
        document.documentElement.setAttribute("data-theme", theme);
        localStorage.setItem("agri-theme", theme);
    },

    get: function () {
        return localStorage.getItem("agri-theme");
    }
};
