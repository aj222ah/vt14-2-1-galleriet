"use strict"

/*  */
var Gallery = {
    init: function () {
        var successDiv = document.getElementById("success");
        var errorZoneDiv = document.getElementById("errorZone");
        var successCloseLink, successCloseIcon, errorCloseLink, errorCloseIcon, closeSuccess, closeError;

        // Skapa "knapp" för stängning av "uppladdning ok"-ruta och lägg till den
        successCloseLink = document.createElement("a");
        successCloseLink.setAttribute("href", "#");
        successCloseLink.setAttribute("title", "Stäng");
        successCloseLink.setAttribute("class", "closeLink");
        successCloseLink.setAttribute("id", "successClose");
        successCloseIcon = document.createElement("img");
        successCloseIcon.setAttribute("src", "Scripts/images/Close-icon.png");
        successCloseIcon.setAttribute("alt", "Stäng");
        successCloseIcon.setAttribute("class", "closeIcon");

        successCloseLink.appendChild(successCloseIcon);
        successDiv.appendChild(successCloseLink);

        // Skapa "knapp" för stängning av felmeddelande-ruta och lägg till den
        errorCloseLink = document.createElement("a");
        errorCloseLink.setAttribute("href", "#");
        errorCloseLink.setAttribute("title", "Stäng");
        errorCloseLink.setAttribute("class", "closeLink");
        errorCloseLink.setAttribute("id", "errorClose");
        errorCloseIcon = document.createElement("img");
        errorCloseIcon.setAttribute("src", "Scripts/images/Close-icon.png");
        errorCloseIcon.setAttribute("alt", "Stäng");
        errorCloseIcon.setAttribute("class", "closeIcon");

        errorCloseLink.appendChild(errorCloseIcon);
        errorZoneDiv.appendChild(errorCloseLink);

        // Koppla "knapparna" till eventlyssnare
        closeSuccess = document.getElementById("successClose");
        closeSuccess.addEventListener("click", Gallery.divClose, false);
        closeError = document.getElementById("errorClose");
        closeError.addEventListener("click", Gallery.divClose, false);
    },

    // Funktion som stänger ett fönster
    divClose: function (e) {
        var closeID = this.id;
        var closeWindow;

        closeWindow = document.getElementById(closeID).parentElement;
        closeWindow.setAttribute("class", "hidden");

        e.preventDefault();
    },
};

window.addEventListener("load", Gallery.init, false);