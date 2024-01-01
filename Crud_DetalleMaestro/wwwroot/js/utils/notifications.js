export const showNotification = (message, type) => {

    let NoticationContent = document.getElementById("NotificationContainer")
    console.log(NoticationContent.clas)
    NoticationContent.classList.add("show");

    let notificationText = document.getElementById("notificationContent")
    notificationText.innerHTML = ""
    notificationText.innerHTML = message

    if (type === 'ERROR') {
        NoticationContent.classList.remove("bg-success")
        NoticationContent.classList.add("bg-danger");
    } else {
        NoticationContent.classList.remove("bg-danger")
        NoticationContent.classList.add("bg-success");
    }
}