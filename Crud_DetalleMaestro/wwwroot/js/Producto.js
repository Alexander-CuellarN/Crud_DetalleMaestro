import { showNotification } from "../js/utils/notifications.js"

document.querySelectorAll(".removeElement").forEach(element => {
    console.log(element)
    element.addEventListener("click", () => {
        let { identificator } = element.dataset
        let successResponse 
        fetch(`/Producto/Delete/${identificator}`, {
            method: "DELETE",
            headers: {
                "content-type": "application/json"
            },
            body: JSON.stringify({ id: identificator })
        })
            .then(data => {
                successResponse = data.ok
                return data.json()
            })
            .then(({ response }) => {
                if (!successResponse) throw new Error(response)
                document.getElementById(`tr_${identificator}`).remove()
                showNotification(response, "success")
            })
            .catch((error) => showNotification(error, "ERROR"))

    })
})