import { showNotification } from "../js/utils/notifications.js"

document.querySelectorAll(".removeElement").forEach(element => {
    console.log(element)
    element.addEventListener("click", () => {
        let { identificator } = element.dataset
        let tokenRfc =  document.querySelector("input[name='__RequestVerificationToken']").value

        fetch(`/Producto/Delete/${identificator}`, {
            method: "DELETE",
            headers: {
                "content-type": "application/json"
            },
            body: JSON.stringify({ id: identificator, __RequestVerificationToken: tokenRfc })
        })
            .then(data => data.json())
            .then(({ response }) => {
                document.getElementById(`tr_${identificator}`).remove()
                showNotification(response, "success")
            })
            .catch(/*() => showNotification("No se ha podido eliminar el producto", "ERROR")*/ error => console.log(error))

    })
})